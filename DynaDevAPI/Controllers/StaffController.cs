using DynaDevAPI.Data;
using DynaDevAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DynaDevAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public StaffController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNhanViens()
        {
            var nhanViens = await _db.NhanViens.ToListAsync();
            return Ok(nhanViens);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNhanVienById(string id)
        {
            var nhanVien = await _db.NhanViens.FindAsync(id);

            if (nhanVien == null)
            {
                return NotFound($"Không tìm thấy nhân viên với mã {id}.");
            }

            return Ok(nhanVien);
        }

        [HttpPost]
        public async Task<IActionResult> AddNhanVien([FromBody] NhanVien model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _db.NhanViens.Add(model);
            await _db.SaveChangesAsync();

            return Ok(model);
        }

       

        [HttpPut("{id}")]
        public async Task<IActionResult> EditNhanVien(string id, NhanVien model)
        {
            if (id != model.MaNV)
            {
                return BadRequest(new { message = "ID không khớp với dữ liệu." });
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var nhanVien = await _db.NhanViens.FindAsync(id);
                    if (nhanVien == null)
                    {
                        return NotFound(new { message = "Không tìm thấy nhân viên." });
                    }

                    nhanVien.TenNV = model.TenNV;
                    nhanVien.Email = model.Email;
                    nhanVien.MatKhau = model.MatKhau;
                    nhanVien.SDT = model.SDT;
                    nhanVien.DiaChi = model.DiaChi;
                    nhanVien.TinhTrang = model.TinhTrang;
                    nhanVien.Luong = model.Luong;

                    _db.Update(nhanVien);
                    await _db.SaveChangesAsync();

                    return Ok(new { message = "Cập nhật thông tin nhân viên thành công!" });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_db.NhanViens.Any(e => e.MaNV == id))
                    {
                        return NotFound(new { message = "Không tìm thấy nhân viên khi cập nhật." });
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return Ok(model);
        }

        [HttpDelete("{maNV}")]
        public IActionResult Delete(string maNV)
        {
            try
            {
                var staff = _db.NhanViens.FirstOrDefault(p => p.MaNV == maNV);
                if (staff == null)
                {
                    return NotFound(new { message = "Nhân viên không tồn tại" });
                }

                _db.NhanViens.Remove(staff);
                _db.SaveChanges();

                return Ok(new { message = "Xóa nhân viên thành công" });
            }
            catch (Exception ex)
            {
                // Trả về lỗi dưới dạng JSON thay vì exception
                return StatusCode(500, new { message = "Đã xảy ra lỗi trong quá trình xóa nhân viên.", error = ex.Message });
            }
        }
    }
}
