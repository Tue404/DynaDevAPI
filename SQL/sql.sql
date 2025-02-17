USE [DynaDev]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 1/16/2025 1:01:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AnhSPs]    Script Date: 1/16/2025 1:01:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AnhSPs](
	[MaAnh] [nvarchar](450) NOT NULL,
	[MaSP] [nvarchar](450) NOT NULL,
	[TenAnh] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_AnhSPs] PRIMARY KEY CLUSTERED 
(
	[MaAnh] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ChiTietDonHangs]    Script Date: 1/16/2025 1:01:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChiTietDonHangs](
	[MaChiTiet] [nvarchar](450) NOT NULL,
	[MaDH] [nvarchar](450) NOT NULL,
	[MaSP] [nvarchar](450) NOT NULL,
	[SoLuong] [int] NOT NULL,
	[Gia] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_ChiTietDonHangs] PRIMARY KEY CLUSTERED 
(
	[MaChiTiet] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DanhGias]    Script Date: 1/16/2025 1:01:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DanhGias](
	[MaDanhGia] [nvarchar](450) NOT NULL,
	[MaSP] [nvarchar](450) NOT NULL,
	[MaKH] [nvarchar](450) NOT NULL,
	[DiemDanhGia] [int] NOT NULL,
	[BinhLuan] [nvarchar](max) NOT NULL,
	[NgayDanhGia] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_DanhGias] PRIMARY KEY CLUSTERED 
(
	[MaDanhGia] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DonHangs]    Script Date: 1/16/2025 1:01:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DonHangs](
	[MaDH] [nvarchar](450) NOT NULL,
	[MaKH] [nvarchar](450) NOT NULL,
	[ThongTinThanhToan] [nvarchar](max) NOT NULL,
	[DiaChiNhanHang] [nvarchar](max) NOT NULL,
	[ThoiGianDatHang] [datetime2](7) NOT NULL,
	[TongTien] [decimal](18, 2) NOT NULL,
	[TinhTrang] [nvarchar](max) NOT NULL,
	[MaNV] [nvarchar](450) NULL,
 CONSTRAINT [PK_DonHangs] PRIMARY KEY CLUSTERED 
(
	[MaDH] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[KhachHangs]    Script Date: 1/16/2025 1:01:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KhachHangs](
	[MaKH] [nvarchar](450) NOT NULL,
	[TenKH] [nvarchar](max) NOT NULL,
	[Email] [nvarchar](max) NOT NULL,
	[MatKhau] [nvarchar](max) NOT NULL,
	[SDT] [nvarchar](max) NOT NULL,
	[DiaChi] [nvarchar](max) NOT NULL,
	[TinhTrang] [nvarchar](max) NOT NULL,
	[NgayDangKy] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_KhachHangs] PRIMARY KEY CLUSTERED 
(
	[MaKH] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LoaiSPs]    Script Date: 1/16/2025 1:01:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LoaiSPs](
	[MaLoai] [nvarchar](450) NOT NULL,
	[TenLoai] [nvarchar](max) NOT NULL,
	[AnhLoai] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_LoaiSPs] PRIMARY KEY CLUSTERED 
(
	[MaLoai] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NhanViens]    Script Date: 1/16/2025 1:01:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NhanViens](
	[MaNV] [nvarchar](450) NOT NULL,
	[TenNV] [nvarchar](max) NOT NULL,
	[Email] [nvarchar](max) NOT NULL,
	[MatKhau] [nvarchar](max) NOT NULL,
	[SDT] [nvarchar](max) NOT NULL,
	[DiaChi] [nvarchar](max) NOT NULL,
	[TinhTrang] [nvarchar](max) NOT NULL,
	[NgayVaoLam] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_NhanViens] PRIMARY KEY CLUSTERED 
(
	[MaNV] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SanPhams]    Script Date: 1/16/2025 1:01:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SanPhams](
	[MaSP] [nvarchar](450) NOT NULL,
	[MaLoai] [nvarchar](450) NOT NULL,
	[TenSanPham] [nvarchar](100) NOT NULL,
	[Gia] [decimal](18, 2) NOT NULL,
	[MoTa] [nvarchar](500) NOT NULL,
	[SoLuongTrongKho] [int] NOT NULL,
	[NgayThem] [datetime2](7) NOT NULL,
	[TinhTrang] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK_SanPhams] PRIMARY KEY CLUSTERED 
(
	[MaSP] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AnhSPs]  WITH CHECK ADD  CONSTRAINT [FK_AnhSPs_SanPhams_MaSP] FOREIGN KEY([MaSP])
REFERENCES [dbo].[SanPhams] ([MaSP])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AnhSPs] CHECK CONSTRAINT [FK_AnhSPs_SanPhams_MaSP]
GO
ALTER TABLE [dbo].[ChiTietDonHangs]  WITH CHECK ADD  CONSTRAINT [FK_ChiTietDonHangs_DonHangs_MaDH] FOREIGN KEY([MaDH])
REFERENCES [dbo].[DonHangs] ([MaDH])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ChiTietDonHangs] CHECK CONSTRAINT [FK_ChiTietDonHangs_DonHangs_MaDH]
GO
ALTER TABLE [dbo].[ChiTietDonHangs]  WITH CHECK ADD  CONSTRAINT [FK_ChiTietDonHangs_SanPhams_MaSP] FOREIGN KEY([MaSP])
REFERENCES [dbo].[SanPhams] ([MaSP])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ChiTietDonHangs] CHECK CONSTRAINT [FK_ChiTietDonHangs_SanPhams_MaSP]
GO
ALTER TABLE [dbo].[DanhGias]  WITH CHECK ADD  CONSTRAINT [FK_DanhGias_KhachHangs_MaKH] FOREIGN KEY([MaKH])
REFERENCES [dbo].[KhachHangs] ([MaKH])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DanhGias] CHECK CONSTRAINT [FK_DanhGias_KhachHangs_MaKH]
GO
ALTER TABLE [dbo].[DanhGias]  WITH CHECK ADD  CONSTRAINT [FK_DanhGias_SanPhams_MaSP] FOREIGN KEY([MaSP])
REFERENCES [dbo].[SanPhams] ([MaSP])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DanhGias] CHECK CONSTRAINT [FK_DanhGias_SanPhams_MaSP]
GO
ALTER TABLE [dbo].[DonHangs]  WITH CHECK ADD  CONSTRAINT [FK_DonHangs_KhachHangs_MaKH] FOREIGN KEY([MaKH])
REFERENCES [dbo].[KhachHangs] ([MaKH])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DonHangs] CHECK CONSTRAINT [FK_DonHangs_KhachHangs_MaKH]
GO
ALTER TABLE [dbo].[DonHangs]  WITH CHECK ADD  CONSTRAINT [FK_DonHangs_NhanViens_MaNV] FOREIGN KEY([MaNV])
REFERENCES [dbo].[NhanViens] ([MaNV])
GO
ALTER TABLE [dbo].[DonHangs] CHECK CONSTRAINT [FK_DonHangs_NhanViens_MaNV]
GO
ALTER TABLE [dbo].[SanPhams]  WITH CHECK ADD  CONSTRAINT [FK_SanPhams_LoaiSPs_MaLoai] FOREIGN KEY([MaLoai])
REFERENCES [dbo].[LoaiSPs] ([MaLoai])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SanPhams] CHECK CONSTRAINT [FK_SanPhams_LoaiSPs_MaLoai]
GO
