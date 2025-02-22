using System.ComponentModel.DataAnnotations;

namespace DynaDevAPI.ViewModels
{
    public class KhachHangVM
    {
        public string MaKH { get; set; }
        public string ?TenKH { get; set; }

        public string ?Email { get; set; }
        public string ?MatKhau { get; set; }
        public string? SDT { get; set; }
        public string ?DiaChi { get; set; }
        public string? TinhTrang { get; set; }
        public DateTime NgayDangKy { get; set; }
    }
}
