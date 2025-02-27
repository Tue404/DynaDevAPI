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
            try
            {
                var response = await _httpClient.GetAsync($"api/Customer?page={page}&pageSize={pageSize}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();

                    // Deserialize cả dữ liệu khách hàng và thông tin phân trang
                    var responseData = JsonSerializer.Deserialize<ResponseWithPagination<KhachHangViewModel>>(jsonData, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    ViewBag.Pagination = responseData.Pagination; // Truyền thông tin phân trang vào View
                    return View(responseData.Data);
                }

                ViewBag.ErrorMessage = $"Lỗi từ API: {response.StatusCode}";
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Đã xảy ra lỗi: {ex.Message}";
            }

            return View(new List<KhachHangViewModel>()); // Trả về danh sách rỗng khi lỗi
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

        //public async Task<JsonResult> Search(string query)
        //{
        //    if (string.IsNullOrWhiteSpace(query))
        //    {
        //        return Json(new List<KhachHangViewModel>());
        //    }

        //        var response = await _httpClient.GetAsync($"api/Customer/Search?query={Uri.EscapeDataString(query)}");
        //        if (response.IsSuccessStatusCode)
        //        {
        //            var jsonData = await response.Content.ReadAsStringAsync();
        //            var customers = JsonSerializer.Deserialize<List<KhachHangViewModel>>(jsonData, new JsonSerializerOptions
        //            {
        //                PropertyNameCaseInsensitive = true
        //            });

        //            return Json(customers);
        //        }       
        //   return View();
        //}
        // GET: Hiển thị form chỉnh sửa khách hàng
        public async Task<IActionResult> Edit(string id)
        {
            var maKH = Request.Cookies["MaKH"];
            if (string.IsNullOrEmpty(maKH))
            {
                return NotFound("Không tìm thấy mã khách hàng.");
            }

            try
            {
                // Lấy thông tin khách hàng từ API
                var response = await _httpClient.GetAsync($"https://localhost:7101/api/TTCN/{maKH}");

                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return NotFound($"Không tìm thấy thông tin khách hàng. Lỗi: {errorMessage}");
                }

                var khachHang = await response.Content.ReadFromJsonAsync<KhachHangViewModel>();
                if (khachHang == null)
                {
                    return NotFound("Dữ liệu khách hàng trống.");
                }

                return View(khachHang);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi kết nối đến API: {ex.Message}");
            }
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
                    foreach (var error in entry.Value.Errors)
                    {
                        Console.WriteLine($"Error in {entry.Key}: {error.ErrorMessage}");
                    }
                }

                ModelState.AddModelError("", "Dữ liệu không hợp lệ.");
                return View(model); // Trả lại model để giữ dữ liệu
            }

            try
            {
                var json = JsonSerializer.Serialize(model, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                Console.WriteLine("Request JSON: " + json);

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"https://localhost:7101/api/TTCN/{model.MaKH}", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Cập nhật thông tin thành công!";
                    return RedirectToAction("Index", "Home");
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine("API Error: " + errorContent);
                ModelState.AddModelError("", $"Lỗi từ API: {errorContent}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi hệ thống: {ex.Message}");
                ModelState.AddModelError("", $"Đã xảy ra lỗi: {ex.Message}");
            }

            return View(model);
        }
    }
}
