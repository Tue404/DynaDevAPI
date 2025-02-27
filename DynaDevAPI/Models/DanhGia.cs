using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public string? TrangThai { get; set; }
        public DateTime NgayDanhGia { get; set; }


        [ValidateNever]
        [ForeignKey("MaSP")]
        public virtual SanPham? SanPham { get; set; }
        [ValidateNever]
        [ForeignKey("MaKH")]
        public virtual KhachHang? KhachHang { get; set; }
    }
}
