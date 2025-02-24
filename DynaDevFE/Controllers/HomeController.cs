using DynaDevAPI.Models;
using DynaDevFE.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;

namespace DynaDevFE.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient;
        public HomeController(ILogger<HomeController> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
        {
            // URL của API để lấy danh sách sản phẩm
            var apiUrl = "https://localhost:7101/api/Products";
            var response = await _httpClient.GetAsync(apiUrl);
            
            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                var products = JsonSerializer.Deserialize<List<SanPhamViewModel>>(jsonData, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                // Gọi API để lấy danh sách ảnh cho từng sản phẩm
                foreach (var product in products)
                {
                    var imagesResponse = await _httpClient.GetAsync($"https://localhost:7101/api/Products/GetImagesByProduct/{product.MaSP}");
                    if (imagesResponse.IsSuccessStatusCode)
                    {
                        var imagesJson = await imagesResponse.Content.ReadAsStringAsync();
                        var imageObjects = JsonSerializer.Deserialize<List<AnhSP>>(imagesJson, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                        // Chỉ lấy đường dẫn ảnh
                        product.DanhSachAnh = imageObjects.Select(img => img.TenAnh).ToList();
                    }

                }

                return View(products);
            }

            // Nếu có lỗi, trả về View rỗng
            return View(new List<SanPhamViewModel>());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
