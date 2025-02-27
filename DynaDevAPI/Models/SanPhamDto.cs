using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DynaDevAPI.Models
{
    public class SanPhamDto
    {
        public string MaSP { get; set; }
        public string MaLoai { get; set; }
        public string TenSanPham { get; set; }
        public string TacGia { get; set; }
        public string TenNCC { get; set; }
        public int NamXuatBan { set; get; }
        public decimal Gia { get; set; }
        public string MoTa { get; set; }
        public int SoLuongTrongKho { get; set; }
        public string TinhTrang { get; set; }
        public string MaNCC { get; set; }
        public List<string> DanhSachAnh { get; set; } = new List<string>();

        public List<IFormFile> AnhSPs { get; set; }
        public string LoaiSP { get; set; }


        // Danh sách đánh giá sản phẩm
        public List<DanhGiaDto> DanhGiaSanPham { get; set; } = new List<DanhGiaDto>();
    }

    public class DanhGiaDto
    {
        [JsonIgnore]
        public string? MaDanhGia { get; set; }
        public string MaSP { get; set; }
        public string MaKH { get; set; }
        public string? Email { get; set; }
        public int DiemDanhGia { get; set; }
        public string BinhLuan { get; set; }
        public string? TrangThai { get; set; }
        public DateTime NgayDanhGia { get; set; }
    }
}
