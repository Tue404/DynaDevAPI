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
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNhaCungCap(string id)
        {
            var nhaCungCap = await _db.NhaCungCaps.FindAsync(id);
            if (nhaCungCap == null)
            {
                return NotFound("Nhà cung cấp không tồn tại.");
            }

            _db.NhaCungCaps.Remove(nhaCungCap);
            await _db.SaveChangesAsync();

            return NoContent(); // Trả về trạng thái 204 No Content
        }


    }


}
