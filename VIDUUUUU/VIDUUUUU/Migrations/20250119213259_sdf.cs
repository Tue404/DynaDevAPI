using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VIDUUUUU.Migrations
{
    public partial class sdf : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "NguoiDungs",
                columns: new[] { "ID", "Email", "HoTen", "Password", "UserName" },
                values: new object[] { 1, "admin@example.com", "Admin User", "admin123", "admin" });

            migrationBuilder.InsertData(
                table: "NguoiDungs",
                columns: new[] { "ID", "Email", "HoTen", "Password", "UserName" },
                values: new object[] { 2, "user1@example.com", "User One", "password1", "user1" });

            migrationBuilder.InsertData(
                table: "NguoiDungs",
                columns: new[] { "ID", "Email", "HoTen", "Password", "UserName" },
                values: new object[] { 3, "user2@example.com", "User Two", "password2", "user2" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "NguoiDungs",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "NguoiDungs",
                keyColumn: "ID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "NguoiDungs",
                keyColumn: "ID",
                keyValue: 3);
        }
    }
}
