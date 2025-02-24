using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynaDevAPI.Models
{
    public class ChiTietDonHang
    {
        [Key]
        public string MaChiTiet { get; set; }

        [Required]
        [ForeignKey("DonHang")]
        public string MaDH { get; set; }
        public DonHang DonHang { get; set; }
        [Required]
        [ForeignKey("SanPham")]
        public string MaSP { get; set; }
        public SanPham SanPham { get; set; }
        public int SoLuong { get; set; }
        public decimal Gia { get; set; }

    }
}
