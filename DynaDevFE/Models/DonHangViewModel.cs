using DynaDevAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DynaDevFE.Models
{
    public class DonHangViewModel
    {
        public string MaDH { get; set; }
        public string MaKH { get; set; }
        public string TenNguoiNhan { get; set; }
        public string SDT { get; set; }

        [JsonPropertyName("paymentStatus")]
        public string ThongTinThanhToan { get; set; }

        [JsonPropertyName("orderStatus")]
        public string TinhTrang { get; set; }

        public string  DiaChiNhanHang{ get; set; }
        public DateTime ThoiGianDatHang { get; set; }
        public decimal TongTien { get; set; }
        public string? MaNV { get; set; }
    }
  
}
