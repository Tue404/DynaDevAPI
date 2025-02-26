using Microsoft.AspNetCore.Mvc;
using DynaDevFE.Models;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DynaDevFE.Controllers
{
    public class DanhMucController : Controller
    {
        private readonly HttpClient _httpClient;

        public DanhMucController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index(string maLoai)
        {
            // Gọi API để lấy các sản phẩm từ backend
            var response = await _httpClient.GetAsync($"https://localhost:7101/api/Products");
            var productsJson = await response.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<List<SanPhamViewModel>>(productsJson);

            // Lọc sản phẩm theo maLoai
            var filteredProducts = products.Where(p => p.MaLoai == maLoai).ToList();

            // Truyền dữ liệu vào view
            ViewBag.MaLoai = maLoai;
            return View(filteredProducts);  
        }

       

    }
}
