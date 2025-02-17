using System.ComponentModel.DataAnnotations;

namespace DynaDevAPI.Models
{
    public class LoaiSP
    {
        [Key]
        public string MaLoai { get; set; }
        public string TenLoai { get; set; }
        public string? AnhLoai { get; set; }

        public string? MoTa { get; set; }
        //public ICollection<SanPham> SanPhams { get; set; }
    }
}
