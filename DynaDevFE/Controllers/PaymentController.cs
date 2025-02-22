using DynaDevAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace DynaDevFE.Controllers
{
    public class PaymentController : Controller
    {
        private readonly HttpClient _httpClient;

        public PaymentController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet]
        public IActionResult ThongTinThanhToan()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DatHang([FromBody] DatHangRequest request)
        {
            if (request == null || request.GioHang == null || !request.GioHang.Any())
            {
                return BadRequest(new { Message = "Giỏ hàng trống!" });
            }

            var requestData = new
            {
                MaKH = "KH01", // Lấy từ user đăng nhập nếu có
                DiaChiNhanHang = request.DiaChiNhanHang,
                PhuongThucThanhToan = request.PhuongThucThanhToan,
                GioHang = request.GioHang
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://localhost:7101/api/DonHang/DatHang", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                return Ok(new { Message = "Đặt hàng thành công!" }); // Trả JSON về frontend để xóa localStorage
            }
            else
            {
                return StatusCode(500, new { Message = "Đặt hàng thất bại, vui lòng thử lại." });
            }
        }


        public IActionResult ThanhCong()
        {
            return View();
        }
    }
}
