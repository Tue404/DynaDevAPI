using DynaDevAPI.Models;
using DynaDevFE.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
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
            _httpClient.BaseAddress = new Uri("https://localhost:7101/");
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

        public async Task<JsonResult> Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return Json(new List<KhachHangViewModel>());
            }

            try
            {
                var response = await _httpClient.GetAsync($"api/Customer/Search?query={Uri.EscapeDataString(query)}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    var customers = JsonSerializer.Deserialize<List<KhachHangViewModel>>(jsonData, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return Json(customers);
                }       

                return Json(new List<KhachHangViewModel>()); // Trả về danh sách rỗng nếu không tìm thấy
            }
            catch (Exception)
            {
                return Json(new List<KhachHangViewModel>()); // Xử lý lỗi bằng cách trả về danh sách rỗng
            }
        }

        // GET: Hiển thị form tạo mới khách hàng
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tạo mới khách hàng
        [HttpPost]
        public async Task<IActionResult> Create(KhachHangViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Dữ liệu không hợp lệ.");
                return View(model);
            }

            var khachHang = new KhachHang
            {
                MaKH = $"KH{new Random().Next(100, 999)}", // Tạo mã khách hàng ngẫu nhiên
                TenKH = model.TenKH,
                Email = model.Email,
                MatKhau = model.MatKhau,
                SDT = model.SDT,
                DiaChi = model.DiaChi,
                TinhTrang = model.TinhTrang,
                NgayDangKy = DateTime.Now
            };

            try
            {
                var jsonData = JsonSerializer.Serialize(khachHang);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("api/Customer", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Thêm khách hàng thành công!";
                    return RedirectToAction("Index");
                }

                // Xử lý lỗi từ API
                var errorContent = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError("", $"Lỗi từ API: {errorContent}");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Đã xảy ra lỗi: {ex.Message}");
            }

            return View(model);
        }

        // GET: Hiển thị form chỉnh sửa khách hàng
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound("Mã khách hàng không hợp lệ.");
            }

            try
            {
                var response = await _httpClient.GetAsync($"api/Customer/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    var customer = JsonSerializer.Deserialize<KhachHangViewModel>(jsonData, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return View(customer);
                }

                ViewBag.ErrorMessage = "Không thể tải thông tin khách hàng.";
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Đã xảy ra lỗi: {ex.Message}";
            }

            return View("Error");
        }

        // POST: Cập nhật thông tin khách hàng
        [HttpPost]
        public async Task<IActionResult> Edit(KhachHangViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Dữ liệu không hợp lệ.");
                return View(model);
            }

            try
            {
                // Lấy thông tin khách hàng từ API để đảm bảo các trường cố định
                var responseGet = await _httpClient.GetAsync($"api/Customer/{model.MaKH}");
                if (!responseGet.IsSuccessStatusCode)
                {
                    var errorContent = await responseGet.Content.ReadAsStringAsync();
                    ModelState.AddModelError("", $"Không thể tải thông tin khách hàng. Chi tiết: {errorContent}");
                    return View(model);
                }

                // Đọc dữ liệu khách hàng từ response
                var jsonData = await responseGet.Content.ReadAsStringAsync();
                var existingCustomer = JsonSerializer.Deserialize<KhachHangViewModel>(jsonData, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (existingCustomer == null)
                {
                    ModelState.AddModelError("", "Khách hàng không tồn tại.");
                    return View(model);
                }

                // Cập nhật các trường từ ViewModel (trừ MaKH và NgayDangKy)
                existingCustomer.TenKH = model.TenKH;
                existingCustomer.DiaChi = model.DiaChi;
                existingCustomer.SDT = model.SDT;
                existingCustomer.Email = model.Email;
                existingCustomer.MatKhau = model.MatKhau;
                existingCustomer.TinhTrang = model.TinhTrang;

                // Serialize lại dữ liệu đã cập nhật
                var updatedData = JsonSerializer.Serialize(existingCustomer);
                var content = new StringContent(updatedData, Encoding.UTF8, "application/json");

                // Gửi yêu cầu PUT để cập nhật thông tin
                var responseUpdate = await _httpClient.PutAsync($"api/Customer/{existingCustomer.MaKH}", content);

                if (responseUpdate.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Cập nhật khách hàng thành công!";
                    return RedirectToAction("Index"); // Quay lại danh sách khách hàng nếu thành công
                }

                // Xử lý lỗi từ API
                var errorUpdateContent = await responseUpdate.Content.ReadAsStringAsync();
                ModelState.AddModelError("", $"Lỗi từ API: {errorUpdateContent}");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Đã xảy ra lỗi: {ex.Message}");
            }

            return View(model); // Trả về View cùng thông báo lỗi nếu có
        }


        // GET: Chi tiết khách hàng
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound("Mã khách hàng không hợp lệ.");
            }

            try
            {
                var response = await _httpClient.GetAsync($"api/Customer/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    var customer = JsonSerializer.Deserialize<KhachHangViewModel>(jsonData, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return View(customer);
                }

                ViewBag.ErrorMessage = "Không thể tải thông tin khách hàng.";
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Đã xảy ra lỗi: {ex.Message}";
            }

            return View("Error");
        }
    }
}
