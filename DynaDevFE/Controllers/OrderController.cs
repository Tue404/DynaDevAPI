using DynaDevFE.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace DynaDevFE.Controllers
{
    public class OrderController : Controller
    {
        private readonly HttpClient _httpClient;

        public OrderController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7101/"); // Base URL của DynaDevAPI
        }

        // GET: Orders (Danh sách đơn hàng)
        public async Task<IActionResult> Index()
        {
            try
            {
                var apiUrl = "api/Order";
                var response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    var orderList = JsonSerializer.Deserialize<List<DonHangViewModel>>(jsonData, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return View(orderList ?? new List<DonHangViewModel>());
                }

                TempData["ErrorMessage"] = "Không thể tải danh sách đơn hàng từ API.";
                return View(new List<DonHangViewModel>());
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Đã xảy ra lỗi: {ex.Message}";
                return View(new List<DonHangViewModel>());
            }
        }



        // GET: Orders/Details/{id} (Chi tiết đơn hàng)
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                TempData["ErrorMessage"] = "Mã đơn hàng không hợp lệ.";
                return RedirectToAction("Index");
            }

            try
            {
                var response = await _httpClient.GetAsync($"api/Order/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    var order = JsonSerializer.Deserialize<DonHangViewModel>(jsonData, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return View(order);
                }

                TempData["ErrorMessage"] = "Không tìm thấy đơn hàng.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Đã xảy ra lỗi: {ex.Message}";
                return RedirectToAction("Index");
            }
        }


        [HttpPost]
        public async Task<IActionResult> ChangeOrderStatus(string id, int newOrderStatusId)
        {
            if (string.IsNullOrEmpty(id))
            {
                TempData["ErrorMessage"] = "Mã đơn hàng không hợp lệ.";
                return RedirectToAction("Index");
            }

            try
            {
                var content = new StringContent(JsonSerializer.Serialize(newOrderStatusId), Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"api/Order/ChangeOrderStatus/{id}", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Cập nhật trạng thái đơn hàng thành công!";

                    // Sau khi thay đổi, bạn có thể gọi lại API để tải lại dữ liệu
                    var updatedOrder = await _httpClient.GetAsync($"api/Order/{id}"); // Gọi API để lấy lại thông tin đơn hàng
                    var jsonData = await updatedOrder.Content.ReadAsStringAsync();
                    var order = JsonSerializer.Deserialize<DonHangViewModel>(jsonData, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return View("Details", order);  // Bạn có thể điều hướng trực tiếp tới trang chi tiết
                }

                var errorResponse = await response.Content.ReadAsStringAsync();
                TempData["ErrorMessage"] = $"Lỗi: {errorResponse}";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Đã xảy ra lỗi: {ex.Message}";
                return RedirectToAction("Index");
            }
        }




        [HttpPost]
        public async Task<IActionResult> ChangePaymentStatus(string id, int newPaymentStatusId)
        {
            if (string.IsNullOrEmpty(id))
            {
                TempData["ErrorMessage"] = "Mã đơn hàng không hợp lệ.";
                return RedirectToAction("Index");
            }

            try
            {
                var content = new StringContent(JsonSerializer.Serialize(newPaymentStatusId), Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"api/Order/ChangePaymentStatus/{id}", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Cập nhật trạng thái thanh toán thành công!";
                    return RedirectToAction("Index");
                }

                TempData["ErrorMessage"] = "Không thể cập nhật trạng thái thanh toán.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Đã xảy ra lỗi: {ex.Message}";
                return RedirectToAction("Index");
            }
        }


        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                TempData["ErrorMessage"] = "Mã đơn hàng không hợp lệ.";
                return RedirectToAction("Index");
            }

            try
            {
                var response = await _httpClient.DeleteAsync($"api/Order/{id}");

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Xóa đơn hàng thành công!";
                    return RedirectToAction("Index");
                }

                TempData["ErrorMessage"] = "Không thể xóa đơn hàng.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Đã xảy ra lỗi: {ex.Message}";
                return RedirectToAction("Index");
            }
        }




    }
}
