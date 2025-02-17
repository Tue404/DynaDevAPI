using DynaDevAPI.Models;
using DynaDevFE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;

namespace DynaDevFE.Controllers
{
    public class StaffController : Controller
    {

        private readonly HttpClient _httpClient;

        public StaffController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7101/");
        }

        // GET: NhanVien
        public async Task<IActionResult> Index()
        {
            // URL của API để lấy danh sách nhân viên
            var apiUrl = "https://localhost:7101/api/Staff";
            var response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                // Deserialize trực tiếp danh sách NhanVien
                var staffList = JsonSerializer.Deserialize<List<NhanVienViewModel>>(jsonData, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                // Trả danh sách nhân viên trực tiếp cho View
                return View(staffList);
            }

            // Nếu có lỗi, trả về View rỗng
            return View("Index");
        }

        public async Task<IActionResult> Details(string id)
        {
            // Gọi API để lấy thông tin nhân viên từ backend
            var response = await _httpClient.GetAsync($"https://localhost:7101/api/Staff/{id}");

            if (!response.IsSuccessStatusCode)
            {
                // Nếu API trả về lỗi, hiển thị thông báo
                ViewBag.ErrorMessage = "Không tìm thấy nhân viên với mã " + id;
                return View();
            }

            var jsonData = await response.Content.ReadAsStringAsync();

            // Deserialize dữ liệu từ JSON sử dụng System.Text.Json
            var nhanVien = JsonSerializer.Deserialize<NhanVienViewModel>(jsonData);

            if (nhanVien == null)
            {
                // Nếu dữ liệu không hợp lệ hoặc không có nhân viên, thông báo lỗi
                ViewBag.ErrorMessage = "Không thể parse dữ liệu nhân viên.";
                return View();
            }

            // Trả lại view với thông tin nhân viên
            return View(nhanVien);
        }

        // GET: NhanVien/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(NhanVienViewModel model)
        {
            var random = new Random();
            int randomvalue = random.Next(0, 999);
            string ma = "NV" + randomvalue.ToString("D3");
            if (ModelState.IsValid)
            {
                // Tạo dữ liệu NhanVien để gửi qua API
                var nhanVien = new NhanVien
                {
                    MaNV = ma,
                    TenNV = model.TenNV,
                    Email = model.Email,
                    MatKhau = model.MatKhau,
                    SDT = model.SDT,
                    DiaChi = model.DiaChi,
                    TinhTrang = model.TinhTrang,
                    Luong = model.Luong,
                    NgayVaoLam = DateTime.Now,
                };

                // Chuyển dữ liệu sang JSON              
                var jsonContent = new StringContent(
                JsonSerializer.Serialize(nhanVien),
                Encoding.UTF8,
                "application/json"
                );


                // Gửi request POST đến API
                var response = await _httpClient.PostAsync("https://localhost:7101/api/Staff", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                  
                    TempData["Success"] = "Thêm nhân viên thành công!";
                    return RedirectToAction("Index");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Lỗi từ API: " + errorContent);
                    ModelState.AddModelError("", $"Lỗi từ API khi thêm nhân viên: {errorContent}");
                }
            }

            ModelState.AddModelError("", "Dữ liệu không hợp lệ.");
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var response = await _httpClient.GetAsync($"api/Staff/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound("Không tìm thấy sản phẩm.");
            }

            var productJson = await response.Content.ReadAsStringAsync();
            var product = JsonSerializer.Deserialize<NhanVienViewModel>(productJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(NhanVienViewModel model)
        {
            if (ModelState.IsValid)
            {
                var jsonContent = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"https://localhost:7101/api/Staff/{model.MaNV}", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Cập nhật thông tin nhân viên thành công!";
                    return RedirectToAction("Index");
                }
            }

            return View(model);
        }

    }
}
