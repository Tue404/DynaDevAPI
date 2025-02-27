using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DynaDevAPI.Migrations
{
    /// <inheritdoc />
    public partial class sdfsdf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TenKH",
                table: "DanhGias");

            migrationBuilder.UpdateData(
                table: "DonHangs",
                keyColumn: "MaDH",
                keyValue: "DH001",
                column: "ThoiGianDatHang",
                value: new DateTime(2025, 2, 24, 2, 5, 35, 112, DateTimeKind.Local).AddTicks(7309));

            migrationBuilder.UpdateData(
                table: "DonHangs",
                keyColumn: "MaDH",
                keyValue: "DH002",
                column: "ThoiGianDatHang",
                value: new DateTime(2025, 2, 25, 2, 5, 35, 112, DateTimeKind.Local).AddTicks(9007));

            migrationBuilder.UpdateData(
                table: "KhachHangs",
                keyColumn: "MaKH",
                keyValue: "KH01",
                column: "NgayDangKy",
                value: new DateTime(2025, 2, 27, 2, 5, 35, 111, DateTimeKind.Local).AddTicks(6996));

            migrationBuilder.UpdateData(
                table: "NhanViens",
                keyColumn: "MaNV",
                keyValue: "NV01",
                column: "NgayVaoLam",
                value: new DateTime(2023, 2, 27, 2, 5, 35, 112, DateTimeKind.Local).AddTicks(6173));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TenKH",
                table: "DanhGias",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "DonHangs",
                keyColumn: "MaDH",
                keyValue: "DH001",
                column: "ThoiGianDatHang",
                value: new DateTime(2025, 2, 24, 0, 57, 28, 91, DateTimeKind.Local).AddTicks(4764));

            migrationBuilder.UpdateData(
                table: "DonHangs",
                keyColumn: "MaDH",
                keyValue: "DH002",
                column: "ThoiGianDatHang",
                value: new DateTime(2025, 2, 25, 0, 57, 28, 91, DateTimeKind.Local).AddTicks(5262));

            migrationBuilder.UpdateData(
                table: "KhachHangs",
                keyColumn: "MaKH",
                keyValue: "KH01",
                column: "NgayDangKy",
                value: new DateTime(2025, 2, 27, 0, 57, 28, 90, DateTimeKind.Local).AddTicks(4712));

            migrationBuilder.UpdateData(
                table: "NhanViens",
                keyColumn: "MaNV",
                keyValue: "NV01",
                column: "NgayVaoLam",
                value: new DateTime(2023, 2, 27, 0, 57, 28, 91, DateTimeKind.Local).AddTicks(4094));
        }
    }
}
