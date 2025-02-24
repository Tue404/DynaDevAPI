using DynaDevAPI.Data;
using DynaDevAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DynaDevAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public CustomerController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: api/Customer?page={page}&pageSize={pageSize}
        [HttpGet]
        public async Task<ActionResult> GetKhachHangs([FromQuery] int page = 1, [FromQuery] int pageSize = 8)
        {
            try
            {
                if (page <= 0 || pageSize <= 0)
                {
                    return BadRequest(new { message = "Page và PageSize phải lớn hơn 0." });
                }

                var totalCustomers = await _db.KhachHangs.CountAsync();
                var totalPages = (int)Math.Ceiling(totalCustomers / (double)pageSize);

                if (page > totalPages)
                {
                    return NotFound(new { message = "Trang không tồn tại." });
                }

                var customers = await _db.KhachHangs
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return Ok(new
                {
                    data = customers.Select(kh => new
                    {
                        kh.MaKH,
                        kh.TenKH,
                        kh.Email,
                        kh.SDT,
                        kh.DiaChi,
                        kh.TinhTrang,
                        kh.NgayDangKy
                    }),
                    pagination = new
                    {
                        currentPage = page,
                        pageSize = pageSize,
                        totalItems = totalCustomers,
                        totalPages = totalPages
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi lấy danh sách khách hàng.", error = ex.Message });
            }
        }

        // GET: api/Customer/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<KhachHang>> GetKhachHang(string id)
        {
            try
            {
                var khachHang = await _db.KhachHangs.FindAsync(id);

                if (khachHang == null)
                {
                    return NotFound(new { message = "Không tìm thấy khách hàng." });
                }

                return Ok(new
                {
                    khachHang.MaKH,
                    khachHang.TenKH,
                    khachHang.Email,
                    khachHang.SDT,
                    khachHang.DiaChi,
                    khachHang.TinhTrang,
                    khachHang.NgayDangKy
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi lấy thông tin khách hàng.", error = ex.Message });
            }
        }

        // GET: api/Customer/Search?query={query}
        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<KhachHang>>> SearchKhachHang([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest(new { message = "Từ khóa tìm kiếm không hợp lệ." });
            }

            try
            {
                // Chuyển query về dạng chữ thường để tìm kiếm không phân biệt hoa thường
                var lowercaseQuery = query.ToLower();

                // Tìm kiếm trong các trường: Mã KH, Tên KH, Email, SDT, Địa chỉ
                var results = await _db.KhachHangs
                    .Where(kh =>
                        kh.MaKH.ToLower().Contains(lowercaseQuery) ||
                        kh.TenKH.ToLower().Contains(lowercaseQuery) ||
                        kh.Email.ToLower().Contains(lowercaseQuery) ||
                        kh.SDT.ToLower().Contains(query) ||
                        kh.DiaChi.ToLower().Contains(lowercaseQuery))
                    .ToListAsync();

                // Nếu không có kết quả
                if (results == null || !results.Any())
                {
                    return NotFound(new { message = "Không tìm thấy khách hàng nào phù hợp." });
                }

                return Ok(results.Select(kh => new
                {
                    kh.MaKH,
                    kh.TenKH,
                    kh.Email,
                    kh.SDT,
                    kh.DiaChi,
                    kh.TinhTrang,
                    kh.NgayDangKy
                }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi tìm kiếm khách hàng.", error = ex.Message });
            }
        }

        // POST: api/Customer
        [HttpPost]
        public async Task<ActionResult> AddKhachHang([FromBody] KhachHang khachHang)
        {
            if (khachHang == null)
            {
                return BadRequest(new { message = "Dữ liệu khách hàng không hợp lệ." });
            }

            try
            {
                // Tạo mã khách hàng nếu không có
                if (string.IsNullOrWhiteSpace(khachHang.MaKH))
                {
                    khachHang.MaKH = $"KH{new Random().Next(100, 999)}";
                }

                // Gán ngày đăng ký nếu không có
                khachHang.NgayDangKy = DateTime.Now;



                await _db.KhachHangs.AddAsync(khachHang);
                await _db.SaveChangesAsync();

                return CreatedAtAction(nameof(GetKhachHang), new { id = khachHang.MaKH }, new { message = "Khách hàng đã được thêm thành công!", data = khachHang });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi thêm khách hàng.", error = ex.Message });
            }
        }

        // PUT: api/Customer/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateKhachHang(string id, [FromBody] KhachHang khachHang)
        {
            // Kiểm tra dữ liệu đầu vào
            if (khachHang == null || id != khachHang.MaKH)
            {
                return BadRequest(new { message = "Dữ liệu khách hàng không hợp lệ hoặc mã khách hàng không khớp." });
            }

            try
            {
                // Tìm khách hàng trong cơ sở dữ liệu
                var existingCustomer = await _db.KhachHangs.FindAsync(id);

                if (existingCustomer == null)
                {
                    return NotFound(new { message = "Không tìm thấy khách hàng." });
                }

                // Cập nhật thông tin khách hàng
                existingCustomer.TenKH = khachHang.TenKH;
                existingCustomer.Email = khachHang.Email;
                existingCustomer.MatKhau = khachHang.MatKhau;
                existingCustomer.SDT = khachHang.SDT;
                existingCustomer.DiaChi = khachHang.DiaChi;
                existingCustomer.TinhTrang = khachHang.TinhTrang;

                // Giữ nguyên các trường không thay đổi
                existingCustomer.NgayDangKy = existingCustomer.NgayDangKy;



                _db.Entry(existingCustomer).State = EntityState.Modified;

                // Lưu thay đổi
                await _db.SaveChangesAsync();

                return Ok(new { message = "Cập nhật khách hàng thành công!", data = existingCustomer });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi cập nhật khách hàng.", error = ex.Message });
            }
        }


        // DELETE: api/Customer/{id}
        [HttpDelete("{MaKH}")]
        public async Task<IActionResult> Delete(string MaKH)
        {
            try
            {
                var khachHang = await _db.KhachHangs.FindAsync(MaKH);

                if (khachHang == null)
                {
                    return NotFound(new { message = "Không tìm thấy khách hàng." });
                }

                _db.KhachHangs.Remove(khachHang);
                await _db.SaveChangesAsync();

                return Ok(new { message = "Xóa khách hàng thành công!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi xóa khách hàng.", error = ex.Message });
            }
        }
    }
}
