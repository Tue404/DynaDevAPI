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

      
        public async Task<IActionResult> Index()
        {
            try
            {
                var vouchers = await _httpClient.GetFromJsonAsync<List<Models.VoucherViewModel>>("https://localhost:7101/api/Voucher");

                if (vouchers == null)
                {
                    ViewBag.Message = "Không có voucher nào";
                }

                return View(vouchers);
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Lỗi khi tải dữ liệu: " + ex.Message;
                return View();
            }
        }

        public async Task<JsonResult> Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return Json(new List<VoucherViewModel>());
            }

            try
            {
                var response = await _httpClient.GetAsync($"https://localhost:7101/api/Voucher/Search?query={Uri.EscapeDataString(query)}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    var vouchers = JsonSerializer.Deserialize<List<VoucherViewModel>>(jsonData, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return Json(vouchers);
                }

                return Json(new List<VoucherViewModel>()); // Trả về danh sách rỗng nếu không tìm thấy
            }
            catch (Exception)
            {
                return Json(new List<VoucherViewModel>()); // Xử lý lỗi bằng cách trả về danh sách rỗng
            }
        }


        public IActionResult Create()
        {
            return View();
        }

    
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

       
        public async Task<IActionResult> Edit(string id)
        {
            var voucher = await _httpClient.GetFromJsonAsync<Models.VoucherViewModel>($"https://localhost:7101/api/Voucher/{id}");

            if (voucher == null)
            {
                return NotFound();
            }

            return View(voucher);
        }

       
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
    }
}
