using System.ComponentModel.DataAnnotations;

namespace DynaDevAPI.Models
{

    public class OrderHistory
    {
        [Key]
        public int OrderHistoryId { get; set; } // Mã lịch sử thay đổi
        public string OrderId { get; set; } // Mã đơn hàng
        public string Status { get; set; } // Trạng thái đơn hàng
        public string ShippingAddress { get; set; } // Địa chỉ giao hàng
/*        public DateTime DateChanged { get; set; } // Thời gian thay đổi
        public string ChangedBy { get; set; } // Người thay đổi (Admin hoặc hệ thống)*/
    }

}
