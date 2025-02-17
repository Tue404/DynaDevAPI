using System.ComponentModel.DataAnnotations;

namespace DynaDevFE.Models
{
    // ViewModel để map dữ liệu
    public class DoanhThuTheoNgayThangNamViewModel
    {
        public DateTime Date { get; set; }
        public int Nam { get; set; }
        public int Thang { get; set; }
        public int Ngay { get; set; }
        public decimal TongDoanhThu { get; set; }
        public int TongDonHang { get; set; }
    }

}
