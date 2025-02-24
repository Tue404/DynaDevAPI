using System.ComponentModel.DataAnnotations;

namespace DynaDevAPI.Models
{
    public class NhaCungCap
    {
        [Key]
        [Required(ErrorMessage = "Mã nhà cung cấp không được để trống.")]
        [StringLength(5, ErrorMessage = "Mã nhà cung cấp không được quá 10 ký tự.")]
        public string MaNCC { get; set; }

        [Required(ErrorMessage = "Tên nhà cung cấp không được để trống.")]
        [StringLength(100, ErrorMessage = "Tên nhà cung cấp không được quá 100 ký tự.")]
        public string TenNCC { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống.")]
        [RegularExpression(@"^\d{10,11}$", ErrorMessage = "Số điện thoại phải có 10-11 chữ số.")]
        public string SDT { get; set; }

        [Required(ErrorMessage = "Email không được để trống.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Địa chỉ không được để trống.")]
        public string DiaChi { get; set; }

        public string TinhTrang { get; set; }
    }
}
