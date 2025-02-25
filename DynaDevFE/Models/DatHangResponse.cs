namespace DynaDevFE.Models
{
    public class DatHangResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string MaDH { get; set; }
        public object OrderData { get; set; }
    }
}
