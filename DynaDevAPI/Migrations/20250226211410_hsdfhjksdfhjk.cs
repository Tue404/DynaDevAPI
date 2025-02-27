using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DynaDevAPI.Migrations
{
    /// <inheritdoc />
    public partial class hsdfhjksdfhjk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TrangThai",
                table: "DanhGias",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "DonHangs",
                keyColumn: "MaDH",
                keyValue: "DH001",
                column: "ThoiGianDatHang",
                value: new DateTime(2025, 2, 24, 4, 14, 10, 89, DateTimeKind.Local).AddTicks(9235));

            migrationBuilder.UpdateData(
                table: "DonHangs",
                keyColumn: "MaDH",
                keyValue: "DH002",
                column: "ThoiGianDatHang",
                value: new DateTime(2025, 2, 25, 4, 14, 10, 89, DateTimeKind.Local).AddTicks(9800));

            migrationBuilder.UpdateData(
                table: "KhachHangs",
                keyColumn: "MaKH",
                keyValue: "KH01",
                column: "NgayDangKy",
                value: new DateTime(2025, 2, 27, 4, 14, 10, 88, DateTimeKind.Local).AddTicks(7282));

            migrationBuilder.UpdateData(
                table: "NhanViens",
                keyColumn: "MaNV",
                keyValue: "NV01",
                column: "NgayVaoLam",
                value: new DateTime(2023, 2, 27, 4, 14, 10, 89, DateTimeKind.Local).AddTicks(8551));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrangThai",
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
    }
}
