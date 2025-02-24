using DynaDevAPI.Data;
using DynaDevAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DynaDevAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliersController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public SuppliersController(ApplicationDbContext db)
        {
            _db = db;
        }

        // Lấy tất cả nhà cung cấp
        [HttpGet]
        public async Task<IActionResult> GetAllNhaCungCap()
        {
            var nhaCungCaps = await _db.NhaCungCaps.ToListAsync();
            return Ok(nhaCungCaps);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNCCById(string id)
        {
            var nhanVien = await _db.NhaCungCaps.FindAsync(id);

            if (nhanVien == null)
            {
                return NotFound($"Không tìm thấy nhân viên với mã {id}.");
            }

            return Ok(nhanVien);
        }

        // Thêm mới nhà cung cấp
        [HttpPost]
        public async Task<IActionResult> CreateSupplier([FromBody] NhaCungCap supplier)
        {
            if (supplier == null)
            {
                return BadRequest("Dữ liệu nhà cung cấp không hợp lệ.");
            }

            _db.NhaCungCaps.Add(supplier);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAllNhaCungCap), new { id = supplier.MaNCC }, supplier);
        }


        // Cập nhật thông tin nhà cung cấp
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNhaCungCap(string id, NhaCungCap nhaCungCap)
        {
            if (id != nhaCungCap.MaNCC)
            {
                return BadRequest("Mã nhà cung cấp không khớp.");
            }

            _db.Entry(nhaCungCap).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NhaCungCapExists(id))
                {
                    return NotFound("Nhà cung cấp không tồn tại.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); // Trả về trạng thái 204 No Content
        }

        // Kiểm tra sự tồn tại của nhà cung cấp
        private bool NhaCungCapExists(string id)
        {
            return _db.NhaCungCaps.Any(e => e.MaNCC == id);
        }


        // Xóa nhà cung cấp
        [HttpDelete("{maNCC}")]
        public IActionResult Delete(string maNCC)
        {
            try
            {
                var ncc = _db.NhaCungCaps.FirstOrDefault(p => p.MaNCC == maNCC);
                if (ncc == null)
                {
                    return NotFound(new { message = "Nhà cung cấp không tồn tại" });
                }

                _db.NhaCungCaps.Remove(ncc);
                _db.SaveChanges();

                return Ok(new { message = "Xóa nhà cung cấp thành công" });
            }
            catch (Exception ex)
            {
                // Trả về lỗi dưới dạng JSON thay vì exception
                return StatusCode(500, new { message = "Đã xảy ra lỗi trong quá trình xóa nhà cung cấp.", error = ex.Message });
            }
        }


    }


}
