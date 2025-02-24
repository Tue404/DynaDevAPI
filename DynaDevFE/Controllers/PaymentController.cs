using DynaDevAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using System.Net.Http.Json;
using DynaDevFE.Models;

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
            string maKH = Request.Cookies["MaKH"];

            if (string.IsNullOrEmpty(maKH))
            {
                return RedirectToAction("DangNhap", "TaiKhoan");
            }

            ViewBag.MaKH = maKH;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DatHang([FromBody] DatHangRequest request)
        {
            if (string.IsNullOrEmpty(request.MaKH))
            {
                return Json(new { success = false, message = "Bạn chưa đăng nhập!" });
            }

            var apiUrl = "https://localhost:7101/api/DonHang/DatHang";
            var jsonData = JsonSerializer.Serialize(request);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(apiUrl, content);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<ApiResponse>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return Json(new { success = true, MaDH = data.MaDH });
            }

            return Json(new { success = false, message = "Đặt hàng thành công!" });
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}
