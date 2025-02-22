using DynaDevAPI.Data;
using DynaDevAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DynaDevAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonHangController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public DonHangController(ApplicationDbContext db)
        {
            _db = db;
        }
        [HttpPost("DatHang")]
        public async Task<IActionResult> DatHang([FromBody] DatHangRequest request)
        {
            if (request == null || request.GioHang == null || !request.GioHang.Any())
            {
                return BadRequest(new { Message = "Giỏ hàng trống!" });
            }

            try
            {
                // **Bước 1: Tạo đơn hàng mới**
                var donHang = new DonHang
                {
                    MaDH = Guid.NewGuid().ToString(), // ✅ Đảm bảo MaDH không NULL
                    MaKH = request.MaKH,
                    DiaChiNhanHang = request.DiaChiNhanHang,
                    PhuongThucThanhToan = request.PhuongThucThanhToan,
                    ThoiGianDatHang = DateTime.Now,
                    TongTien = request.GioHang.Sum(x => x.Gia * x.SoLuong),
                    PaymentStatusId = request.PhuongThucThanhToan == "COD" ? 1 : 0,
                    OrderStatusId = 1
                };

                // **Bước 2: Lưu đơn hàng vào database trước**
                _db.DonHangs.Add(donHang);
                await _db.SaveChangesAsync(); // 🔹 Save trước để có MaDH trong database

                // **Bước 3: Kiểm tra nếu giỏ hàng không rỗng**
                if (request.GioHang != null && request.GioHang.Any())
                {
                    foreach (var sp in request.GioHang)
                    {
                        var chiTiet = new ChiTietDonHang
                        {
                            MaChiTiet = Guid.NewGuid().ToString(), // ✅ Đảm bảo khóa chính
                            MaDH = donHang.MaDH,  // ✅ Lấy MaDH từ đơn hàng đã lưu
                            MaSP = sp.MaSP,
                            SoLuong = sp.SoLuong,
                            Gia = sp.Gia
                        };
                        _db.ChiTietDonHangs.Add(chiTiet);
                    }

                    // **Bước 4: Lưu danh sách sản phẩm vào database**
                    await _db.SaveChangesAsync(); // 🔹 Save toàn bộ sản phẩm vào đơn hàng
                }

                return Ok(new { Message = "Đặt hàng thành công!", MaDH = donHang.MaDH });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Đặt hàng thất bại!", Error = ex.Message });
            }
        }
    }
}
