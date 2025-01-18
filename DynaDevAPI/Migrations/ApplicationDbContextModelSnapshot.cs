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

                    b.Property<string>("MaKH")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MaSP")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("NgayDanhGia")
                        .HasColumnType("datetime2");

                    b.HasKey("MaDanhGia");

                    b.HasIndex("MaKH");

                    b.HasIndex("MaSP");

                    b.ToTable("DanhGias");
                });

            modelBuilder.Entity("DynaDevAPI.Models.DonHang", b =>
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
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("ThoiGianDatHang")
                        .HasColumnType("datetime2");

                    b.Property<string>("ThongTinThanhToan")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TinhTrang")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("TongTien")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("MaDH");

                    b.HasIndex("MaKH");

                    b.HasIndex("MaNV");

                    b.HasIndex("MaVoucher");

                    b.ToTable("DonHangs");
                });

            modelBuilder.Entity("DynaDevAPI.Models.KhachHang", b =>
                {
                    b.Property<string>("MaKH")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("DiaChi")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MatKhau")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("NgayDangKy")
                        .HasColumnType("datetime2");

                    b.Property<string>("SDT")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TenKH")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TinhTrang")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MaKH");

                    b.ToTable("KhachHangs");
                });

            modelBuilder.Entity("DynaDevAPI.Models.LoaiSP", b =>
                {
                    b.Property<string>("MaLoai")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AnhLoai")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TenLoai")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MaLoai");

                    b.ToTable("LoaiSPs");
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
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SoLuongTrongKho")
                        .HasColumnType("int");

                    b.Property<string>("TacGia")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TenSanPham")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("TinhTrang")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

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
                    b.HasOne("DynaDevAPI.Models.DonHang", "DonHang")
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
                        .HasForeignKey("MaKH")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DynaDevAPI.Models.SanPham", "SanPham")
                        .WithMany("DanhGias")
                        .HasForeignKey("MaSP")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("KhachHang");

                    b.Navigation("SanPham");
                });

            modelBuilder.Entity("DynaDevAPI.Models.DonHang", b =>
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
                        .HasForeignKey("MaVoucher")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("KhachHang");

                    b.Navigation("NhanVien");

                    b.Navigation("Voucher");
                });

            modelBuilder.Entity("DynaDevAPI.Models.SanPham", b =>
                {
                    b.HasOne("DynaDevAPI.Models.LoaiSP", "LoaiSP")
                        .WithMany("SanPhams")
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

            modelBuilder.Entity("DynaDevAPI.Models.DonHang", b =>
                {
                    b.Navigation("ChiTietDonHangs");
                });

            modelBuilder.Entity("DynaDevAPI.Models.KhachHang", b =>
                {
                    b.Navigation("DanhGias");

                    b.Navigation("DonHangs");
                });

            modelBuilder.Entity("DynaDevAPI.Models.LoaiSP", b =>
                {
                    b.Navigation("SanPhams");
                });

            modelBuilder.Entity("DynaDevAPI.Models.NhaCungCap", b =>
                {
                    b.Navigation("SanPhams");
                });

            modelBuilder.Entity("DynaDevAPI.Models.NhanVien", b =>
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
