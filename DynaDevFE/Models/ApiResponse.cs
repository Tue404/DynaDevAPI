using System.Text.Json.Serialization;

namespace DynaDevFE.Models
{
    public class ApiResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }
        [JsonPropertyName("message")]
        public string Message { get; set; }
        [JsonPropertyName("maDH")]
        public string MaDH { get; set; }
        [JsonPropertyName("tongTienSauGiam")]
        public decimal? TongTienSauGiam { get; set; } // Thêm thuộc tính này (nullable để xử lý trường hợp không có voucher)
        [JsonPropertyName("giamGia")]
        public decimal? GiamGia { get; set; } // Thêm thuộc tính này (nullable để xử lý trường hợp không có voucher)
    }
}
