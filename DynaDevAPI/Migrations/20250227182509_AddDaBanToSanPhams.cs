using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DynaDevAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddDaBanToSanPhams : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DaBan",
                table: "SanPhams",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "DonHangs",
                keyColumn: "MaDH",
                keyValue: "DH001",
                column: "ThoiGianDatHang",
                value: new DateTime(2025, 2, 25, 1, 25, 9, 175, DateTimeKind.Local).AddTicks(552));

            migrationBuilder.UpdateData(
                table: "DonHangs",
                keyColumn: "MaDH",
                keyValue: "DH002",
                column: "ThoiGianDatHang",
                value: new DateTime(2025, 2, 26, 1, 25, 9, 175, DateTimeKind.Local).AddTicks(1171));

            migrationBuilder.UpdateData(
                table: "KhachHangs",
                keyColumn: "MaKH",
                keyValue: "KH01",
                column: "NgayDangKy",
                value: new DateTime(2025, 2, 28, 1, 25, 9, 174, DateTimeKind.Local).AddTicks(426));

            migrationBuilder.UpdateData(
                table: "NhanViens",
                keyColumn: "MaNV",
                keyValue: "NV01",
                column: "NgayVaoLam",
                value: new DateTime(2023, 2, 28, 1, 25, 9, 174, DateTimeKind.Local).AddTicks(9937));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DaBan",
                table: "SanPhams");

            migrationBuilder.UpdateData(
                table: "DonHangs",
                keyColumn: "MaDH",
                keyValue: "DH001",
                column: "ThoiGianDatHang",
                value: new DateTime(2025, 2, 24, 21, 13, 0, 500, DateTimeKind.Local).AddTicks(9290));

            migrationBuilder.UpdateData(
                table: "DonHangs",
                keyColumn: "MaDH",
                keyValue: "DH002",
                column: "ThoiGianDatHang",
                value: new DateTime(2025, 2, 25, 21, 13, 0, 500, DateTimeKind.Local).AddTicks(9791));

            migrationBuilder.UpdateData(
                table: "KhachHangs",
                keyColumn: "MaKH",
                keyValue: "KH01",
                column: "NgayDangKy",
                value: new DateTime(2025, 2, 27, 21, 13, 0, 499, DateTimeKind.Local).AddTicks(8158));

            migrationBuilder.UpdateData(
                table: "NhanViens",
                keyColumn: "MaNV",
                keyValue: "NV01",
                column: "NgayVaoLam",
                value: new DateTime(2023, 2, 27, 21, 13, 0, 500, DateTimeKind.Local).AddTicks(8645));
        }
    }
}
