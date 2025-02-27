using DynaDevAPI.Models;
using DynaDevFE.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace DynaDevFE.Controllers
{
    public class CommetController : Controller
    {
        private readonly HttpClient _httpClient;
        public CommetController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7101/");
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 8)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/Commet?page={page}&pageSize={pageSize}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();


                    var responseData = JsonSerializer.Deserialize<ResponseWithPagination<DanhGiaViewModel>>(jsonData, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    ViewBag.Pagination = responseData.Pagination;
                    return View(responseData.Data);
                }

                ViewBag.ErrorMessage = $"Lỗi từ API: {response.StatusCode}";
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Đã xảy ra lỗi: {ex.Message}";
            }

            return View(new List<DanhGiaViewModel>()); // Trả về danh sách rỗng khi lỗi
        }

        // Class giúp deserialize dữ liệu trả về
        public class ResponseWithPagination<T>
        {
            public List<T> Data { get; set; }
            public Pagination Pagination { get; set; }
        }

        public class Pagination
        {
            public int CurrentPage { get; set; }
            public int PageSize { get; set; }
            public int TotalItems { get; set; }
            public int TotalPages { get; set; }
        }

        public async Task<JsonResult> Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return Json(new List<DanhGiaViewModel>());
            }

            try
            {
                var response = await _httpClient.GetAsync($"api/Commet/Search?query={Uri.EscapeDataString(query)}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    var customers = JsonSerializer.Deserialize<List<DanhGiaViewModel>>(jsonData, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return Json(customers);
                }

                return Json(new List<DanhGiaViewModel>()); // Trả về danh sách rỗng nếu không tìm thấy
            }
            catch (Exception)
            {
                return Json(new List<DanhGiaViewModel>()); // Xử lý lỗi bằng cách trả về danh sách rỗng
            }
        }

    }
}
