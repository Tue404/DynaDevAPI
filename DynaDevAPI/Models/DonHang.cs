using System.ComponentModel.DataAnnotations;

namespace DynaDevAPI.Models
{
    public class DonHang
    {
        [Key]
        public string MaDH { get; set; }
        public string MaKH { get; set; }
        public string ThongTinThanhToan { get; set; }
        public string DiaChiNhanHang { get; set; }
        public DateTime ThoiGianDatHang { get; set; }
        public decimal TongTien { get; set; }
        public string TinhTrang { get; set; }
        public string? MaNV { get; set; }

        public KhachHang KhachHang { get; set; }
        public NhanVien NhanVien { get; set; }
        public ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; }
    }
}
