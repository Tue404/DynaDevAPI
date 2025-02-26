using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DynaDevAPI.Migrations
{
    /// <inheritdoc />
    public partial class hdhdhdhd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DanhGias_KhachHangs_KhachHangMaKH",
                table: "DanhGias");

            migrationBuilder.DropForeignKey(
                name: "FK_DanhGias_SanPhams_SanPhamMaSP",
                table: "DanhGias");

            migrationBuilder.DropIndex(
                name: "IX_DanhGias_KhachHangMaKH",
                table: "DanhGias");

            migrationBuilder.DropIndex(
                name: "IX_DanhGias_SanPhamMaSP",
                table: "DanhGias");

            migrationBuilder.DropColumn(
                name: "KhachHangMaKH",
                table: "DanhGias");

            migrationBuilder.DropColumn(
                name: "SanPhamMaSP",
                table: "DanhGias");

            migrationBuilder.AlterColumn<string>(
                name: "MaSP",
                table: "DanhGias",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "MaKH",
                table: "DanhGias",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "DonHangs",
                keyColumn: "MaDH",
                keyValue: "DH001",
                column: "ThoiGianDatHang",
                value: new DateTime(2025, 2, 23, 21, 29, 54, 485, DateTimeKind.Local).AddTicks(7228));

            migrationBuilder.UpdateData(
                table: "DonHangs",
                keyColumn: "MaDH",
                keyValue: "DH002",
                column: "ThoiGianDatHang",
                value: new DateTime(2025, 2, 24, 21, 29, 54, 485, DateTimeKind.Local).AddTicks(7787));

            migrationBuilder.UpdateData(
                table: "KhachHangs",
                keyColumn: "MaKH",
                keyValue: "KH01",
                column: "NgayDangKy",
                value: new DateTime(2025, 2, 26, 21, 29, 54, 484, DateTimeKind.Local).AddTicks(7141));

            migrationBuilder.UpdateData(
                table: "NhanViens",
                keyColumn: "MaNV",
                keyValue: "NV01",
                column: "NgayVaoLam",
                value: new DateTime(2023, 2, 26, 21, 29, 54, 485, DateTimeKind.Local).AddTicks(6543));

            migrationBuilder.CreateIndex(
                name: "IX_DanhGias_MaKH",
                table: "DanhGias",
                column: "MaKH");

            migrationBuilder.CreateIndex(
                name: "IX_DanhGias_MaSP",
                table: "DanhGias",
                column: "MaSP");

            migrationBuilder.AddForeignKey(
                name: "FK_DanhGias_KhachHangs_MaKH",
                table: "DanhGias",
                column: "MaKH",
                principalTable: "KhachHangs",
                principalColumn: "MaKH",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DanhGias_SanPhams_MaSP",
                table: "DanhGias",
                column: "MaSP",
                principalTable: "SanPhams",
                principalColumn: "MaSP",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DanhGias_KhachHangs_MaKH",
                table: "DanhGias");

            migrationBuilder.DropForeignKey(
                name: "FK_DanhGias_SanPhams_MaSP",
                table: "DanhGias");

            migrationBuilder.DropIndex(
                name: "IX_DanhGias_MaKH",
                table: "DanhGias");

            migrationBuilder.DropIndex(
                name: "IX_DanhGias_MaSP",
                table: "DanhGias");

            migrationBuilder.AlterColumn<string>(
                name: "MaSP",
                table: "DanhGias",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "MaKH",
                table: "DanhGias",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "KhachHangMaKH",
                table: "DanhGias",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SanPhamMaSP",
                table: "DanhGias",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

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

            migrationBuilder.CreateIndex(
                name: "IX_DanhGias_KhachHangMaKH",
                table: "DanhGias",
                column: "KhachHangMaKH");

            migrationBuilder.CreateIndex(
                name: "IX_DanhGias_SanPhamMaSP",
                table: "DanhGias",
                column: "SanPhamMaSP");

            migrationBuilder.AddForeignKey(
                name: "FK_DanhGias_KhachHangs_KhachHangMaKH",
                table: "DanhGias",
                column: "KhachHangMaKH",
                principalTable: "KhachHangs",
                principalColumn: "MaKH",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DanhGias_SanPhams_SanPhamMaSP",
                table: "DanhGias",
                column: "SanPhamMaSP",
                principalTable: "SanPhams",
                principalColumn: "MaSP",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
