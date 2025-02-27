namespace DynaDevFE.Models
{
    public class CheckVoucherResponse
    {
        public bool Success { get; set; }
        public decimal TongTienSauGiam { get; set; }
        public decimal GiamGia { get; set; }
        public string Message { get; set; }
    }
}
