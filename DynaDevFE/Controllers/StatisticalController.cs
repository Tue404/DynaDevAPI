using DynaDevFE.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace DynaDevFE.Controllers
{
    public class StatisticalController : Controller
    {
        private readonly HttpClient _httpClient;

        public StatisticalController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7101/");
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/Statistical/DoanhThuTheoNgayThangNam");
                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    var doanhThu = JsonSerializer.Deserialize<List<DoanhThuTheoNgayThangNamViewModel>>(jsonData, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return View(doanhThu);
                }

                ViewBag.ErrorMessage = $"Lỗi từ API: {response.StatusCode}";
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Đã xảy ra lỗi: {ex.Message}";
            }

            return View(new List<DoanhThuTheoNgayThangNamViewModel>());
        }
    }
}
