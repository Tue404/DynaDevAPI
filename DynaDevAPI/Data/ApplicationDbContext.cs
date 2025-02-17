using DynaDevAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;


namespace DynaDevAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<KhachHang> KhachHangs { get; set; }
        public DbSet<NhaCungCap> NhaCungCaps { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<NhanVien> NhanViens { get; set; }
        public DbSet<LoaiSP> LoaiSPs { get; set; }
        public DbSet<SanPham> SanPhams { get; set; }
        public DbSet<AnhSP> AnhSPs { get; set; }
        public DbSet<DonHang> DonHangs { get; set; }
        public DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; }
        public DbSet<DanhGia> DanhGias { get; set; }
        public DbSet<PaymentStatus> PaymentStatuses { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }




        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SanPham>()
                .HasOne(sp => sp.LoaiSP)
                .WithMany()
                .HasForeignKey(sp => sp.MaLoai)
                .OnDelete(DeleteBehavior.Cascade); // Xóa LoaiSP sẽ xóa các sản phẩm liên quan


            // Cấu hình kiểu dữ liệu decimal
            modelBuilder.Entity<SanPham>()
                .Property(sp => sp.Gia)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<SanPham>()
                .HasOne(sp => sp.NhaCungCap)
                .WithMany(ncc => ncc.SanPhams)
                .HasForeignKey(sp => sp.MaNCC);

            modelBuilder.Entity<AnhSP>()
                .HasOne(asp => asp.SanPham)
                .WithMany(sp => sp.AnhSPs)
                .HasForeignKey(asp => asp.MaSP);

            modelBuilder.Entity<DonHang>()
                .HasOne(dh => dh.KhachHang)
                .WithMany(kh => kh.DonHangs)
                .HasForeignKey(dh => dh.MaKH);

            modelBuilder.Entity<DonHang>()
                .Property(dh => dh.TongTien)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<DonHang>()
                .HasOne(dh => dh.Voucher)
                .WithMany(vc => vc.DonHangs)
                .HasForeignKey(dh => dh.MaVoucher);

            modelBuilder.Entity<ChiTietDonHang>()
                .Property(ctdh => ctdh.Gia)
                .HasColumnType("decimal(18,2)");


            modelBuilder.Entity<LoaiSP>().ToTable("LoaiSPs");
            // Seed data cho bảng OrderStatus
            modelBuilder.Entity<OrderStatus>().HasData(
                new OrderStatus { Id = 1, Name = "Chưa xác nhận" },
                new OrderStatus { Id = 2, Name = "Đã xác nhận" },
                new OrderStatus { Id = 3, Name = "Đang chuẩn bị hàng" },
                new OrderStatus { Id = 4, Name = "Đang giao hàng" },
                new OrderStatus { Id = 5, Name = "Đã giao hàng" },
                new OrderStatus { Id = 6, Name = "Đã hủy" }
            );

            // Seed data cho bảng PaymentStatus
            modelBuilder.Entity<PaymentStatus>().HasData(
                new PaymentStatus { Id = 1, Name = "Chưa thanh toán" },
                new PaymentStatus { Id = 2, Name = "Đã thanh toán" }
            );

            // Seed data cho bảng KhachHang
            modelBuilder.Entity<KhachHang>().HasData(
                new KhachHang
                {
                    MaKH = "KH01",
                    TenKH = "Nguyễn Văn A",
                    Email = "vana@gmail.com",
                    MatKhau = "123456",
                    SDT = "0123456789",
                    DiaChi = "Hà Nội",
                    TinhTrang = "Hoạt động",
                    NgayDangKy = DateTime.Now
                }
            );

            // Seed data cho bảng NhanVien
            modelBuilder.Entity<NhanVien>().HasData(
                new NhanVien
                {
                    MaNV = "NV01",
                    TenNV = "Trần Văn B",
                    Email = "vanb@gmail.com",
                    MatKhau = "admin123",
                    SDT = "0987654321",
                    DiaChi = "TP.HCM",
                    TinhTrang = "Đang làm việc",
                    NgayVaoLam = DateTime.Now.AddYears(-2)
                }
            );

            // Seed data cho bảng DonHang
            modelBuilder.Entity<DonHang>().HasData(
                new DonHang
                {
                    MaDH = "DH001",
                    MaKH = "KH01", // Phải khớp với KhachHang.MaKH
                    DiaChiNhanHang = "123 Đường Văn Học, Hà Nội",
                    ThoiGianDatHang = DateTime.Now.AddDays(-3),
                    TongTien = 240000,
                    PaymentStatusId = 1, // Chưa thanh toán
                    OrderStatusId = 1,   // Chờ xử lý
                    MaNV = "NV01"        // Phải khớp với NhanVien.MaNV
                },
                new DonHang
                {
                    MaDH = "DH002",
                    MaKH = "KH01", // Phải khớp với KhachHang.MaKH
                    DiaChiNhanHang = "456 Đường Khoa Học, TP.HCM",
                    ThoiGianDatHang = DateTime.Now.AddDays(-2),
                    TongTien = 90000,
                    PaymentStatusId = 2, // Đã thanh toán
                    OrderStatusId = 3,   // Hoàn thành
                    MaNV = "NV01"        // Phải khớp với NhanVien.MaNV
                }
            );

            modelBuilder.Entity<LoaiSP>().HasData(
                new LoaiSP
                {
                    MaLoai = "1",
                    TenLoai = "Sách - Truyện Tranh",
                    MoTa = "Danh mục sách và truyện tranh",
                    AnhLoai = null
                },
                new LoaiSP
                {
                    MaLoai = "2",
                    TenLoai = "Dụng Cụ Vẽ - VPP",
                    MoTa = "Dụng cụ văn phòng phẩm",
                    AnhLoai = null
                },
                new LoaiSP
                {
                    MaLoai = "3",
                    TenLoai = "Băng Đĩa - Phụ Kiện Số",
                    MoTa = "Băng đĩa và phụ kiện số",
                    AnhLoai = null
                }
            );






        }
    }
}
