using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DynaDevAPI.Migrations
{
    /// <inheritdoc />
    public partial class addnxb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NhaXuatBan",
                table: "SanPhams",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NhaXuatBan",
                table: "SanPhams");
        }
    }
}
