using Microsoft.AspNetCore.Mvc;
using DynaDevFE.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace DynaDevFE.Controllers
{
    public class CuaHangController : Controller
    {
        private readonly HttpClient _httpClient;

        public CuaHangController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Action để hiển thị danh sách sản phẩm
        public async Task<IActionResult> Index(string searchQuery)
        {
            string apiUrl = string.IsNullOrEmpty(searchQuery)
                ? "https://localhost:7101/api/Products"
                : $"https://localhost:7101/api/Products/Search?query={searchQuery}";

            var response = await _httpClient.GetStringAsync(apiUrl);
            var products = JsonConvert.DeserializeObject<List<SanPhamViewModel>>(response);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                // Nếu là AJAX request, trả về JSON
                return Json(products);
            }
            // Nếu không, trả về view
            return View(products);
        }

    }
}
