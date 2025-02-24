using DynaDevAPI.Data;
using DynaDevAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [HttpGet("{maDH}")]
        public async Task<IActionResult> GetDonHang(string maDH)
        {
            var donHang = await _db.DonHangs
                .Include(dh => dh.KhachHang) // ✅ JOIN với bảng KhachHang
                .FirstOrDefaultAsync(dh => dh.MaDH == maDH);

            if (donHang == null)
            {
                return NotFound(new { Message = "Không tìm thấy đơn hàng!" });
            }

            var response = new
            {
                MaDH = donHang.MaDH,
                MaKH = donHang.MaKH,
                TenNguoiNhan = donHang.TenNguoiNhan,
                Sdt = donHang.SoDienThoai,
                DiaChiNhanHang = donHang.DiaChiNhanHang,
                PhuongThucThanhToan = donHang.PhuongThucThanhToan,
                ThoiGianDatHang = donHang.ThoiGianDatHang,
                TongTien = donHang.TongTien
            };

            return Ok(response);
        }

        [HttpGet("GetProvinces")]
        public async Task<IActionResult> GetProvinces()
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("https://provinces.open-api.vn/api/?depth=1");

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Lỗi khi lấy danh sách tỉnh/thành.");
            }

            var jsonData = await response.Content.ReadAsStringAsync();
            return Content(jsonData, "application/json");
        }

        [HttpGet("GetDistricts")]
        public async Task<IActionResult> GetDistricts(string provinceId)
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync($"https://provinces.open-api.vn/api/districts/{provinceId}?depth=2");

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Lỗi khi lấy danh sách quận/huyện.");
            }

            var jsonData = await response.Content.ReadAsStringAsync();
            return Content(jsonData, "application/json");
        }

        [HttpGet("GetWards")]
        public async Task<IActionResult> GetWards(string districtId)
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync($"https://provinces.open-api.vn/api/wards/{districtId}?depth=3");

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Lỗi khi lấy danh sách xã/phường.");
            }

            var jsonData = await response.Content.ReadAsStringAsync();
            return Content(jsonData, "application/json");
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
                    TenNguoiNhan = request.TenKH,
                    SoDienThoai = request.SoDienThoai,
                    DiaChiNhanHang = request.DiaChiNhanHang,
                    PhuongThucThanhToan = request.PhuongThucThanhToan,
                    ThoiGianDatHang = DateTime.Now,
                    TongTien = request.GioHang.Sum(x => (decimal)(x.Gia * x.SoLuong)),
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

                return Ok(new { success = true, Message = "Đặt hàng thành công!", MaDH = donHang.MaDH });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Đặt hàng thất bại!", Error = ex.Message });
            }
        }
    }
}
