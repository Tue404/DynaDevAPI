using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DynaDevAPI.Models
{
    public class SanPham
    {
        [Key]

        public string MaSP { get; set; }
        [Required(ErrorMessage = "Mã loại không được để trống.")]

        public string MaLoai { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm không được để trống.")]
        [StringLength(100, ErrorMessage = "Tên sản phẩm không được vượt quá 100 ký tự.")]
        public string TenSanPham { get; set; }
        [Required(ErrorMessage = "Tác giả không được để trống.")]
        public string TacGia { get; set; }
        public string? NhaXuatBan { get; set; }
        public int NamXuatBan {  set; get; }

        [Required(ErrorMessage = "Giá không được để trống.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Giá sản phẩm phải lớn hơn 0.")]
        public decimal Gia { get; set; }
        [StringLength(500, ErrorMessage = "Mô tả không được vượt quá 500 ký tự.")]
        public string MoTa { get; set; }

        [Required(ErrorMessage = "Số lượng trong kho không được để trống.")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng trong kho phải là số nguyên dương.")]
        public int SoLuongTrongKho { get; set; }

        public DateTime NgayThem { get; set; }
        [Required(ErrorMessage = "Tình trạng không được để trống.")]
        [StringLength(20, ErrorMessage = "Tình trạng sản phẩm không được vượt quá 20 ký tự.")]
        public string TinhTrang { get; set; }
        public string MaNCC { get; set; }

        public LoaiSP? LoaiSP { get; set; }
        public NhaCungCap? NhaCungCap { get; set; }
        public ICollection<AnhSP> AnhSPs { get; set; }
        public ICollection<ChiTietDonHang>? ChiTietDonHangs { get; set; }
        public ICollection<DanhGia>? DanhGias { get; set; }
    }
}
