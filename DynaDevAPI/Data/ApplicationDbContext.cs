using DynaDevAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace DynaDevAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<KhachHang> KhachHangs { get; set; }
        public DbSet<NhanVien> NhanViens { get; set; }
        public DbSet<LoaiSP> LoaiSPs { get; set; }
        public DbSet<SanPham> SanPhams { get; set; }
        public DbSet<AnhSP> AnhSPs { get; set; }
        public DbSet<DonHang> DonHangs { get; set; }
        public DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; }
        public DbSet<DanhGia> DanhGias { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<SanPham>()
                .HasOne(sp => sp.LoaiSP)
                .WithMany(ls => ls.SanPhams)
                .HasForeignKey(sp => sp.MaLoai);

            modelBuilder.Entity<AnhSP>()
                .HasOne(asp => asp.SanPham)
                .WithMany(sp => sp.AnhSPs)
                .HasForeignKey(asp => asp.MaSP);

            modelBuilder.Entity<DonHang>()
                .HasOne(dh => dh.KhachHang)
                .WithMany(kh => kh.DonHangs)
                .HasForeignKey(dh => dh.MaKH);

            modelBuilder.Entity<DonHang>()
                .HasOne(dh => dh.NhanVien)
                .WithMany(nv => nv.DonHangs)
                .HasForeignKey(dh => dh.MaNV);

            modelBuilder.Entity<ChiTietDonHang>()
                .HasOne(ctdh => ctdh.DonHang)
                .WithMany(dh => dh.ChiTietDonHangs)
                .HasForeignKey(ctdh => ctdh.MaDH);

            modelBuilder.Entity<ChiTietDonHang>()
                .HasOne(ctdh => ctdh.SanPham)
                .WithMany(sp => sp.ChiTietDonHangs)
                .HasForeignKey(ctdh => ctdh.MaSP);

            modelBuilder.Entity<DanhGia>()
                .HasOne(dg => dg.KhachHang)
                .WithMany(kh => kh.DanhGias)
                .HasForeignKey(dg => dg.MaKH);

            modelBuilder.Entity<DanhGia>()
                .HasOne(dg => dg.SanPham)
                .WithMany(sp => sp.DanhGias)
                .HasForeignKey(dg => dg.MaSP);
        }
    }
}
