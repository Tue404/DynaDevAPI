using System.ComponentModel.DataAnnotations;

namespace DynaDevAPI.Models
{
    public class DanhGia
    {
        [Key]
        public string MaDanhGia { get; set; }
        public string MaSP { get; set; }
        public string MaKH { get; set; }
        public int DiemDanhGia { get; set; }
        public string BinhLuan { get; set; }
        public DateTime NgayDanhGia { get; set; }

        public SanPham SanPham { get; set; }
        public KhachHang KhachHang { get; set; }
    }
}
