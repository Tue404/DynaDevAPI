using DynaDevAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DynaDevAPI.Data;

namespace DynaDevAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoucherController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public VoucherController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: api/voucher (with pagination)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Voucher>>> GetAllVouchers(int pageNumber = 1, int pageSize = 8)
        {
            try
            {
                // Remove expired vouchers
                var expiredVouchers = await _db.Vouchers
                    .Where(v => v.NgayKetThuc.Date <= DateTime.Now.Date)
                    .ToListAsync();

                // Xóa voucher hết hạn nếu có
                if (expiredVouchers.Any())
                {
                    _db.Vouchers.RemoveRange(expiredVouchers);
                    await _db.SaveChangesAsync();  // Lưu các thay đổi vào DB
                }

                // Lấy danh sách voucher phân trang
                var vouchers = await _db.Vouchers
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                if (!vouchers.Any())
                {
                    return NotFound(new { message = "Không tìm thấy voucher nào." });
                }

                // Lấy tổng số lượng voucher cho metadata phân trang
                var totalVouchers = await _db.Vouchers.CountAsync();
                var totalPages = (int)Math.Ceiling((double)totalVouchers / pageSize);

                var result = new
                {
                    TotalVouchers = totalVouchers,
                    TotalPages = totalPages,
                    CurrentPage = pageNumber,
                    PageSize = pageSize,
                    Vouchers = vouchers
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                // In ra thông báo lỗi chi tiết
                Console.WriteLine(ex.InnerException?.Message);
                return StatusCode(500, new { message = "Lỗi khi lấy danh sách voucher...", error = ex.Message });
            }
        }


        // GET: api/voucher/{id} (Get a single voucher by id)
        [HttpGet("{id}")]
        public async Task<ActionResult<Voucher>> GetVoucher(string id)
        {
            try
            {
                var voucher = await _db.Vouchers.FindAsync(id);

                if (voucher == null)
                {
                    return NotFound(new { message = "Không tìm thấy voucher." });
                }

                return Ok(voucher);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi lấy thông tin voucher.", error = ex.Message });
            }
        }

        // POST: api/voucher (Add a new voucher)
        [HttpPost]
        public async Task<IActionResult> AddVoucher([FromBody] Voucher voucher)
        {
            if (voucher == null)
            {
                return BadRequest(new { message = "Dữ liệu voucher không hợp lệ." });
            }

            try
            {
                voucher.MaVoucher = $"VC{new Random().Next(100, 999)}";

                await _db.Vouchers.AddAsync(voucher);
                await _db.SaveChangesAsync();

                return CreatedAtAction(nameof(GetVoucher), new { id = voucher.MaVoucher }, new { message = "Voucher đã được thêm thành công!", data = voucher });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi thêm voucher.", error = ex.Message });
            }
        }

        // PUT: api/voucher/{id} (Update a voucher by id)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVoucher(string id, [FromBody] Voucher voucher)
        {
            if (voucher == null || id != voucher.MaVoucher)
            {
                return BadRequest(new { message = "Dữ liệu voucher không hợp lệ hoặc mã voucher không khớp." });
            }

            try
            {
                var expiredVouchers = await _db.Vouchers
                    .Where(v => v.NgayKetThuc <= DateTime.Now)
                    .ToListAsync();

                if (expiredVouchers.Any())
                {
                    _db.Vouchers.RemoveRange(expiredVouchers);
                    await _db.SaveChangesAsync();
                }

                var existingVoucher = await _db.Vouchers.FindAsync(id);

                if (existingVoucher == null)
                {
                    return NotFound(new { message = "Không tìm thấy voucher." });
                }

                existingVoucher.TenVoucher = voucher.TenVoucher;
                existingVoucher.MoTa = voucher.MoTa;
                existingVoucher.GiamGia = voucher.GiamGia;
                existingVoucher.LoaiGiamGia = voucher.LoaiGiamGia;
                existingVoucher.NgayBatDau = voucher.NgayBatDau;
                existingVoucher.NgayKetThuc = voucher.NgayKetThuc;
                existingVoucher.DieuKien = voucher.DieuKien;
                existingVoucher.SoLuong = voucher.SoLuong;
                existingVoucher.TrangThai = voucher.TrangThai;

                _db.Entry(existingVoucher).State = EntityState.Modified;

                await _db.SaveChangesAsync();

                return Ok(new { message = "Cập nhật voucher thành công!", data = existingVoucher });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi cập nhật voucher.", error = ex.Message });
            }
        }

        // DELETE: api/voucher/{id} (Delete a voucher by id)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVoucher(string id)
        {
            try
            {
                var voucher = await _db.Vouchers.FindAsync(id);

                if (voucher == null)
                {
                    return NotFound(new { message = "Không tìm thấy voucher." });
                }

                _db.Vouchers.Remove(voucher);
                await _db.SaveChangesAsync();

                return Ok(new { message = "Xóa voucher thành công!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi xóa voucher.", error = ex.Message });
            }
        }

        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<Voucher>>> SearchVoucher([FromQuery] string query, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 8)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest(new { message = "Từ khóa tìm kiếm không hợp lệ." });
            }

            try
            {
                var lowercaseQuery = query.ToLower();

                var results = await _db.Vouchers
                    .Where(v =>
                        v.MaVoucher.ToLower().Contains(lowercaseQuery) ||
                        v.TenVoucher.ToLower().Contains(lowercaseQuery) ||
                        v.SoLuong.ToString().Contains(lowercaseQuery) ||
                        v.TrangThai.ToString().Contains(lowercaseQuery) ||
                        v.DieuKien.ToString().Contains(lowercaseQuery) ||
                        v.LoaiGiamGia.ToString().Contains(lowercaseQuery) ||
                        v.MoTa.ToLower().Contains(lowercaseQuery))
                    .ToListAsync();

                // Phân trang
                var paginatedResults = results.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                if (!paginatedResults.Any())
                {
                    return NotFound(new { message = "Không tìm thấy voucher nào phù hợp." });
                }

                return Ok(new
                {
                    vouchers = paginatedResults,
                    totalPages = (int)Math.Ceiling(results.Count / (double)pageSize)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi tìm kiếm voucher.", error = ex.Message });
            }
        }


    }
}
