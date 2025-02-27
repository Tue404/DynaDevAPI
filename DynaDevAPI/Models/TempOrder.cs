namespace DynaDevAPI.Models
{
    public class TempOrder
    {
        public int Id { get; set; } // Khóa chính
        public string MaDH { get; set; } // Mã đơn hàng tạm
        public string RequestData { get; set; } // Dữ liệu đơn hàng dạng JSON
        public DateTime CreatedDate { get; set; } // Thời gian tạo
    }
}
