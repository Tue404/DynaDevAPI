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
        string maKH = Request.Cookies["MaKH"];

        if (string.IsNullOrEmpty(maKH))
        {
            return RedirectToAction("DangNhap", "TaiKhoan");
        }

        ViewBag.MaKH = maKH;

        var client = _clientFactory.CreateClient();
        if (client == null)
        {
            Console.WriteLine("HttpClient bị null!");
            return View("Error");
        }

        var response = await client.GetAsync($"https://localhost:7101/api/OrderHistory/{maKH}");

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
        var response = await client.GetAsync($"https://localhost:7081/api/OrderHistory/{id}");

        if (response.IsSuccessStatusCode)
        {
            var orderDetail = await response.Content.ReadAsAsync<OrderHistoryViewModel>();
            return View(orderDetail);
        }

        return View("Error");
    }
}
