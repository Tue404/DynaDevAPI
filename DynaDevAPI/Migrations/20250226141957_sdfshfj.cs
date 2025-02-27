using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DynaDevAPI.Migrations
{
    /// <inheritdoc />
    public partial class sdfshfj : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "DonHangs",
                keyColumn: "MaDH",
                keyValue: "DH001",
                column: "ThoiGianDatHang",
                value: new DateTime(2025, 2, 23, 21, 19, 53, 10, DateTimeKind.Local).AddTicks(5356));

            migrationBuilder.UpdateData(
                table: "DonHangs",
                keyColumn: "MaDH",
                keyValue: "DH002",
                column: "ThoiGianDatHang",
                value: new DateTime(2025, 2, 24, 21, 19, 53, 10, DateTimeKind.Local).AddTicks(5844));

            migrationBuilder.UpdateData(
                table: "KhachHangs",
                keyColumn: "MaKH",
                keyValue: "KH01",
                column: "NgayDangKy",
                value: new DateTime(2025, 2, 26, 21, 19, 53, 9, DateTimeKind.Local).AddTicks(4489));

            migrationBuilder.UpdateData(
                table: "NhanViens",
                keyColumn: "MaNV",
                keyValue: "NV01",
                column: "NgayVaoLam",
                value: new DateTime(2023, 2, 26, 21, 19, 53, 10, DateTimeKind.Local).AddTicks(4694));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "DonHangs",
                keyColumn: "MaDH",
                keyValue: "DH001",
                column: "ThoiGianDatHang",
                value: new DateTime(2025, 2, 23, 11, 22, 26, 73, DateTimeKind.Local).AddTicks(8076));

            migrationBuilder.UpdateData(
                table: "DonHangs",
                keyColumn: "MaDH",
                keyValue: "DH002",
                column: "ThoiGianDatHang",
                value: new DateTime(2025, 2, 24, 11, 22, 26, 73, DateTimeKind.Local).AddTicks(8636));

            migrationBuilder.UpdateData(
                table: "KhachHangs",
                keyColumn: "MaKH",
                keyValue: "KH01",
                column: "NgayDangKy",
                value: new DateTime(2025, 2, 26, 11, 22, 26, 72, DateTimeKind.Local).AddTicks(7181));

            migrationBuilder.UpdateData(
                table: "NhanViens",
                keyColumn: "MaNV",
                keyValue: "NV01",
                column: "NgayVaoLam",
                value: new DateTime(2023, 2, 26, 11, 22, 26, 73, DateTimeKind.Local).AddTicks(7288));
        }
    }
}
