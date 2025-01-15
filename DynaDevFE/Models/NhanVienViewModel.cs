using DynaDevAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DynaDevFE.Models
{
    public class NhanVienViewModel
    {
        [JsonPropertyName("maNV")]
        public string MaNV { get; set; }
        [JsonPropertyName("tenNV")]
        public string TenNV { get; set; }
        [EmailAddress]
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("matKhau")]
        public string MatKhau { get; set; }
        [JsonPropertyName("sdt")]
        public string SDT { get; set; }
        [JsonPropertyName("diaChi")]
        public string DiaChi { get; set; }
        [JsonPropertyName("tinhTrang")]
        public string TinhTrang { get; set; }
        [JsonPropertyName("ngayVaoLam")]
        public DateTime NgayVaoLam { get; set; }   
    }
}
