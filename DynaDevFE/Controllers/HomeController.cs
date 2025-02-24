using DynaDevAPI.Models;
using DynaDevFE.Models;
using DynaDevFE.Models.DynaDevFE.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;

namespace DynaDevFE.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient;

        public HomeController(ILogger<HomeController> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
        {

            var apiProductUrl = "https://localhost:7101/api/Products";
            var apiVoucherUrl = "https://localhost:7101/api/voucher?pageNumber=1&pageSize=8";

            var products = new List<SanPhamViewModel>();
            var vouchers = new List<VoucherViewModel>();

            try
            {

                // URL của API để lấy danh sách sản phẩm
                var apiUrl = "https://localhost:7101/api/Products";
                var response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    // Lấy sản phẩm
                    var productResponse = await _httpClient.GetAsync(apiProductUrl);
                    if (productResponse.IsSuccessStatusCode)
                    {
                        var productJson = await productResponse.Content.ReadAsStringAsync();
                        products = JsonSerializer.Deserialize<List<SanPhamViewModel>>(productJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                                   ?? new List<SanPhamViewModel>();

                        // Lấy hình ảnh cho từng sản phẩm
                        foreach (var product in products)
                        {
                            var imgResponse = await _httpClient.GetAsync($"https://localhost:7101/api/Products/GetImagesByProduct/{product.MaSP}");
                            if (imgResponse.IsSuccessStatusCode)
                            {
                                var imgJson = await imgResponse.Content.ReadAsStringAsync();
                                var images = JsonSerializer.Deserialize<List<AnhSP>>(imgJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                                             ?? new List<AnhSP>();
                                product.DanhSachAnh = images.Select(img => img.TenAnh).ToList();
                            }
                        }
                    }

                    // Lấy voucher
                    var voucherResponse = await _httpClient.GetAsync(apiVoucherUrl);
                    if (voucherResponse.IsSuccessStatusCode)
                    {
                        var voucherJson = await voucherResponse.Content.ReadAsStringAsync();
                        var voucherResult = JsonSerializer.Deserialize<VoucherResultViewModel>(voucherJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                        // Tránh lỗi CS0019 bằng cách đảm bảo kiểu dữ liệu phù hợp
                        vouchers = voucherResult?.Vouchers ?? new List<VoucherViewModel>();
                    }

                    // Truyền dữ liệu sang View
                    ViewBag.Vouchers = vouchers;
                    return View(products);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tải dữ liệu");
                return View(new List<SanPhamViewModel>());
            }
            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
