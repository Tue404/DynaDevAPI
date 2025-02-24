namespace DynaDevFE.Models
{
    public class DatHangViewModel
    {
        public string MaKH { get; set; } // Mã khách hàng
        public string TenNguoiNhan { get; set; } // Tên người nhận
        public string SoDienThoai { get; set; } // Số điện thoại của khách hàng
        public string DiaChiNhanHang { get; set; } // Địa chỉ nhận hàng
        public string PhuongThucThanhToan { get; set; } // Phương thức thanh toán
        public List<CartViewModel> GioHang { get; set; } = new List<CartViewModel>(); // Giỏ hàng của khách hàng
    }
}
