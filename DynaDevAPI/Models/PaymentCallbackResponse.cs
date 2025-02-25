namespace DynaDevAPI.Models
{
    public class PaymentCallbackResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string RedirectUrl { get; set; }
        public string OrderId { get; set; }
        public string TransactionId { get; set; }
    }
}
