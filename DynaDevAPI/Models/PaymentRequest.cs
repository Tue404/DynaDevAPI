namespace DynaDevAPI.Models
{
    public class PaymentRequest
    {
        public string OrderId { get; set; }
        public double Amount { get; set; }
        public string OrderDescription { get; set; }
        public DateTime CreatedDate { get; set; }
    }

}
