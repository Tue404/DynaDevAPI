using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DynaDevAPI.Migrations
{
    /// <inheritdoc />
    public partial class addNCCandVoucher : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DonHangs_Voucher_MaVoucher",
                table: "DonHangs");

            migrationBuilder.DropForeignKey(
                name: "FK_SanPhams_NhaCungCap_MaNCC",
                table: "SanPhams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Voucher",
                table: "Voucher");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NhaCungCap",
                table: "NhaCungCap");

            migrationBuilder.RenameTable(
                name: "Voucher",
                newName: "Vouchers");

            migrationBuilder.RenameTable(
                name: "NhaCungCap",
                newName: "NhaCungCaps");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vouchers",
                table: "Vouchers",
                column: "MaVoucher");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NhaCungCaps",
                table: "NhaCungCaps",
                column: "MaNCC");

            migrationBuilder.AddForeignKey(
                name: "FK_DonHangs_Vouchers_MaVoucher",
                table: "DonHangs",
                column: "MaVoucher",
                principalTable: "Vouchers",
                principalColumn: "MaVoucher",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SanPhams_NhaCungCaps_MaNCC",
                table: "SanPhams",
                column: "MaNCC",
                principalTable: "NhaCungCaps",
                principalColumn: "MaNCC",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DonHangs_Vouchers_MaVoucher",
                table: "DonHangs");

            migrationBuilder.DropForeignKey(
                name: "FK_SanPhams_NhaCungCaps_MaNCC",
                table: "SanPhams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vouchers",
                table: "Vouchers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NhaCungCaps",
                table: "NhaCungCaps");

            migrationBuilder.RenameTable(
                name: "Vouchers",
                newName: "Voucher");

            migrationBuilder.RenameTable(
                name: "NhaCungCaps",
                newName: "NhaCungCap");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Voucher",
                table: "Voucher",
                column: "MaVoucher");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NhaCungCap",
                table: "NhaCungCap",
                column: "MaNCC");

            migrationBuilder.AddForeignKey(
                name: "FK_DonHangs_Voucher_MaVoucher",
                table: "DonHangs",
                column: "MaVoucher",
                principalTable: "Voucher",
                principalColumn: "MaVoucher",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SanPhams_NhaCungCap_MaNCC",
                table: "SanPhams",
                column: "MaNCC",
                principalTable: "NhaCungCap",
                principalColumn: "MaNCC",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
