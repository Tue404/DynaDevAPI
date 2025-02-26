using DynaDevAPI.Data;
using DynaDevAPI.Models;
using DynaDevAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DynaDevAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TTCNController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public TTCNController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        // GET: api/TTCN/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<KhachHang>> GetKhachHang(string id)
        {
            try
            {
                var khachHang = await _dbContext.KhachHangs.FindAsync(id);

                if (khachHang == null)
                {
                    return NotFound(new { message = "Không tìm thấy khách hàng." });
                }

                return Ok(new KhachHang
                {
                    MaKH = khachHang.MaKH,
                    TenKH = khachHang.TenKH,
                    Email = khachHang.Email,
                    MatKhau = khachHang.MatKhau,
                    SDT = khachHang.SDT,
                    DiaChi = khachHang.DiaChi,
                    TinhTrang = khachHang.TinhTrang
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi lấy thông tin khách hàng.", error = ex.Message });
            }
        }

        // PUT: api/TTCN/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateKhachHang(string id, [FromBody] KhachHang model)
        {
            if (model == null || string.IsNullOrEmpty(model.MaKH))
            {
                return BadRequest(new { message = "Dữ liệu không hợp lệ." });
            }

            try
            {
                var existingCustomer = await _dbContext.KhachHangs.FindAsync(model.MaKH);
                if (existingCustomer == null)
                {
                    return NotFound(new { message = "Không tìm thấy khách hàng." });
                }

                // Cập nhật thông tin
                existingCustomer.TenKH = model.TenKH;
                existingCustomer.Email = model.Email;
                existingCustomer.SDT = model.SDT;
                existingCustomer.DiaChi = model.DiaChi;

                // Chỉ cập nhật mật khẩu nếu có thay đổi
                if (!string.IsNullOrEmpty(model.MatKhau))
                {
                    existingCustomer.MatKhau = model.MatKhau; // Mật khẩu mới
                }

                _dbContext.Entry(existingCustomer).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();

                return Ok(new { message = "Cập nhật thông tin thành công!", data = existingCustomer });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi cập nhật thông tin.", error = ex.Message });
            }
        }
    }
}
