namespace DynaDevAPI.Models
{
    public class CartItemDto
    {
        public string MaSP { get; set; }
        public string TenSanPham { get; set; }
        public string? Anh { get; set; }
        public decimal Gia { get; set; }
        public int SoLuong { get; set; }
    }
}
