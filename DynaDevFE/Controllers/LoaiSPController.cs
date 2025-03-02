using DynaDevAPI.Models;
using DynaDevFE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace DynaDevFE.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class LoaiSPController : Controller
    {

        private readonly HttpClient _httpClient;

        public LoaiSPController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7101/");
        }


        public async Task<IActionResult> Index()
        {
            var apiUrl = "api/LoaiSP"; // API endpoint để lấy danh mục sản phẩm
            var response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                var loaiSPList = JsonSerializer.Deserialize<List<LoaiSPViewModel>>(jsonData, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return View(loaiSPList);
            }

            return View(new List<LoaiSPViewModel>());
        }



        public IActionResult Create()
        {
            return View();
        }

        // POST: LoaiSP/Create
        [HttpPost]
        public async Task<IActionResult> Create(LoaiSPViewModel loaiSP)
        {
            if (!ModelState.IsValid)
            {
                return View(loaiSP);
            }

            var jsonData = JsonSerializer.Serialize(loaiSP);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/LoaiSP", content);
            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Response: {responseContent}");


            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Không thể thêm danh mục sản phẩm.");
            return View(loaiSP);
        }


        // GET: LoaiSP/Edit/{id}
        public async Task<IActionResult> Edit(string id)
        {
            var response = await _httpClient.GetAsync($"api/LoaiSP/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var jsonData = await response.Content.ReadAsStringAsync();
            var loaiSP = JsonSerializer.Deserialize<LoaiSPViewModel>(jsonData, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return View(loaiSP);
        }

        // POST: LoaiSP/Edit/{id}
        [HttpPost]
        public async Task<IActionResult> Edit(string id, LoaiSPViewModel loaiSP)
        {
            if (!ModelState.IsValid)
            {
                return View(loaiSP);
            }

            var jsonData = JsonSerializer.Serialize(loaiSP);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            // Gửi yêu cầu PUT đến API
            var response = await _httpClient.PutAsync($"api/LoaiSP/{id}", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Cập nhật danh mục sản phẩm thất bại.");
            return View(loaiSP);
        }


        // GET: LoaiSP/Delete/{id}
        public async Task<IActionResult> Delete(string id)
        {
            var response = await _httpClient.DeleteAsync($"api/LoaiSP/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return NotFound();
        }


    }
}
