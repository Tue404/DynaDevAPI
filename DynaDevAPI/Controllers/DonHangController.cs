﻿using DynaDevAPI.Data;
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
                decimal tongTien = request.GioHang.Sum(x => (decimal)(x.Gia * x.SoLuong));
                decimal giamGia = 0;

                // Xử lý voucher
                if (!string.IsNullOrEmpty(request.MaVoucher))
                {
                    var voucher = await _db.Vouchers.FirstOrDefaultAsync(v => v.MaVoucher == request.MaVoucher);
                    if (voucher == null)
                    {
                        return BadRequest(new { Success = false, Message = "Mã voucher không hợp lệ!" });
                    }

                    if (voucher.TrangThai != "Hoạt động")
                    {
                        return BadRequest(new { Success = false, Message = "Mã voucher không còn hoạt động!" });
                    }

                    if (DateTime.Now < voucher.NgayBatDau || DateTime.Now > voucher.NgayKetThuc)
                    {
                        return BadRequest(new { Success = false, Message = "Mã voucher đã hết thời gian sử dụng!" });
                    }

                    if (voucher.SoLuong <= 0)
                    {
                        return BadRequest(new { Success = false, Message = "Mã voucher đã hết số lượng!" });
                    }

                    string[] dieuKienParts = voucher.DieuKien.Split(':');
                    if (dieuKienParts.Length == 2 && dieuKienParts[0] == "DonHangToiThieu")
                    {
                        if (!decimal.TryParse(dieuKienParts[1], out decimal dieuKienToiThieu))
                        {
                            return BadRequest(new { Success = false, Message = "Điều kiện voucher không hợp lệ!" });
                        }

                        if (tongTien < dieuKienToiThieu)
                        {
                            return BadRequest(new { Success = false, Message = $"Đơn hàng phải tối thiểu {dieuKienToiThieu:N0} VNĐ để áp dụng voucher!" });
                        }
                    }

                    if (voucher.LoaiGiamGia == "Giảm giá theo phần trăm")
                    {
                        giamGia = tongTien * (voucher.GiamGia / 100);
                    }
                    else if (voucher.LoaiGiamGia == "SoTien")
                    {
                        giamGia = voucher.GiamGia;
                        if (giamGia > tongTien) giamGia = tongTien;
                    }
                    else
                    {
                        return BadRequest(new { Success = false, Message = "Loại giảm giá không hợp lệ!" });
                    }

                    voucher.SoLuong--; // Giảm số lượng voucher
                    _db.Vouchers.Update(voucher);
                }

                var maDH = Guid.NewGuid().ToString();

                if (request.PhuongThucThanhToan == PaymentType.VNPAY)
                {
                    var paymentRequest = new VnPaymentRequestModel
                    {
                        OrderId = maDH,
                        FullName = request.TenKH,
                        Description = $"Thanh toán đơn hàng {maDH}",
                        Amount = (double)(tongTien - giamGia),
                        CreatedDate = DateTime.Now
                    };

                    // Lưu tạm thông tin đơn hàng
                    var tempOrder = new TempOrder
                    {
                        MaDH = maDH,
                        RequestData = JsonSerializer.Serialize(request),
                        CreatedDate = DateTime.Now
                    };
                    _db.TempOrders.Add(tempOrder);
                    await _db.SaveChangesAsync();

                    // Tạo URL thanh toán
                    string paymentUrl = _vnPayservice.CreatePaymentUrl(HttpContext, paymentRequest);

                    return Ok(new
                    {
                        Success = true,
                        Message = "Chuẩn bị thanh toán VNPay",
                        MaDH = maDH,
                        TongTienSauGiam = tongTien - giamGia,
                        GiamGia = giamGia,
                        PaymentUrl = paymentUrl
                    });
                }

                // Trường hợp không dùng VNPay, lưu ngay
                var donHang = new DonHang
                {
                    MaDH = maDH,
                    MaKH = request.MaKH,
                    MaVoucher = string.IsNullOrEmpty(request.MaVoucher) ? null : request.MaVoucher,
                    TenNguoiNhan = request.TenKH,
                    SoDienThoai = request.SoDienThoai,
                    DiaChiNhanHang = request.DiaChiNhanHang,
                    PhuongThucThanhToan = request.PhuongThucThanhToan,
                    ThoiGianDatHang = DateTime.Now,
                    TongTien = tongTien - giamGia,
                    OrderStatusId = 1,
                    PaymentStatusId = 1
                };

                _db.DonHangs.Add(donHang);
                foreach (var sp in request.GioHang)
                {
                    var product = await _db.SanPhams.FirstOrDefaultAsync(p => p.MaSP == sp.MaSP);
                    if (product != null)
                    {
                        if (product.SoLuongTrongKho < sp.SoLuong)
                        {
                            return BadRequest(new { Success = false, Message = $"Số lượng sản phẩm '{product.TenSanPham}' trong kho không đủ!" });
                        }
                        product.SoLuongTrongKho -= sp.SoLuong;
                        product.DaBan += sp.SoLuong;
                        _db.SanPhams.Update(product);
                    }
                    else
                    {
                        return BadRequest(new { Success = false, Message = $"Không tìm thấy sản phẩm với mã '{sp.MaSP}'!" });
                    }
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

                return Ok(new
                {
                    Success = true,
                    Message = "Đặt hàng thành công!",
                    MaDH = donHang.MaDH,
                    TongTienSauGiam = donHang.TongTien,
                    GiamGia = giamGia
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = "Đặt hàng thất bại: " + ex.Message });
            }
        }

        [HttpGet("CheckVoucher")]
        public IActionResult CheckVoucher(string maVoucher, decimal tongTien)
        {
            if (string.IsNullOrEmpty(maVoucher))
            {
                return BadRequest(new { Success = false, Message = "Vui lòng nhập mã voucher!" });
            }

            var voucher = _db.Vouchers.FirstOrDefault(v => v.MaVoucher == maVoucher);
            if (voucher == null)
            {
                return BadRequest(new { Success = false, Message = "Mã voucher không hợp lệ!" });
            }

            if (voucher.TrangThai != "Hoạt động")
            {
                return BadRequest(new { Success = false, Message = "Mã voucher không còn hoạt động!" });
            }

            if (DateTime.Now < voucher.NgayBatDau || DateTime.Now > voucher.NgayKetThuc)
            {
                return BadRequest(new { Success = false, Message = "Mã voucher đã hết thời gian sử dụng!" });
            }

            if (voucher.SoLuong <= 0)
            {
                return BadRequest(new { Success = false, Message = "Mã voucher đã hết số lượng!" });
            }

            // Parse điều kiện từ DieuKien
            string[] dieuKienParts = voucher.DieuKien.Split(':');
            if (dieuKienParts.Length == 2 && dieuKienParts[0] == "DonHangToiThieu")
            {
                if (!decimal.TryParse(dieuKienParts[1], out decimal dieuKienToiThieu))
                {
                    return BadRequest(new { Success = false, Message = "Điều kiện voucher không hợp lệ!" });
                }

                if (tongTien < dieuKienToiThieu)
                {
                    return BadRequest(new { Success = false, Message = $"Đơn hàng phải tối thiểu {dieuKienToiThieu:N0} VNĐ để áp dụng voucher!" });
                }
            }

            decimal giamGia = 0;
            if (voucher.LoaiGiamGia == "Giảm giá theo phần trăm")
            {
                giamGia = tongTien * (voucher.GiamGia / 100);
            }
            else if (voucher.LoaiGiamGia == "SoTien")
            {
                giamGia = voucher.GiamGia;
                if (giamGia > tongTien)
                {
                    giamGia = tongTien;
                }
            }
            else
            {
                return BadRequest(new { Success = false, Message = "Loại giảm giá không hợp lệ!" });
            }

            decimal tongTienSauGiam = tongTien - giamGia;

            return Ok(new
            {
                Success = true,
                TongTienSauGiam = tongTienSauGiam,
                GiamGia = giamGia
            });
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
        public async Task<IActionResult> PaymentCallback()
        {
            try
            {
                // Gọi IVnPayService để xử lý phản hồi từ VNPay
                var response = _vnPayservice.PaymentExecute(Request.Query);

                if (!response.Success)
                {
                    return BadRequest(new { Success = false, Message = "Chữ ký không hợp lệ hoặc dữ liệu không đúng!" });
                }

                if (response.VnPayResponseCode != "00")
                {
                    var tempOrder = await _db.TempOrders.FirstOrDefaultAsync(t => t.MaDH == response.OrderId);
                    if (tempOrder != null)
                    {
                        _db.TempOrders.Remove(tempOrder);
                        await _db.SaveChangesAsync();
                    }
                    return BadRequest(new { Success = false, Message = $"Thanh toán thất bại! Mã lỗi: {response.VnPayResponseCode}" });
                }

                // Thanh toán thành công, lấy thông tin tạm
                var tempOrderSuccess = await _db.TempOrders.FirstOrDefaultAsync(t => t.MaDH == response.OrderId);
                if (tempOrderSuccess == null)
                {
                    return BadRequest(new { Success = false, Message = "Không tìm thấy thông tin đơn hàng tạm!" });
                }

                // Deserialize thông tin đơn hàng
                var request = JsonSerializer.Deserialize<DatHangRequest>(tempOrderSuccess.RequestData);
                if (request == null)
                {
                    return StatusCode(500, new { Success = false, Message = "Không thể deserialize dữ liệu đơn hàng tạm!" });
                }

                // Tính toán lại tổng tiền và giảm giá
                decimal tongTien = request.GioHang.Sum(x => (decimal)(x.Gia * x.SoLuong));
                decimal giamGia = 0;
                if (!string.IsNullOrEmpty(request.MaVoucher))
                {
                    var voucher = await _db.Vouchers.FirstOrDefaultAsync(v => v.MaVoucher == request.MaVoucher);
                    if (voucher != null && voucher.SoLuong > 0)
                    {
                        if (voucher.LoaiGiamGia == "Giảm giá theo phần trăm")
                        {
                            giamGia = tongTien * (voucher.GiamGia / 100);
                        }
                        else if (voucher.LoaiGiamGia == "SoTien")
                        {
                            giamGia = voucher.GiamGia;
                            if (giamGia > tongTien) giamGia = tongTien;
                        }
                        voucher.SoLuong--;
                        _db.Vouchers.Update(voucher);
                    }
                }

                // Lưu đơn hàng chính thức
                var donHang = new DonHang
                {
                    MaDH = tempOrderSuccess.MaDH,
                    MaKH = request.MaKH,
                    MaVoucher = string.IsNullOrEmpty(request.MaVoucher) ? null : request.MaVoucher,
                    TenNguoiNhan = request.TenKH,
                    SoDienThoai = request.SoDienThoai,
                    DiaChiNhanHang = request.DiaChiNhanHang,
                    PhuongThucThanhToan = request.PhuongThucThanhToan,
                    ThoiGianDatHang = DateTime.Now,
                    TongTien = tongTien - giamGia,
                    OrderStatusId = 1,
                    PaymentStatusId = 2
                };

                _db.DonHangs.Add(donHang);
                foreach (var sp in request.GioHang)
                {
                    var product = await _db.SanPhams.FirstOrDefaultAsync(p => p.MaSP == sp.MaSP);
                    if (product != null)
                    {
                        if (product.SoLuongTrongKho < sp.SoLuong)
                        {
                            return BadRequest(new { Success = false, Message = $"Số lượng sản phẩm '{product.TenSanPham}' trong kho không đủ!" });
                        }
                        product.SoLuongTrongKho -= sp.SoLuong;
                        product.DaBan += sp.SoLuong;
                        _db.SanPhams.Update(product);
                    }
                    else
                    {
                        return BadRequest(new { Success = false, Message = $"Không tìm thấy sản phẩm với mã '{sp.MaSP}'!" });
                    }
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

                // Xóa bản ghi tạm
                _db.TempOrders.Remove(tempOrderSuccess);
                await _db.SaveChangesAsync();

                return Ok(new { Success = true, RedirectUrl = "/Checkout/Success" });
            }
            catch (Exception ex)
            {
                // Log lỗi để kiểm tra
                // Ví dụ: ILogger hoặc Console.WriteLine nếu chưa có logging
                Console.WriteLine($"Lỗi trong PaymentCallback: {ex.Message}");
                return StatusCode(500, new { Success = false, Message = "Lỗi xử lý callback: " + ex.Message });
            }
        }

        [HttpPost("payment-callback")]
        public async Task<IActionResult> PaymentCallback([FromBody] VnPaymentResponseModel responseModel)
        {
            if (!responseModel.Success || responseModel.VnPayResponseCode != "00")
            {
                return Ok(new { Success = false, Message = "Thanh toán thất bại", RedirectUrl = "/Checkout/Fail" });
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
