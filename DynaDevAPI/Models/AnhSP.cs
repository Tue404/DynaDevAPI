using System.ComponentModel.DataAnnotations;

namespace DynaDevAPI.Models
{
    public class AnhSP
    {
        [Key]
        public string MaAnh { get; set; }
        public string MaSP { get; set; }
        public string TenAnh { get; set; }

        public SanPham SanPham { get; set; }
    }
}
