using System.ComponentModel.DataAnnotations;

namespace DynaDevAPI.Models
{
    public class KhachHang
    {
        [Key]
        public string MaKH { get; set; }
        public string TenKH { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string MatKhau { get; set; }
        public int SDT { get; set; }
        public string DiaChi { get; set; }
        public string TinhTrang { get; set; }
        public DateTime NgayDangKy { get; set; }

        public ICollection<DonHang> DonHangs { get; set; }
        public ICollection<DanhGia> DanhGias { get; set; }
    }
}
