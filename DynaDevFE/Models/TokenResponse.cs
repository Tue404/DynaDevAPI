using System.Text.Json.Serialization;

namespace DynaDevFE.Models
{
    public class TokenResponse
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }

        [JsonPropertyName("role")]
        public string Role { get; set; }

        [JsonPropertyName("MaKH")]
        public string MaKH { get; set; }

    }
}
