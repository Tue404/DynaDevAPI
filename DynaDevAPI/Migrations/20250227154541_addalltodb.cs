using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DynaDevAPI.Migrations
{
    /// <inheritdoc />
    public partial class addalltodb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KhachHangs",
                columns: table => new
                {
                    MaKH = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenKH = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MatKhau = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SDT = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TinhTrang = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    AnhLoai = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoaiSPs", x => x.MaLoai);
                });

            migrationBuilder.CreateTable(
                name: "NhaCungCaps",
                columns: table => new
                {
                    MaNCC = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    TenNCC = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SDT = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiaChi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TinhTrang = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhaCungCaps", x => x.MaNCC);
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
                name: "OrderStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vouchers",
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
                    table.PrimaryKey("PK_Vouchers", x => x.MaVoucher);
                });

            migrationBuilder.CreateTable(
                name: "SanPhams",
                columns: table => new
                {
                    MaSP = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaLoai = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenSanPham = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TacGia = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NhaXuatBan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NamXuatBan = table.Column<int>(type: "int", nullable: false),
                    Gia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    SoLuongTrongKho = table.Column<int>(type: "int", nullable: false),
                    NgayThem = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TinhTrang = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaNCC = table.Column<string>(type: "nvarchar(5)", nullable: false)
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
                        name: "FK_SanPhams_NhaCungCaps_MaNCC",
                        column: x => x.MaNCC,
                        principalTable: "NhaCungCaps",
                        principalColumn: "MaNCC",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DonHangs",
                columns: table => new
                {
                    MaDH = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaKH = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaVoucher = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TenNguoiNhan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SoDienThoai = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiaChiNhanHang = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhuongThucThanhToan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThoiGianDatHang = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TongTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentStatusId = table.Column<int>(type: "int", nullable: false),
                    OrderStatusId = table.Column<int>(type: "int", nullable: false),
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
                        name: "FK_DonHangs_OrderStatuses_OrderStatusId",
                        column: x => x.OrderStatusId,
                        principalTable: "OrderStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DonHangs_PaymentStatuses_PaymentStatusId",
                        column: x => x.PaymentStatusId,
                        principalTable: "PaymentStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DonHangs_Vouchers_MaVoucher",
                        column: x => x.MaVoucher,
                        principalTable: "Vouchers",
                        principalColumn: "MaVoucher");
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
                    TrangThai = table.Column<string>(type: "nvarchar(max)", nullable: true),
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

            migrationBuilder.InsertData(
                table: "KhachHangs",
                columns: new[] { "MaKH", "DiaChi", "Email", "MatKhau", "NgayDangKy", "SDT", "TenKH", "TinhTrang" },
                values: new object[] { "KH01", "Hà Nội", "vana@gmail.com", "123456", new DateTime(2025, 2, 27, 22, 45, 40, 711, DateTimeKind.Local).AddTicks(5351), "0123456789", "Nguyễn Văn A", "Hoạt động" });

            migrationBuilder.InsertData(
                table: "LoaiSPs",
                columns: new[] { "MaLoai", "AnhLoai", "MoTa", "TenLoai" },
                values: new object[,]
                {
                    { "1", null, "Danh mục sách và truyện tranh", "Sách - Truyện Tranh" },
                    { "2", null, "Dụng cụ văn phòng phẩm", "Dụng Cụ Vẽ - VPP" },
                    { "3", null, "Băng đĩa và phụ kiện số", "Băng Đĩa - Phụ Kiện Số" }
                });

            migrationBuilder.InsertData(
                table: "NhanViens",
                columns: new[] { "MaNV", "DiaChi", "Email", "Luong", "MatKhau", "NgayVaoLam", "SDT", "TenNV", "TinhTrang" },
                values: new object[] { "NV01", "TP.HCM", "vanb@gmail.com", 0f, "admin123", new DateTime(2023, 2, 27, 22, 45, 40, 712, DateTimeKind.Local).AddTicks(3573), "0987654321", "Trần Văn B", "Đang làm việc" });

            migrationBuilder.InsertData(
                table: "OrderStatuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Chưa xác nhận" },
                    { 2, "Đã xác nhận" },
                    { 3, "Đang chuẩn bị hàng" },
                    { 4, "Đang giao hàng" },
                    { 5, "Đã giao hàng" },
                    { 6, "Đã hủy" }
                });

            migrationBuilder.InsertData(
                table: "PaymentStatuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Chưa thanh toán" },
                    { 2, "Đã thanh toán" }
                });

            migrationBuilder.InsertData(
                table: "DonHangs",
                columns: new[] { "MaDH", "DiaChiNhanHang", "MaKH", "MaNV", "MaVoucher", "OrderStatusId", "PaymentStatusId", "PhuongThucThanhToan", "SoDienThoai", "TenNguoiNhan", "ThoiGianDatHang", "TongTien" },
                values: new object[,]
                {
                    { "DH001", "123 Đường Văn Học, Hà Nội", "KH01", "NV01", null, 1, 1, null, null, null, new DateTime(2025, 2, 24, 22, 45, 40, 712, DateTimeKind.Local).AddTicks(4285), 240000m },
                    { "DH002", "456 Đường Khoa Học, TP.HCM", "KH01", "NV01", null, 3, 2, null, null, null, new DateTime(2025, 2, 25, 22, 45, 40, 712, DateTimeKind.Local).AddTicks(4796), 90000m }
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
                name: "IX_DonHangs_OrderStatusId",
                table: "DonHangs",
                column: "OrderStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_DonHangs_PaymentStatusId",
                table: "DonHangs",
                column: "PaymentStatusId");

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
                name: "OrderStatuses");

            migrationBuilder.DropTable(
                name: "PaymentStatuses");

            migrationBuilder.DropTable(
                name: "Vouchers");

            migrationBuilder.DropTable(
                name: "LoaiSPs");

            migrationBuilder.DropTable(
                name: "NhaCungCaps");
        }
    }
}
