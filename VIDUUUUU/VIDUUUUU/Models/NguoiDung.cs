using System.ComponentModel.DataAnnotations;

namespace VIDUUUUU.Models
{
    public class NguoiDung
    {
        [Key]
        public int ID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string HoTen { get; set; }
        public string Email { get; set; }   
    }
}
