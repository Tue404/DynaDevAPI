using DynaDevAPI.Data;
using DynaDevAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DynaDevAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductDetailsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public ProductDetailsController(ApplicationDbContext db)
        {
            _db = db;
        }

        // Lấy sản phẩm cùng với thông tin đánh giá
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSanPham(string id)
        {
            var sanPham = await _db.SanPhams
                                   .Include(sp => sp.AnhSPs)
                                   .Include(sp => sp.NhaCungCap)
                                   .Include(sp => sp.DanhGias)
                                   .ThenInclude(dg => dg.KhachHang) // Bao gồm thông tin Khách Hàng
                                   .FirstOrDefaultAsync(sp => sp.MaSP == id);

            if (sanPham == null)
            {
                return NotFound(new { success = false, message = "Không tìm thấy sản phẩm." });
            }

            var sanPhamDto = new SanPhamDto
            {
                MaSP = sanPham.MaSP,
                MaLoai = sanPham.MaLoai,
                SoLuongTrongKho = sanPham.SoLuongTrongKho,
                TenSanPham = sanPham.TenSanPham,
                TacGia = sanPham.TacGia,
                TenNCC = sanPham.NhaXuatBan,
                MaNCC = sanPham.MaNCC,
                NamXuatBan = sanPham.NamXuatBan,
                Gia = sanPham.Gia,
                MoTa = sanPham.MoTa,
                TinhTrang = sanPham.TinhTrang,
                DanhSachAnh = sanPham.AnhSPs.Select(a => a.TenAnh).ToList(),
                DanhGiaSanPham = sanPham.DanhGias.Select(d => new DanhGiaDto
                {
                    MaDanhGia = d.MaDanhGia,
                    MaKH = d.MaKH,
                    Email = d.KhachHang.Email, // Thêm Email
                    DiemDanhGia = d.DiemDanhGia,
                    BinhLuan = d.BinhLuan,
                    TrangThai = d.TrangThai,
                    NgayDanhGia = d.NgayDanhGia
                }).ToList()
            };

            return Ok(sanPhamDto);
        }

        // Thêm đánh giá
        [HttpPost("AddReview")]
        public async Task<IActionResult> AddReview([FromBody] DanhGiaDto danhGiaDto)
        {
            if (danhGiaDto == null || danhGiaDto.DiemDanhGia < 1 || danhGiaDto.DiemDanhGia > 5)
            {
                return BadRequest(new { success = false, message = "Điểm đánh giá phải từ 1 đến 5." });
            }

            // Kiểm tra xem MaKH có tồn tại hay không
            string maKH = danhGiaDto.MaKH; // Lấy MaKH từ frontend
            if (string.IsNullOrEmpty(maKH))
            {
                return Unauthorized(new { success = false, message = "MaKH không hợp lệ." });
            }

            var khachHang = await _db.KhachHangs.FirstOrDefaultAsync(kh => kh.MaKH == maKH);
            if (khachHang == null)
            {
                return BadRequest(new { success = false, message = "Khách hàng không tồn tại." });
            }

            // Kiểm tra sản phẩm tồn tại
            var sanPham = await _db.SanPhams.FindAsync(danhGiaDto.MaSP);
            if (sanPham == null)
            {
                return BadRequest(new { success = false, message = "Sản phẩm không tồn tại." });
            }

            // Tạo MaDanhGia nếu chưa có
            danhGiaDto.MaDanhGia = $"DG{new Random().Next(100, 999)}";

            var danhGia = new DanhGia
            {
                MaDanhGia = danhGiaDto.MaDanhGia,
                MaSP = danhGiaDto.MaSP,
                MaKH = maKH, // Lưu MaKH từ Khách Hàng
                DiemDanhGia = danhGiaDto.DiemDanhGia,
                BinhLuan = danhGiaDto.BinhLuan ?? "",
                NgayDanhGia = DateTime.UtcNow,
                TrangThai = "Chưa Duyệt"
            };

            try
            {
                _db.DanhGias.Add(danhGia);
                await _db.SaveChangesAsync();
                return Ok(new { success = true, message = "Đánh giá đã được thêm!", danhGia });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Lỗi khi lưu đánh giá", error = ex.Message });
            }
        }
    }
}
