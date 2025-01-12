using System.ComponentModel.DataAnnotations;

namespace DynaDevAPI.Models
{
    public class SanPhamDto
    {
        public string MaSP { get; set; }
      
        public string MaLoai { get; set; }

        public string TenSanPham { get; set; }

        public decimal Gia { get; set; }

        public string MoTa { get; set; }

        public int SoLuongTrongKho { get; set; }

        public DateTime NgayThem { get; set; }

        public string TinhTrang { get; set; }

        public List<string> DanhSachAnh { get; set; } = new List<string>();
        public List<IFormFile> AnhSPs { get; set; }
        public string LoaiSP { get; set; }
    }
}
