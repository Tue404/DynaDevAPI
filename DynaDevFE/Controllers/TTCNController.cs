using DynaDevAPI.Models;
using DynaDevFE.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DynaDevFE.Controllers
{
    public class TTCNController : Controller
    {
        private readonly HttpClient _httpClient;
        public TTCNController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 8)
        {

            return View();
        }
        // GET: Hiển thị form chỉnh sửa khách hàng
        public async Task<IActionResult> Edit(string id)
        {
            var maKH = Request.Cookies["MaKH"];
            if (string.IsNullOrEmpty(maKH))
            {
                return NotFound("Không tìm thấy mã khách hàng.");
            }

            // Lấy thông tin khách hàng từ API
            var response = await _httpClient.GetAsync($"https://localhost:7101/api/TTCN/{maKH}");
            if (response.IsSuccessStatusCode)
            {
                var khachHang = await response.Content.ReadFromJsonAsync<KhachHangViewModel>();
                return View(khachHang); // Truyền thông tin khách hàng vào ViewModel
            }

            return NotFound("Không tìm thấy thông tin khách hàng.");
        }
        // POST: Cập nhật thông tin khách hàng
        [HttpPost]
        public async Task<IActionResult> Edit(KhachHangViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Ghi lại các lỗi trong ModelState
                foreach (var entry in ModelState)
                {
                    var key = entry.Key;
                    var errors = entry.Value.Errors;
                    foreach (var error in errors)
                    {
                        Console.WriteLine($"Error in {key}: {error.ErrorMessage}");
                    }
                }

                ModelState.AddModelError("", "Dữ liệu không hợp lệ.");
                return View(model); // Trả lại model để giữ dữ liệu
            }

            // Tiến hành cập nhật thông tin nếu ModelState hợp lệ
            try
            {
                var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"https://localhost:7101/api/TTCN/{model.MaKH}", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Cập nhật thông tin thành công!";
                    return RedirectToAction("Index", "Home");
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError("", $"Lỗi từ API: {errorContent}");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Đã xảy ra lỗi: {ex.Message}");
            }

            return View(model);
        }
    }
}
