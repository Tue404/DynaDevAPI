using DynaDevAPI.Models;

namespace DynaDevFE.Models
{
    public class CheckoutRequest
    {
        public string MaKH { get; set; }
        public string TenKH { get; set; }
        public string SoDienThoai { get; set; }
        public string DiaChiNhanHang { get; set; }
        public string PaymentMethod { get; set; }
        public List<CartViewModel> CartItems { get; set; }
    }
}
