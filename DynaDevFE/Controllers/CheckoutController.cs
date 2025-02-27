using DynaDevAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using System.Net.Http.Json;
using DynaDevFE.Models;
using System.Net.Http;
using DynaDevAPI.Helpers;
using DynaDevAPI.ViewModels;

namespace DynaDevFE.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiBaseUrl = "https://localhost:7101";

        public CheckoutController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult ThongTinThanhToan()
            {
            string maKH = Request.Cookies["MaKH"];

            if (string.IsNullOrEmpty(maKH))
            {
                TempData["ShowLoginModal"] = true;
                TempData["LoginMessage"] = "Bạn cần đăng nhập để tiếp tục thanh toán!";
                return RedirectToAction("Index", "Cart");
            }

            ViewBag.MaKH = maKH;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DatHang([FromBody] CheckoutRequest request)
        {
            if (request.CartItems == null || !request.CartItems.Any())
            {
                return Json(new { success = false, message = "Giỏ hàng trống!" });
            }

            var client = _httpClientFactory.CreateClient();

            // Gửi yêu cầu đặt hàng qua API
            var datHangRequest = new DatHangRequest
            {
                MaKH = request.MaKH,
                TenKH = request.TenKH,
                SoDienThoai = request.SoDienThoai,
                DiaChiNhanHang = request.DiaChiNhanHang,
                PhuongThucThanhToan = request.PaymentMethod,
                MaVoucher = request.MaVoucher,
                GioHang = request.CartItems.Select(item => new ChiTietDonHangRequest
                {
                    MaSP = item.MaSP,
                    SoLuong = item.SoLuong,
                    Gia = (int)item.Gia
                }).ToList()
            };

            var response = await client.PostAsJsonAsync($"{_apiBaseUrl}/api/DonHang/DatHang", datHangRequest);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return Json(new { success = false, message = $"Lỗi đặt hàng: {error}" });
            }

            var result = await response.Content.ReadFromJsonAsync<ApiResponse>();
            if (!result.Success || string.IsNullOrEmpty(result.MaDH))
            {
                return Json(new { success = false, message = result.Message ?? "Đặt hàng thất bại!" });
            }

            if (request.PaymentMethod == PaymentType.VNPAY)
            {
                var paymentRequest = new VnPaymentRequestModel
                {
                    OrderId = result.MaDH,
                    FullName = request.TenKH,
                    Description = $"Thanh toán đơn hàng {result.MaDH}",
                    Amount = (double)result.TongTienSauGiam,
                    CreatedDate = DateTime.Now
                };

                var paymentResponse = await client.PostAsJsonAsync($"{_apiBaseUrl}/api/DonHang/create-payment-url", paymentRequest);
                if (paymentResponse.IsSuccessStatusCode)
                {
                    var paymentResult = await paymentResponse.Content.ReadFromJsonAsync<PaymentResponse1>();
                    return Json(new { success = true, redirectUrl = paymentResult.PaymentUrl });
                }

                var paymentError = await paymentResponse.Content.ReadAsStringAsync();
                return Json(new { success = false, message = $"Lỗi tạo URL thanh toán: {paymentError}" });
            }

            return Json(new { success = true, redirectUrl = "/Checkout/Success" });
        }

        [HttpGet]
        public async Task<IActionResult> CheckVoucher(string maVoucher, decimal tongTien)
        {
            if (string.IsNullOrEmpty(maVoucher))
            {
                return Json(new { success = false, message = "Vui lòng nhập mã voucher!" });
            }

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"{_apiBaseUrl}/api/DonHang/CheckVoucher?maVoucher={maVoucher}&tongTien={tongTien}");
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return Json(new { success = false, message = $"Lỗi kiểm tra voucher: {error}" });
            }

            var result = await response.Content.ReadFromJsonAsync<CheckVoucherResponse>();
            return Json(new { success = true, tongTienSauGiam = result.TongTienSauGiam, giamGia = result.GiamGia });
        }

        [HttpGet]
        public async Task<IActionResult> PaymentCallBack()
        {
            var client = _httpClientFactory.CreateClient();
            var queryString = Request.QueryString.ToString();

            // Gọi API callback từ backend
            var callbackResponse = await client.GetAsync($"{_apiBaseUrl}/api/DonHang/callback{queryString}");
            if (!callbackResponse.IsSuccessStatusCode)
            {
                var errorContent = await callbackResponse.Content.ReadAsStringAsync();
                // Log lỗi để kiểm tra
                Console.WriteLine($"Lỗi từ backend: {errorContent}");
                return RedirectToAction("Fail");
            }

            var result = await callbackResponse.Content.ReadFromJsonAsync<PaymentCallbackResponse>();
            if (result == null || !result.Success)
            {
                return RedirectToAction("OrderConfirmationFail");
            }
            return Redirect(result.RedirectUrl ?? "/Checkout/Success");
        }

        public IActionResult Success()
        {
            return View();
        }

        public IActionResult Fail()
        {
            return View();
        }
    }
}
