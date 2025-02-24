using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DynaDevAPI.Models
{
    public class Voucher
    {
        [Key]
        public string MaVoucher { get; set; }
        public string TenVoucher { get; set; }
        public string MoTa { get; set; }
        public decimal GiamGia { get; set; }
        public string LoaiGiamGia { get; set; }
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }
        public string DieuKien { get; set; }
        public int SoLuong { get; set; }
        public string TrangThai { get; set; }

        [JsonIgnore]
        public ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();
    }
}
