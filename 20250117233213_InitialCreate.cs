using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DynaDevAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "KhachHangs",
                keyColumn: "MaKH",
                keyValue: "KH001",
                column: "NgayDangKy",
                value: new DateTime(2025, 1, 18, 6, 32, 12, 962, DateTimeKind.Local).AddTicks(5131));

            migrationBuilder.UpdateData(
                table: "KhachHangs",
                keyColumn: "MaKH",
                keyValue: "KH002",
                column: "NgayDangKy",z
                value: new DateTime(2025, 1, 18, 6, 32, 12, 963, DateTimeKind.Local).AddTicks(2360));

            migrationBuilder.UpdateData(
                table: "KhachHangs",
                keyColumn: "MaKH",
                keyValue: "KH003",
                column: "NgayDangKy",
                value: new DateTime(2025, 1, 18, 6, 32, 12, 963, DateTimeKind.Local).AddTicks(2371));

            migrationBuilder.UpdateData(
                table: "NhanViens",
                keyColumn: "MaNV",
                keyValue: "NV001",
                column: "NgayVaoLam",
                value: new DateTime(2025, 1, 18, 6, 32, 12, 963, DateTimeKind.Local).AddTicks(6914));

            migrationBuilder.UpdateData(
                table: "NhanViens",
                keyColumn: "MaNV",
                keyValue: "NV002",
                column: "NgayVaoLam",
                value: new DateTime(2025, 1, 18, 6, 32, 12, 963, DateTimeKind.Local).AddTicks(7039));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "KhachHangs",
                keyColumn: "MaKH",
                keyValue: "KH001",
                column: "NgayDangKy",
                value: new DateTime(2025, 1, 18, 6, 26, 32, 422, DateTimeKind.Local).AddTicks(3075));

            migrationBuilder.UpdateData(
                table: "KhachHangs",
                keyColumn: "MaKH",
                keyValue: "KH002",
                column: "NgayDangKy",
                value: new DateTime(2025, 1, 18, 6, 26, 32, 423, DateTimeKind.Local).AddTicks(1163));

            migrationBuilder.UpdateData(
                table: "KhachHangs",
                keyColumn: "MaKH",
                keyValue: "KH003",
                column: "NgayDangKy",
                value: new DateTime(2025, 1, 18, 6, 26, 32, 423, DateTimeKind.Local).AddTicks(1175));

            migrationBuilder.UpdateData(
                table: "NhanViens",
                keyColumn: "MaNV",
                keyValue: "NV001",
                column: "NgayVaoLam",
                value: new DateTime(2025, 1, 18, 6, 26, 32, 423, DateTimeKind.Local).AddTicks(5974));

            migrationBuilder.UpdateData(
                table: "NhanViens",
                keyColumn: "MaNV",
                keyValue: "NV002",
                column: "NgayVaoLam",
                value: new DateTime(2025, 1, 18, 6, 26, 32, 423, DateTimeKind.Local).AddTicks(6102));
        }
    }
}
