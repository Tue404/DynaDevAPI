using DynaDevAPI.Data;
using DynaDevAPI.Helpers;
using DynaDevAPI.Models;
using DynaDevAPI.Services;
using DynaDevAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;


namespace DynaDevAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonHangController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IVnPayService _vnPayservice;
        private readonly IConfiguration _configuration;
        public DonHangController(ApplicationDbContext db, IConfiguration configuration, IVnPayService vnPayservice)
        {
            _db = db;
            _configuration = configuration;
            _vnPayservice = vnPayservice;
        }

        [HttpGet("{maDH}")]
        public async Task<IActionResult> GetDonHang(string maDH)
        {
            var donHang = await _db.DonHangs
                .Include(dh => dh.KhachHang) // ✅ JOIN với bảng KhachHang
                .FirstOrDefaultAsync(dh => dh.MaDH == maDH);

            if (donHang == null)
            {
                return NotFound(new { Message = "Không tìm thấy đơn hàng!" });
            }

            var response = new
            {
                MaDH = donHang.MaDH,
                MaKH = donHang.MaKH,
                TenNguoiNhan = donHang.TenNguoiNhan,
                Sdt = donHang.SoDienThoai,
                DiaChiNhanHang = donHang.DiaChiNhanHang,
                PhuongThucThanhToan = donHang.PhuongThucThanhToan,
                ThoiGianDatHang = donHang.ThoiGianDatHang,
                TongTien = donHang.TongTien
            };

            return Ok(response);
        }

        [HttpPost("DatHang")]
        public async Task<IActionResult> DatHang([FromBody] DatHangRequest request)
        {
            if (request == null || request.GioHang == null || !request.GioHang.Any())
            {
                return BadRequest(new { Message = "Giỏ hàng trống!" });
            }

            try
            {
                var donHang = new DonHang
                {
                    MaDH = Guid.NewGuid().ToString(),
                    MaKH = request.MaKH,
                    TenNguoiNhan = request.TenKH,
                    SoDienThoai = request.SoDienThoai,
                    DiaChiNhanHang = request.DiaChiNhanHang,
                    PhuongThucThanhToan = request.PhuongThucThanhToan,
                    ThoiGianDatHang = DateTime.Now,
                    TongTien = request.GioHang.Sum(x => (decimal)(x.Gia * x.SoLuong)),
                    OrderStatusId = 1,
                    PaymentStatusId = request.PhuongThucThanhToan == PaymentType.VNPAY ? 2 : 1
                };

                _db.DonHangs.Add(donHang);
                await _db.SaveChangesAsync();

                foreach (var sp in request.GioHang)
                {
                    var chiTiet = new ChiTietDonHang
                    {
                        MaChiTiet = Guid.NewGuid().ToString(),
                        MaDH = donHang.MaDH,
                        MaSP = sp.MaSP,
                        SoLuong = sp.SoLuong,
                        Gia = sp.Gia
                    };
                    _db.ChiTietDonHangs.Add(chiTiet);
                }

                await _db.SaveChangesAsync();

                return Ok(new { Success = true, Message = "Đặt hàng thành công!", MaDH = donHang.MaDH });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = "Đặt hàng thất bại: " + ex.Message });
            }
        }

        [HttpPost("create-payment-url")]
        public IActionResult CreatePaymentUrl([FromBody] VnPaymentRequestModel model)
        {
            try
            {
                var paymentUrl = _vnPayservice.CreatePaymentUrl(HttpContext, model);
                return Ok(new { PaymentUrl = paymentUrl });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Lỗi khi tạo URL thanh toán: " + ex.Message });
            }
        }

        [HttpGet("callback")]
        public IActionResult PaymentCallback()
        {
            var response = _vnPayservice.PaymentExecute(Request.Query);
            return Ok(response);
        }

        [HttpPost("payment-callback")]
        public async Task<IActionResult> PaymentCallback([FromBody] VnPaymentResponseModel responseModel)
        {
            if (!responseModel.Success || responseModel.VnPayResponseCode != "00")
            {
                return Ok(new { Success = false, Message = "Thanh toán thất bại", RedirectUrl = "/Checkout/OrderConfirmationFail" });
            }

            try
            {
                var donHang = await _db.DonHangs
                    .Include(dh => dh.ChiTietDonHangs)
                    .FirstOrDefaultAsync(dh => dh.MaDH == responseModel.OrderId);

                if (donHang == null)
                {
                    return BadRequest(new { Success = false, Message = "Không tìm thấy đơn hàng!" });
                }

                donHang.OrderStatusId = 2;
                donHang.PhuongThucThanhToan = "VnPay";
                donHang.ThoiGianDatHang = DateTime.Now;

                foreach (var chiTiet in donHang.ChiTietDonHangs)
                {
                    var sanPham = await _db.SanPhams.FirstOrDefaultAsync(sp => sp.MaSP == chiTiet.MaSP);
                    if (sanPham != null)
                    {
                        sanPham.SoLuongTrongKho -= chiTiet.SoLuong;
                    }
                }

                await _db.SaveChangesAsync();

                return Ok(new
                {
                    Success = true,
                    Message = "Thanh toán thành công",
                    RedirectUrl = "/Checkout/OrderConfirmation",
                    OrderId = donHang.MaDH,
                    TransactionId = responseModel.TransactionId
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = "Lỗi xử lý thanh toán: " + ex.Message });
            }
        }
    }
}
