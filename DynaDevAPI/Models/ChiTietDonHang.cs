using System.ComponentModel.DataAnnotations;

namespace DynaDevAPI.Models
{
    public class ChiTietDonHang
    {
        [Key]
        public string MaChiTiet { get; set; }
        public string MaDH { get; set; }
        public string MaSP { get; set; }
        public int SoLuong { get; set; }
        public decimal Gia { get; set; }

        public DonHang DonHang { get; set; }
        public SanPham SanPham { get; set; }
    }
}
