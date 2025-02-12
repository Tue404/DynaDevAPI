using System.Text.Json.Serialization;

namespace DynaDevFE.Models
{
    public class VoucherViewModel
    {
        public string MaVoucher { get; set; }
        [JsonPropertyName("tenVoucher")]
        public string TenVoucher { get; set; }
        public string MoTa { get; set; }
        public double GiamGia { get; set; }
        public string LoaiGiamGia { get; set; }
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }
        public string DieuKien { get; set; }
        public int SoLuong { get; set; }
        public string TrangThai { get; set; }
    }
}
