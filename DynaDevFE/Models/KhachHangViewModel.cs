using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace DynaDevFE.Models
{
    public class KhachHangViewModel
    {
        [JsonPropertyName("maKH")]
        public string MaKH { get; set; }

        [JsonPropertyName("tenKH")]
        [Required(ErrorMessage = "Tên khách hàng không được để trống.")]
        [StringLength(100, ErrorMessage = "Tên khách hàng không được vượt quá 100 ký tự.")]
        public string TenKH { get; set; }


        [JsonPropertyName("email")]
        [Required(ErrorMessage = "Email không được để trống.")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng.")]
        public string Email { get; set; }

        [JsonPropertyName("matKhau")]
        [Required(ErrorMessage = "Mật khẩu không được để trống.")]
        [StringLength(100, ErrorMessage = "Mật khẩu không được vượt quá 50 ký tự.")]
        public string? MatKhau { get; set; }

        [JsonPropertyName("sdt")]
        [Required(ErrorMessage = "Số điện thoại không được để trống.")]
        public string? SDT { get; set; }


        [JsonPropertyName("diaChi")]
        [Required(ErrorMessage = "Địa chỉ không được để trống.")]
        public string DiaChi { get; set; }

        [JsonPropertyName("tinhTrang")]
        [Required(ErrorMessage = "Tình trạng không được để trống.")]
        public string TinhTrang { get; set; }

        public DateTime? NgayDangKy { get; set; }
    }
}
