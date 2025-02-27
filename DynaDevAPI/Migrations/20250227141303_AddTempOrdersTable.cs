using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DynaDevAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddTempOrdersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TempOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaDH = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TempOrders", x => x.Id);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TempOrders");

            migrationBuilder.UpdateData(
                table: "DonHangs",
                keyColumn: "MaDH",
                keyValue: "DH001",
                column: "ThoiGianDatHang",
                value: new DateTime(2025, 2, 24, 11, 47, 36, 250, DateTimeKind.Local).AddTicks(8841));

            migrationBuilder.UpdateData(
                table: "DonHangs",
                keyColumn: "MaDH",
                keyValue: "DH002",
                column: "ThoiGianDatHang",
                value: new DateTime(2025, 2, 25, 11, 47, 36, 250, DateTimeKind.Local).AddTicks(9312));

            migrationBuilder.UpdateData(
                table: "KhachHangs",
                keyColumn: "MaKH",
                keyValue: "KH01",
                column: "NgayDangKy",
                value: new DateTime(2025, 2, 27, 11, 47, 36, 249, DateTimeKind.Local).AddTicks(9178));

            migrationBuilder.UpdateData(
                table: "NhanViens",
                keyColumn: "MaNV",
                keyValue: "NV01",
                column: "NgayVaoLam",
                value: new DateTime(2023, 2, 27, 11, 47, 36, 250, DateTimeKind.Local).AddTicks(8244));
        }
    }
}
