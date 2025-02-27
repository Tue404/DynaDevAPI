using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DynaDevFE.Models
{
    public class SanPhamViewModel
    {
        [JsonPropertyName("maSP")]
        public string MaSP { get; set; }

        [JsonPropertyName("maLoai")]
        [Required(ErrorMessage = "Mã loại không được để trống.")]
        [StringLength(50, ErrorMessage = "Mã loại sản phẩm không được vượt quá 50 ký tự.")]
        public string MaLoai { get; set; }
        [JsonPropertyName("tenNCC")]
        public string? TenNCC { get; set; }
        [JsonPropertyName("maNCC")]
        public string MaNCC { get; set; }

        [JsonPropertyName("tenSanPham")]

        [Required(ErrorMessage = "Tên sản phẩm không được để trống.")]
        [StringLength(100, ErrorMessage = "Tên sản phẩm không được vượt quá 100 ký tự.")]
        public string TenSanPham { get; set; }
        [JsonPropertyName("tacGia")]
        public string TacGia { get; set; }
        [Required(ErrorMessage = "Nhà xuất bản không được để trống.")]
        //[JsonPropertyName("nhaXuatBan")]
        //public string NhaXuatBan { get; set; }
        [JsonPropertyName("namXuatBan")]
        public int NamXuatBan { set; get; }

        [JsonPropertyName("gia")]
        [Required(ErrorMessage = "Giá không được để trống.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Giá sản phẩm phải lớn hơn 0.")]
        public decimal Gia { get; set; }

        [JsonPropertyName("moTa")]
        [StringLength(1000, ErrorMessage = "Mô tả không được vượt quá 500 ký tự.")]
        public string MoTa { get; set; }

        [JsonPropertyName("soLuongTrongKho")]
        [Required(ErrorMessage = "Số lượng trong kho không được để trống.")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng trong kho phải là số nguyên dương.")]
        public int SoLuongTrongKho { get; set; }

        [JsonPropertyName("ngayThem")]
        public DateTime NgayThem { get; set; }

        [JsonPropertyName("tinhTrang")]
        [Required(ErrorMessage = "Tình trạng không được để trống.")]
        [StringLength(20, ErrorMessage = "Tình trạng sản phẩm không được vượt quá 20 ký tự.")]
        public string TinhTrang { get; set; }

        public List<string> DanhSachAnh { get; set; } = new List<string>();
        [JsonPropertyName("anhSPs")]
        public List<IFormFile> AnhSPs { get; set; }


        // Thêm danh sách đánh giá vào ViewModel
        public List<DanhGiaViewModel> DanhGiaSanPham { get; set; } = new List<DanhGiaViewModel>();

    }


    public class DanhGiaViewModel
    {
        public string MaDanhGia { get; set; }
        public string MaSP { get; set; }
        public string MaKH { get; set; }
        public string? Email { get; set; }
        public int DiemDanhGia { get; set; }
        public string BinhLuan { get; set; }
        public string? TrangThai { get; set; }
        public DateTime NgayDanhGia { get; set; }
    }
}
