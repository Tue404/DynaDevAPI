using DynaDevAPI.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class DonHang
{
    [Key]
    public string MaDH { get; set; }

    // Khóa ngoại liên kết tới KhachHang
    [Required]
    [ForeignKey("KhachHang")]
    public string MaKH { get; set; }
    public KhachHang KhachHang { get; set; }    

    [ForeignKey("MaVoucher")]
    public string? MaVoucher { get; set; }
    public string? TenNguoiNhan { get; set; } 
    public string? SoDienThoai { get; set; }
    public string DiaChiNhanHang { get; set; }
    public string? PhuongThucThanhToan {  get; set; }
    public DateTime ThoiGianDatHang { get; set; }
    public decimal TongTien { get; set; }

    // Khóa ngoại liên kết tới PaymentStatus
    public int PaymentStatusId { get; set; }
    public PaymentStatus PaymentStatus { get; set; }

    // Khóa ngoại liên kết tới OrderStatus
    public int OrderStatusId { get; set; }
    public OrderStatus OrderStatus { get; set; }

    // Khóa ngoại liên kết tới NhanVien
    [ForeignKey("NhanVien")]
    public string? MaNV { get; set; }
    public NhanVien NhanVien { get; set; }

    public Voucher? Voucher { get; set; }
    public ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; }
}