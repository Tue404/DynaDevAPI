using DynaDevAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DynaDevAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticalController : Controller
    {
        private readonly ApplicationDbContext _db;

        public StatisticalController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet("DoanhThuTheoNgayThangNam")]
        public async Task<IActionResult> GetDoanhThuTheoNgayThangNam([FromQuery] int? year, [FromQuery] int? month, [FromQuery] DateTime? date)
        {
            try
            {
                var query = _db.DonHangs.AsQueryable();

                // Lọc dữ liệu
                if (date.HasValue)
                {
                    query = query.Where(dh => dh.ThoiGianDatHang.Date == date.Value.Date);
                }
                else if (year.HasValue && month.HasValue)
                {
                    query = query.Where(dh => dh.ThoiGianDatHang.Year == year.Value && dh.ThoiGianDatHang.Month == month.Value);
                }
                else if (year.HasValue)
                {
                    query = query.Where(dh => dh.ThoiGianDatHang.Year == year.Value);
                }

                // Kiểm tra dữ liệu
                if (!await query.AnyAsync())
                {
                    return Ok(new List<object>());
                }

                // Xử lý doanh thu
                var doanhThuTheoNgayThangNam = await query
                    .GroupBy(dh => new { dh.ThoiGianDatHang.Year, dh.ThoiGianDatHang.Month, dh.ThoiGianDatHang.Day })
                    .Select(g => new
                    {
                        Nam = g.Key.Year,
                        Thang = g.Key.Month,
                        Ngay = g.Key.Day,
                        TongDoanhThu = g.Sum(dh => dh.TongTien),
                        TongDonHang = g.Count() 
                    })
                    .OrderBy(g => g.Nam)
                    .ThenBy(g => g.Thang)
                    .ThenBy(g => g.Ngay)
                    .ToListAsync();

                return Ok(doanhThuTheoNgayThangNam);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, new { message = "Lỗi khi xử lý dữ liệu.", error = ex.Message });
            }
        }
    }
}
