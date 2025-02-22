namespace DynaDevAPI.Models
{
    public class DatHangRequest
    {
        public string MaKH { get; set; }
        public string DiaChiNhanHang { get; set; }
        public DateTime ThoiGianDatHang { get; set; }
        public string PhuongThucThanhToan { get; set; }

        public List<ChiTietDonHangRequest> GioHang { get; set; }
    }
}
