namespace DynaDevAPI.Models
{
    public class PaymentStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<DonHang> DonHangs { get; set; }
    }
}
