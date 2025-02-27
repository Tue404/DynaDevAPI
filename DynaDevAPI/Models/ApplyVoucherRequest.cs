namespace DynaDevAPI.Models
{
    public class ApplyVoucherRequest
    {
        public string VoucherCode { get; set; }
        public decimal OrderTotal { get; set; }
    }
}
