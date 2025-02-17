using DynaDevAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DynaDevFE.Models
{
    public class LoaiSPViewModel
    {
        [JsonPropertyName("maLoai")]
        public string MaLoai { get; set; }
        [JsonPropertyName("tenLoai")]
        public string TenLoai { get; set; }
        [JsonPropertyName("moTa")]
        public string? MoTa { get; set; }
    }
}
