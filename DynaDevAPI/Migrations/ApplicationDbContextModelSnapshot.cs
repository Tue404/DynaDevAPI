﻿// <auto-generated />
using System;
using DynaDevAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DynaDevAPI.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DonHang", b =>
                {
                    b.Property<string>("MaDH")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("DiaChiNhanHang")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MaKH")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MaNV")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MaVoucher")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("OrderStatusId")
                        .HasColumnType("int");

                    b.Property<int>("PaymentStatusId")
                        .HasColumnType("int");

                    b.Property<string>("PhuongThucThanhToan")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SoDienThoai")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TenNguoiNhan")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ThoiGianDatHang")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("TongTien")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("MaDH");

                    b.HasIndex("MaKH");

                    b.HasIndex("MaNV");

                    b.HasIndex("MaVoucher");

                    b.HasIndex("OrderStatusId");

                    b.HasIndex("PaymentStatusId");

                    b.ToTable("DonHangs");

                    b.HasData(
                        new
                        {
                            MaDH = "DH001",
                            DiaChiNhanHang = "123 Đường Văn Học, Hà Nội",
                            MaKH = "KH01",
                            MaNV = "NV01",
                            OrderStatusId = 1,
                            PaymentStatusId = 1,
                            ThoiGianDatHang = new DateTime(2025, 2, 19, 16, 34, 1, 154, DateTimeKind.Local).AddTicks(208),
                            TongTien = 240000m
                        },
                        new
                        {
                            MaDH = "DH002",
                            DiaChiNhanHang = "456 Đường Khoa Học, TP.HCM",
                            MaKH = "KH01",
                            MaNV = "NV01",
                            OrderStatusId = 3,
                            PaymentStatusId = 2,
                            ThoiGianDatHang = new DateTime(2025, 2, 20, 16, 34, 1, 154, DateTimeKind.Local).AddTicks(685),
                            TongTien = 90000m
                        });
                });

            modelBuilder.Entity("DynaDevAPI.Models.AnhSP", b =>
                {
                    b.Property<string>("MaAnh")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MaSP")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("TenAnh")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MaAnh");

                    b.HasIndex("MaSP");

                    b.ToTable("AnhSPs");
                });

            modelBuilder.Entity("DynaDevAPI.Models.ChiTietDonHang", b =>
                {
                    b.Property<string>("MaChiTiet")
                        .HasColumnType("nvarchar(450)");

                    b.Property<decimal>("Gia")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("MaDH")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MaSP")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("SoLuong")
                        .HasColumnType("int");

                    b.HasKey("MaChiTiet");

                    b.HasIndex("MaDH");

                    b.HasIndex("MaSP");

                    b.ToTable("ChiTietDonHangs");
                });

            modelBuilder.Entity("DynaDevAPI.Models.DanhGia", b =>
                {
                    b.Property<string>("MaDanhGia")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("BinhLuan")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DiemDanhGia")
                        .HasColumnType("int");

                    b.Property<string>("KhachHangMaKH")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MaKH")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MaSP")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("NgayDanhGia")
                        .HasColumnType("datetime2");

                    b.Property<string>("SanPhamMaSP")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("MaDanhGia");

                    b.HasIndex("KhachHangMaKH");

                    b.HasIndex("SanPhamMaSP");

                    b.ToTable("DanhGias");
                });

            modelBuilder.Entity("DynaDevAPI.Models.KhachHang", b =>
                {
                    b.Property<string>("MaKH")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("DiaChi")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MatKhau")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("NgayDangKy")
                        .HasColumnType("datetime2");

                    b.Property<string>("SDT")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TenKH")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TinhTrang")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MaKH");

                    b.ToTable("KhachHangs");

                    b.HasData(
                        new
                        {
                            MaKH = "KH01",
                            DiaChi = "Hà Nội",
                            Email = "vana@gmail.com",
                            MatKhau = "123456",
                            NgayDangKy = new DateTime(2025, 2, 22, 16, 34, 1, 152, DateTimeKind.Local).AddTicks(9682),
                            SDT = "0123456789",
                            TenKH = "Nguyễn Văn A",
                            TinhTrang = "Hoạt động"
                        });
                });

            modelBuilder.Entity("DynaDevAPI.Models.LoaiSP", b =>
                {
                    b.Property<string>("MaLoai")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AnhLoai")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MoTa")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TenLoai")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MaLoai");

                    b.ToTable("LoaiSPs", (string)null);

                    b.HasData(
                        new
                        {
                            MaLoai = "1",
                            MoTa = "Danh mục sách và truyện tranh",
                            TenLoai = "Sách - Truyện Tranh"
                        },
                        new
                        {
                            MaLoai = "2",
                            MoTa = "Dụng cụ văn phòng phẩm",
                            TenLoai = "Dụng Cụ Vẽ - VPP"
                        },
                        new
                        {
                            MaLoai = "3",
                            MoTa = "Băng đĩa và phụ kiện số",
                            TenLoai = "Băng Đĩa - Phụ Kiện Số"
                        });
                });

            modelBuilder.Entity("DynaDevAPI.Models.NhaCungCap", b =>
                {
                    b.Property<string>("MaNCC")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("DiaChi")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SDT")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TenNCC")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TinhTrang")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MaNCC");

                    b.ToTable("NhaCungCaps");
                });

            modelBuilder.Entity("DynaDevAPI.Models.NhanVien", b =>
                {
                    b.Property<string>("MaNV")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("DiaChi")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Luong")
                        .HasColumnType("real");

                    b.Property<string>("MatKhau")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("NgayVaoLam")
                        .HasColumnType("datetime2");

                    b.Property<string>("SDT")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TenNV")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TinhTrang")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MaNV");

                    b.ToTable("NhanViens");

                    b.HasData(
                        new
                        {
                            MaNV = "NV01",
                            DiaChi = "TP.HCM",
                            Email = "vanb@gmail.com",
                            Luong = 0f,
                            MatKhau = "admin123",
                            NgayVaoLam = new DateTime(2023, 2, 22, 16, 34, 1, 153, DateTimeKind.Local).AddTicks(9543),
                            SDT = "0987654321",
                            TenNV = "Trần Văn B",
                            TinhTrang = "Đang làm việc"
                        });
                });

            modelBuilder.Entity("DynaDevAPI.Models.OrderStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("OrderStatuses");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Chưa xác nhận"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Đã xác nhận"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Đang chuẩn bị hàng"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Đang giao hàng"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Đã giao hàng"
                        },
                        new
                        {
                            Id = 6,
                            Name = "Đã hủy"
                        });
                });

            modelBuilder.Entity("DynaDevAPI.Models.PaymentStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("PaymentStatuses");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Chưa thanh toán"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Đã thanh toán"
                        });
                });

            modelBuilder.Entity("DynaDevAPI.Models.SanPham", b =>
                {
                    b.Property<string>("MaSP")
                        .HasColumnType("nvarchar(450)");

                    b.Property<decimal>("Gia")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("MaLoai")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MaNCC")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MoTa")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int>("NamXuatBan")
                        .HasColumnType("int");

                    b.Property<DateTime>("NgayThem")
                        .HasColumnType("datetime2");

                    b.Property<string>("NhaXuatBan")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SoLuongTrongKho")
                        .HasColumnType("int");

                    b.Property<string>("TacGia")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TenSanPham")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TinhTrang")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MaSP");

                    b.HasIndex("MaLoai");

                    b.HasIndex("MaNCC");

                    b.ToTable("SanPhams");
                });

            modelBuilder.Entity("DynaDevAPI.Models.Voucher", b =>
                {
                    b.Property<string>("MaVoucher")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("DieuKien")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("GiamGia")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("LoaiGiamGia")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MoTa")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("NgayBatDau")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("NgayKetThuc")
                        .HasColumnType("datetime2");

                    b.Property<int>("SoLuong")
                        .HasColumnType("int");

                    b.Property<string>("TenVoucher")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TrangThai")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MaVoucher");

                    b.ToTable("Vouchers");
                });

            modelBuilder.Entity("DonHang", b =>
                {
                    b.HasOne("DynaDevAPI.Models.KhachHang", "KhachHang")
                        .WithMany("DonHangs")
                        .HasForeignKey("MaKH")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DynaDevAPI.Models.NhanVien", "NhanVien")
                        .WithMany("DonHangs")
                        .HasForeignKey("MaNV");

                    b.HasOne("DynaDevAPI.Models.Voucher", "Voucher")
                        .WithMany("DonHangs")
                        .HasForeignKey("MaVoucher");

                    b.HasOne("DynaDevAPI.Models.OrderStatus", "OrderStatus")
                        .WithMany("DonHangs")
                        .HasForeignKey("OrderStatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DynaDevAPI.Models.PaymentStatus", "PaymentStatus")
                        .WithMany("DonHangs")
                        .HasForeignKey("PaymentStatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("KhachHang");

                    b.Navigation("NhanVien");

                    b.Navigation("OrderStatus");

                    b.Navigation("PaymentStatus");

                    b.Navigation("Voucher");
                });

            modelBuilder.Entity("DynaDevAPI.Models.AnhSP", b =>
                {
                    b.HasOne("DynaDevAPI.Models.SanPham", "SanPham")
                        .WithMany("AnhSPs")
                        .HasForeignKey("MaSP")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SanPham");
                });

            modelBuilder.Entity("DynaDevAPI.Models.ChiTietDonHang", b =>
                {
                    b.HasOne("DonHang", "DonHang")
                        .WithMany("ChiTietDonHangs")
                        .HasForeignKey("MaDH")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DynaDevAPI.Models.SanPham", "SanPham")
                        .WithMany("ChiTietDonHangs")
                        .HasForeignKey("MaSP")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DonHang");

                    b.Navigation("SanPham");
                });

            modelBuilder.Entity("DynaDevAPI.Models.DanhGia", b =>
                {
                    b.HasOne("DynaDevAPI.Models.KhachHang", "KhachHang")
                        .WithMany("DanhGias")
                        .HasForeignKey("KhachHangMaKH")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DynaDevAPI.Models.SanPham", "SanPham")
                        .WithMany("DanhGias")
                        .HasForeignKey("SanPhamMaSP")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("KhachHang");

                    b.Navigation("SanPham");
                });

            modelBuilder.Entity("DynaDevAPI.Models.SanPham", b =>
                {
                    b.HasOne("DynaDevAPI.Models.LoaiSP", "LoaiSP")
                        .WithMany()
                        .HasForeignKey("MaLoai")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DynaDevAPI.Models.NhaCungCap", "NhaCungCap")
                        .WithMany("SanPhams")
                        .HasForeignKey("MaNCC")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("LoaiSP");

                    b.Navigation("NhaCungCap");
                });

            modelBuilder.Entity("DonHang", b =>
                {
                    b.Navigation("ChiTietDonHangs");
                });

            modelBuilder.Entity("DynaDevAPI.Models.KhachHang", b =>
                {
                    b.Navigation("DanhGias");

                    b.Navigation("DonHangs");
                });

            modelBuilder.Entity("DynaDevAPI.Models.NhaCungCap", b =>
                {
                    b.Navigation("SanPhams");
                });

            modelBuilder.Entity("DynaDevAPI.Models.NhanVien", b =>
                {
                    b.Navigation("DonHangs");
                });

            modelBuilder.Entity("DynaDevAPI.Models.OrderStatus", b =>
                {
                    b.Navigation("DonHangs");
                });

            modelBuilder.Entity("DynaDevAPI.Models.PaymentStatus", b =>
                {
                    b.Navigation("DonHangs");
                });

            modelBuilder.Entity("DynaDevAPI.Models.SanPham", b =>
                {
                    b.Navigation("AnhSPs");

                    b.Navigation("ChiTietDonHangs");

                    b.Navigation("DanhGias");
                });

            modelBuilder.Entity("DynaDevAPI.Models.Voucher", b =>
                {
                    b.Navigation("DonHangs");
                });
#pragma warning restore 612, 618
        }
    }
}
