namespace DynaDevFE.Models
{
    public class PaymentRequest
    {
        public string OrderId { get; set; }
        public int Amount { get; set; }
        public string OrderDescription { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class PaymentResponse1
    {
        public string PaymentUrl { get; set; }
    }
}
