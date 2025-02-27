using DynaDevFE.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

public class OrderHistoryController : Controller
{
    private readonly IHttpClientFactory _clientFactory;

    public OrderHistoryController(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    // GET: /OrderHistory/Index
    public async Task<IActionResult> Index()
    {
        // Lấy mã khách hàng từ Session hoặc từ cơ chế đăng nhập của bạn
        var customerId = HttpContext.Session.GetString("MaKH");

        if (string.IsNullOrEmpty(customerId))
        {
            // Nếu không có mã khách hàng, chuyển hướng về trang đăng nhập
            return RedirectToAction("Login", "Account");
        }

        var client = _clientFactory.CreateClient();
        var response = await client.GetAsync($"https://localhost:7081/api/OrderHistory/OrderHistory/{customerId}");

        if (response.IsSuccessStatusCode)
        {
            var orders = await response.Content.ReadAsAsync<IEnumerable<OrderHistoryViewModel>>();

            // Kiểm tra dữ liệu trước khi gửi đến view
            if (orders == null || !orders.Any())
            {
                // Nếu không có đơn hàng, trả về view Error
                return View("Error");
            }

            return View(orders); // Truyền dữ liệu vào view
        }

        return View("Error"); // Xử lý lỗi nếu không có đơn hàng
    }

    // Xem chi tiết đơn hàng
    public async Task<IActionResult> Details(string id)
    {
        var client = _clientFactory.CreateClient();
        var response = await client.GetAsync($"https://localhost:7081/api/OrderHistory/OrderHistory/{id}");

        if (response.IsSuccessStatusCode)
        {
            var orderDetail = await response.Content.ReadAsAsync<OrderHistoryViewModel>();
            return View(orderDetail);
        }

        return View("Error");
    }
}
