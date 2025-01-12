using System.ComponentModel.DataAnnotations;

namespace DynaDevAPI.Models
{
    public class NhanVien
    {
        [Key]
        public string MaNV { get; set; }
        public string TenNV { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string MatKhau { get; set; }
        public int SDT { get; set; }
        public string DiaChi { get; set; }
        public string TinhTrang { get; set; }
        public DateTime NgayVaoLam { get; set; }

        public ICollection<DonHang> DonHangs { get; set; }
    }
}
