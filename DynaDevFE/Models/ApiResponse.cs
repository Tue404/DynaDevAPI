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
    }
}
