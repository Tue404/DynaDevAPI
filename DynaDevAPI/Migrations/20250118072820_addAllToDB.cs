using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DynaDevAPI.Migrations
{
    /// <inheritdoc />
    public partial class addAllToDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KhachHangs",
                columns: table => new
                {
                    MaKH = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenKH = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MatKhau = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SDT = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiaChi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TinhTrang = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgayDangKy = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KhachHangs", x => x.MaKH);
                });

            migrationBuilder.CreateTable(
                name: "LoaiSPs",
                columns: table => new
                {
                    MaLoai = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenLoai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnhLoai = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoaiSPs", x => x.MaLoai);
                });

            migrationBuilder.CreateTable(
                name: "NhaCungCap",
                columns: table => new
                {
                    MaNCC = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenNCC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SDT = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiaChi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TinhTrang = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhaCungCap", x => x.MaNCC);
                });

            migrationBuilder.CreateTable(
                name: "NhanViens",
                columns: table => new
                {
                    MaNV = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenNV = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MatKhau = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SDT = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiaChi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TinhTrang = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgayVaoLam = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Luong = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhanViens", x => x.MaNV);
                });

            migrationBuilder.CreateTable(
                name: "Voucher",
                columns: table => new
                {
                    MaVoucher = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenVoucher = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GiamGia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LoaiGiamGia = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgayBatDau = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayKetThuc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DieuKien = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Voucher", x => x.MaVoucher);
                });

            migrationBuilder.CreateTable(
                name: "SanPhams",
                columns: table => new
                {
                    MaSP = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaLoai = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenSanPham = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TacGia = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NamXuatBan = table.Column<int>(type: "int", nullable: false),
                    Gia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    SoLuongTrongKho = table.Column<int>(type: "int", nullable: false),
                    NgayThem = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TinhTrang = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MaNCC = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SanPhams", x => x.MaSP);
                    table.ForeignKey(
                        name: "FK_SanPhams_LoaiSPs_MaLoai",
                        column: x => x.MaLoai,
                        principalTable: "LoaiSPs",
                        principalColumn: "MaLoai",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SanPhams_NhaCungCap_MaNCC",
                        column: x => x.MaNCC,
                        principalTable: "NhaCungCap",
                        principalColumn: "MaNCC",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DonHangs",
                columns: table => new
                {
                    MaDH = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaKH = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaVoucher = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ThongTinThanhToan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiaChiNhanHang = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ThoiGianDatHang = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TongTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TinhTrang = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaNV = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonHangs", x => x.MaDH);
                    table.ForeignKey(
                        name: "FK_DonHangs_KhachHangs_MaKH",
                        column: x => x.MaKH,
                        principalTable: "KhachHangs",
                        principalColumn: "MaKH",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DonHangs_NhanViens_MaNV",
                        column: x => x.MaNV,
                        principalTable: "NhanViens",
                        principalColumn: "MaNV");
                    table.ForeignKey(
                        name: "FK_DonHangs_Voucher_MaVoucher",
                        column: x => x.MaVoucher,
                        principalTable: "Voucher",
                        principalColumn: "MaVoucher",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnhSPs",
                columns: table => new
                {
                    MaAnh = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaSP = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenAnh = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnhSPs", x => x.MaAnh);
                    table.ForeignKey(
                        name: "FK_AnhSPs_SanPhams_MaSP",
                        column: x => x.MaSP,
                        principalTable: "SanPhams",
                        principalColumn: "MaSP",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DanhGias",
                columns: table => new
                {
                    MaDanhGia = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaSP = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaKH = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DiemDanhGia = table.Column<int>(type: "int", nullable: false),
                    BinhLuan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgayDanhGia = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DanhGias", x => x.MaDanhGia);
                    table.ForeignKey(
                        name: "FK_DanhGias_KhachHangs_MaKH",
                        column: x => x.MaKH,
                        principalTable: "KhachHangs",
                        principalColumn: "MaKH",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DanhGias_SanPhams_MaSP",
                        column: x => x.MaSP,
                        principalTable: "SanPhams",
                        principalColumn: "MaSP",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietDonHangs",
                columns: table => new
                {
                    MaChiTiet = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaDH = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaSP = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    Gia = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietDonHangs", x => x.MaChiTiet);
                    table.ForeignKey(
                        name: "FK_ChiTietDonHangs_DonHangs_MaDH",
                        column: x => x.MaDH,
                        principalTable: "DonHangs",
                        principalColumn: "MaDH",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietDonHangs_SanPhams_MaSP",
                        column: x => x.MaSP,
                        principalTable: "SanPhams",
                        principalColumn: "MaSP",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnhSPs_MaSP",
                table: "AnhSPs",
                column: "MaSP");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDonHangs_MaDH",
                table: "ChiTietDonHangs",
                column: "MaDH");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDonHangs_MaSP",
                table: "ChiTietDonHangs",
                column: "MaSP");

            migrationBuilder.CreateIndex(
                name: "IX_DanhGias_MaKH",
                table: "DanhGias",
                column: "MaKH");

            migrationBuilder.CreateIndex(
                name: "IX_DanhGias_MaSP",
                table: "DanhGias",
                column: "MaSP");

            migrationBuilder.CreateIndex(
                name: "IX_DonHangs_MaKH",
                table: "DonHangs",
                column: "MaKH");

            migrationBuilder.CreateIndex(
                name: "IX_DonHangs_MaNV",
                table: "DonHangs",
                column: "MaNV");

            migrationBuilder.CreateIndex(
                name: "IX_DonHangs_MaVoucher",
                table: "DonHangs",
                column: "MaVoucher");

            migrationBuilder.CreateIndex(
                name: "IX_SanPhams_MaLoai",
                table: "SanPhams",
                column: "MaLoai");

            migrationBuilder.CreateIndex(
                name: "IX_SanPhams_MaNCC",
                table: "SanPhams",
                column: "MaNCC");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnhSPs");

            migrationBuilder.DropTable(
                name: "ChiTietDonHangs");

            migrationBuilder.DropTable(
                name: "DanhGias");

            migrationBuilder.DropTable(
                name: "DonHangs");

            migrationBuilder.DropTable(
                name: "SanPhams");

            migrationBuilder.DropTable(
                name: "KhachHangs");

            migrationBuilder.DropTable(
                name: "NhanViens");

            migrationBuilder.DropTable(
                name: "Voucher");

            migrationBuilder.DropTable(
                name: "LoaiSPs");

            migrationBuilder.DropTable(
                name: "NhaCungCap");
        }
    }
}
