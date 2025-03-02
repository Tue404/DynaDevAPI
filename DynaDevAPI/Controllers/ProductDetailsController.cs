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
                DaBan = sanPham.DaBan,
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

        [HttpPost("AddReview")]
        public async Task<IActionResult> AddReview([FromBody] DanhGiaDto danhGiaDto)
        {
            if (danhGiaDto == null || danhGiaDto.DiemDanhGia < 1 || danhGiaDto.DiemDanhGia > 5)
            {
                return BadRequest(new { success = false, message = "Điểm đánh giá phải từ 1 đến 5." });
            }

            string maKH = danhGiaDto.MaKH;
            if (string.IsNullOrEmpty(maKH))
            {
                return Unauthorized(new { success = false, message = "MaKH không hợp lệ." });
            }

            var khachHang = await _db.KhachHangs.FirstOrDefaultAsync(kh => kh.MaKH == maKH);
            if (khachHang == null)
            {
                return BadRequest(new { success = false, message = "Khách hàng không tồn tại." });
            }

            var sanPham = await _db.SanPhams.FindAsync(danhGiaDto.MaSP);
            if (sanPham == null)
            {
                return BadRequest(new { success = false, message = "Sản phẩm không tồn tại." });
            }

            danhGiaDto.MaDanhGia = $"DG{new Random().Next(100, 999)}";

            // Danh sách từ ngữ không phù hợp
            List<string> tuCam = new List<string> { "xấu", "tệ", "vô dụng", "kém chất lượng",
        "lồn", "cặc", "địt", "chịch", "buồi", "đụ", "đéo", "điếm",
        "bitch", "fuck", "dick", "pussy", "asshole", "motherfucker",
        "cu", "chó chết", "dâm", "ngu", "vl", "dm", "clgt", "vcl",
        "phò", "đĩ", "khốn nạn", "con mẹ mày", "đéo mẹ", "nứng", "tổ sư",
        "mẹ kiếp", "bố mày", "liếm lồn", "liếm cặc", "óc chó", "cave", "đm",
        "wtf", "fucking", "shit", "cum", "slut", "whore", "tits", "boobs",
        "rape", "jerk", "suck", "balls", "blowjob", "handjob", "faggot",
        "gay", "lesbian", "dildo", "vagina", "penis", "anus", "scum", "bastard",
        "đĩ mẹ", "đụ mẹ", "đụ cha", "đụ bà", "mẹ cha", "đù", "fuck you",
        "đéo hiểu", "mẹ nó", "fuck off", "cút", "get lost", "piss off",
        "liếm buồi", "bú cặc", "bú lồn", "bố láo", "chó má", "súc vật",
        "mất dạy", "khốn", "khốn kiếp", "mẹ kiếp", "thằng khốn", "con khốn",
        "dickhead", "cunt", "shithead", "piss", "pissing", "screw you",
        "goddamn", "son of a bitch", "sonofabitch", "dirty", "mothafucka",
        "jackass", "douchebag", "retard", "fuckface", "cock", "shitbag",
        "fuckwit", "fuckstick", "arsehole", "tosser", "bloody hell",
        "cuntface", "ballsack", "fucker", "dickhead", "bitchface",
        "ho", "cumdumpster", "dickwad", "twat", "shitfaced", "cockface",
        "gobshite", "bollocks", "minger", "arse", "knobhead", "twatwaffle",
        "dumbfuck", "shitcunt", "cumslut", "wanker", "prick", "fucknugget",
        "fuckhead", "dickweasel", "cockmongler", "dickfucker", "shitweasel",
        "fucksocks", "fucksponge", "fuckbiscuit", "fuckbucket", "cumguzzler",
        "cockjockey", "shitbrick", "cumbucket", "fucktard", "dicknose",
        "shitstain", "craphole", "fuckpile", "shitstick", "fuckbunny",
        "fuckrag", "fuckknuckle", "shitsmear", "cocksucker", "cocksplat" };
            bool isBadComment = tuCam.Any(tu => danhGiaDto.BinhLuan.ToLower().Contains(tu));


            var danhGia = new DanhGia
            {
                MaDanhGia = danhGiaDto.MaDanhGia,
                MaSP = danhGiaDto.MaSP,
                MaKH = maKH,
                DiemDanhGia = danhGiaDto.DiemDanhGia,
                BinhLuan = danhGiaDto.BinhLuan ?? "",
                NgayDanhGia = DateTime.UtcNow,
                TrangThai = isBadComment ? "Chưa Duyệt" : "Đã Duyệt"
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
