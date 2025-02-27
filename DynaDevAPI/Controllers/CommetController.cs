using DynaDevAPI.Data;
using DynaDevAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DynaDevAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommetController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public CommetController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: api/Customer?page={page}&pageSize={pageSize}
        [HttpGet]
        public async Task<ActionResult> GetDanhGias([FromQuery] int page = 1, [FromQuery] int pageSize = 8)
        {
            try
            {
                if (page <= 0 || pageSize <= 0)
                {
                    return BadRequest(new { message = "Page và PageSize phải lớn hơn 0." });
                }

                var totalCommets = await _db.DanhGias.CountAsync();
                var totalPages = (int)Math.Ceiling(totalCommets / (double)pageSize);

                if (page > totalPages)
                {
                    return NotFound(new { message = "Trang không tồn tại." });
                }

                var commets = await _db.DanhGias
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return Ok(new
                {
                    data = commets.Select(kh => new
                    {
                        kh.MaDanhGia,
                        kh.MaKH,
                        kh.MaSP,
                        kh.BinhLuan,
                        kh.TrangThai,
                        kh.DiemDanhGia,
                        kh.NgayDanhGia
                    }),
                    pagination = new
                    {
                        currentPage = page,
                        pageSize = pageSize,
                        totalItems = totalCommets,
                        totalPages = totalPages
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi lấy danh sách đánh giá.", error = ex.Message });
            }
        }

        // GET: api/Customer/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<KhachHang>> GetDanhGia(string id)
        {
            try
            {
                var danhGia = await _db.DanhGias.FindAsync(id);

                if (danhGia == null)
                {
                    return NotFound(new { message = "Không tìm thấy đánh giá." });
                }

                return Ok(new
                {
                    danhGia.MaDanhGia,
                    danhGia.MaKH,
                    danhGia.MaSP,
                    danhGia.BinhLuan,
                    danhGia.TrangThai,
                    danhGia.DiemDanhGia,
                    danhGia.NgayDanhGia
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi lấy thông tin đánh giá.", error = ex.Message });
            }
        }

        // GET: api/Customer/Search?query={query}
        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<DanhGia>>> SearchDanhGia([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest(new { message = "Từ khóa tìm kiếm không hợp lệ." });
            }

            try
            {
                // Chuyển query về dạng chữ thường để tìm kiếm không phân biệt hoa thường
                var lowercaseQuery = query.ToLower();

                // Tìm kiếm trong các trường: Mã KH, Tên KH, Email, SDT, Địa chỉ
                var results = await _db.DanhGias
                    .Where(kh =>
                        kh.MaKH.ToLower().Contains(lowercaseQuery) ||
                        kh.MaDanhGia.ToLower().Contains(lowercaseQuery) ||
                          kh.MaSP.ToLower().Contains(lowercaseQuery) ||
                          kh.TrangThai.ToLower().Contains(lowercaseQuery) ||
                        kh.BinhLuan.ToLower().Contains(lowercaseQuery))
                    .ToListAsync();

                // Nếu không có kết quả
                if (results == null || !results.Any())
                {
                    return NotFound(new { message = "Không tìm thấy đánh giá nào phù hợp." });
                }

                return Ok(results.Select(danhGia => new
                {
                    danhGia.MaDanhGia,
                    danhGia.MaKH,
                    danhGia.MaSP,
                    danhGia.BinhLuan,
                    danhGia.TrangThai,
                    danhGia.DiemDanhGia,
                    danhGia.NgayDanhGia
                }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi tìm kiếm đánh giá.", error = ex.Message });
            }
        }

        // PUT: api/Commet/Approve/{MaDanhGia}
        [HttpPut("Approve/{MaDanhGia}")]
        public async Task<IActionResult> ApproveComment(string MaDanhGia)
        {
            try
            {
                var danhGia = await _db.DanhGias.FindAsync(MaDanhGia);

                if (danhGia == null)
                {
                    return NotFound(new { message = "Không tìm thấy bình luận." });
                }

                // Update the TrangThai to "Đã Duyệt"
                danhGia.TrangThai = "Đã Duyệt";

                _db.DanhGias.Update(danhGia);
                await _db.SaveChangesAsync();

                return Ok(new { success = true, message = "Bình luận đã được duyệt thành công!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Lỗi khi duyệt bình luận.", error = ex.Message });
            }
        }


        // DELETE: api/Commet/{MaDanhGia}
        [HttpDelete("{MaDanhGia}")]
        public async Task<IActionResult> Delete(string MaDanhGia)
        {
            try
            {
                var danhGia = await _db.DanhGias.FindAsync(MaDanhGia);

                if (danhGia == null)
                {
                    return NotFound(new { message = "Không tìm thấy đánh giá." });
                }

                _db.DanhGias.Remove(danhGia);
                await _db.SaveChangesAsync();

                return Ok(new { success = true, message = "Xóa đánh giá thành công!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Lỗi khi xóa đánh giá.", error = ex.Message });
            }
        }

    }
}
