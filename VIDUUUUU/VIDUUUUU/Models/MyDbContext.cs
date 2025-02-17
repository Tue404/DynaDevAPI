using VIDUUUUU.Models;
using Microsoft.EntityFrameworkCore;

namespace VIDUUUUU.Data 
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions options) : base(options) { }

        #region DbSet
        public DbSet<NguoiDung> NguoiDungs { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed dữ liệu mẫu cho bảng NguoiDung
            modelBuilder.Entity<NguoiDung>().HasData(
                new NguoiDung
                {
                    ID = 1,
                    UserName = "admin",
                    Password = "admin123", // Nên mã hóa mật khẩu trong thực tế
                    HoTen = "Admin User",
                    Email = "admin@example.com"
                },
                new NguoiDung
                {
                    ID = 2,
                    UserName = "user1",
                    Password = "password1",
                    HoTen = "User One",
                    Email = "user1@example.com"
                },
                new NguoiDung
                {
                    ID = 3,
                    UserName = "user2",
                    Password = "password2",
                    HoTen = "User Two",
                    Email = "user2@example.com"
                }
            );
        }
    }
}
