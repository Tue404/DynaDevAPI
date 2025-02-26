using DynaDevFE.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using DynaDevAPI.Models;
using System.Text.Json;

namespace DynaDevFE.Controllers
{
    public class VoucherController : Controller
    {
        private readonly HttpClient _httpClient;

        public VoucherController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // GET: api/Voucher (with pagination)
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 8)
        {
            try
            {
                var response = await _httpClient.GetAsync($"https://localhost:7101/api/Voucher?pageNumber={pageNumber}&pageSize={pageSize}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<dynamic>(jsonData, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    var vouchers = result["Vouchers"].ToObject<List<VoucherViewModel>>();
                    var totalVouchers = result["TotalVouchers"].ToObject<int>();
                    var totalPages = result["TotalPages"].ToObject<int>();

                    ViewBag.TotalVouchers = totalVouchers;
                    ViewBag.TotalPages = totalPages;
                    ViewBag.CurrentPage = pageNumber;
                    ViewBag.PageSize = pageSize;

                    return View(vouchers);
                }

                ViewBag.Message = "Không có voucher nào";
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Lỗi khi tải dữ liệu: " + ex.Message;
                return View();
            }
        }

        public async Task<JsonResult> Search(string query, int pageNumber = 1, int pageSize = 8)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return Json(new { vouchers = new List<VoucherViewModel>(), totalPages = 0 });
            }

            try
            {
                var response = await _httpClient.GetAsync($"https://localhost:7101/api/Voucher/Search?query={Uri.EscapeDataString(query)}&pageNumber={pageNumber}&pageSize={pageSize}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    var data = JsonSerializer.Deserialize<dynamic>(jsonData, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    var vouchers = data.vouchers.ToObject<List<VoucherViewModel>>();
                    var totalPages = data.totalPages;

                    return Json(new { vouchers = vouchers, totalPages = totalPages });
                }

                return Json(new { vouchers = new List<VoucherViewModel>(), totalPages = 0 });
            }
            catch (Exception)
            {
                return Json(new { vouchers = new List<VoucherViewModel>(), totalPages = 0 });
            }
        }


        // GET: Create voucher
        public IActionResult Create()
        {
            return View();
        }

        // POST: Create voucher
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VoucherViewModel voucher)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var response = await _httpClient.PostAsJsonAsync("https://localhost:7101/api/Voucher", voucher);

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Success"] = "Thêm voucher thành công!";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["Error"] = "Lỗi khi thêm voucher";
                    }
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Lỗi khi thêm voucher: " + ex.Message;
                }
            }

            return View(voucher);
        }

        // GET: Edit voucher
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var voucher = await _httpClient.GetFromJsonAsync<VoucherViewModel>($"https://localhost:7101/api/Voucher/{id}");

            if (voucher == null)
            {
                return NotFound();
            }

            return View(voucher);
        }

        // POST: Edit voucher
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, VoucherViewModel voucher)
        {
            if (id != voucher.MaVoucher)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var response = await _httpClient.PutAsJsonAsync($"https://localhost:7101/api/Voucher/{id}", voucher);

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Success"] = "Cập nhật voucher thành công!";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["Error"] = "Lỗi khi cập nhật voucher";
                    }
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Lỗi khi cập nhật voucher: " + ex.Message;
                }
            }

            return View(voucher);
        }

        // GET: Delete voucher
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"https://localhost:7101/api/Voucher/{id}");

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Xóa voucher thành công!";
                }
                else
                {
                    TempData["Error"] = "Lỗi khi xóa voucher";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi khi xóa voucher: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        // GET: View voucher details
        public async Task<IActionResult> Details(string id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"https://localhost:7101/api/Voucher/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    var voucher = JsonSerializer.Deserialize<VoucherViewModel>(jsonData, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return View(voucher);
                }

                ViewBag.ErrorMessage = "Không thể tải thông tin Voucher.";
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Đã xảy ra lỗi: {ex.Message}";
            }

            return View("Error");
        }
    }
}
