using System.ComponentModel.DataAnnotations;

namespace DynaDevAPI.Models
{
    public class NhaCungCap
    {
        [Key]
        public string MaNCC {  get; set; }
        public string TenNCC { get; set; }
        public string SDT {  get; set; }
        public string Email { get; set; }
        public string DiaChi { get; set; }
        public string TinhTrang { get; set; }

        public ICollection<SanPham> SanPhams { get; set; }
    }
}
