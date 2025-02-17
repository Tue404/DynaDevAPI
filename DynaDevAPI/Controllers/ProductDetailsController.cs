using DynaDevAPI.Data;
using DynaDevAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSanPham(string id)
        {
            var sanPham = await _db.SanPhams
                                   .Include(sp => sp.AnhSPs)
                                   .Include(sp => sp.NhaCungCap)
                                   .FirstOrDefaultAsync(sp => sp.MaSP == id);

            if (sanPham == null)
            {
                return NotFound("Không tìm thấy sản phẩm.");
            }

            // Tạo DTO trả về
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
                DanhSachAnh = sanPham.AnhSPs.Select(a => a.TenAnh).ToList()
            };

            return Ok(sanPhamDto);
        }

    }
}
