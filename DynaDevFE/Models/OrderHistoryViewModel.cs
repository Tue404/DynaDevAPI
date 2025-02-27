namespace DynaDevFE.Models
{
    public class OrderHistoryViewModel
    {
        public string MaDH { get; set; }
        public DateTime ThoiGianDatHang { get; set; }
        public decimal TongTien { get; set; }
        public string OrderStatus { get; set; }
        public string PaymentStatus { get; set; }

        public string ShippingAddress { get; set; }
/*        public DateTime DateChanged { get; set; }
        public string ChangedBy { get; set; }*/
        public List<ProductHistory> Products { get; set; }
    }

    public class ProductHistory
    {
        public string TenSanPham { get; set; }
        public int SoLuong { get; set; }
        public decimal Gia { get; set; }
        public decimal Total { get; set; }
    }

}
