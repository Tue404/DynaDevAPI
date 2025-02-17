using DynaDevAPI.Data;
using DynaDevAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DynaDevAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoaiSPController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LoaiSPController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Lấy danh sách danh mục sản phẩm
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoaiSP>>> GetLoaiSPs()
        {
            return await _context.LoaiSPs.ToListAsync();
        }

        // Lấy thông tin một danh mục theo ID
        [HttpGet("{id}")]
        public async Task<ActionResult<LoaiSP>> GetLoaiSP(string id)
        {
            var loaiSP = await _context.LoaiSPs.FirstOrDefaultAsync(l => l.MaLoai == id);

            if (loaiSP == null)
            {
                return NotFound("Không tìm thấy danh mục.");
            }

            return loaiSP;
        }

        // Thêm danh mục sản phẩm
        // POST: api/LoaiSP
        [HttpPost]
        public async Task<ActionResult<LoaiSP>> PostLoaiSP(LoaiSP loaiSP)
        {
            if (_context.LoaiSPs.Any(l => l.MaLoai == loaiSP.MaLoai))
            {
                return Conflict("Mã danh mục đã tồn tại.");
            }

            _context.LoaiSPs.Add(loaiSP);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetLoaiSP), new { id = loaiSP.MaLoai }, loaiSP);
        }

        // PUT: api/LoaiSP/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLoaiSP(string id, LoaiSP loaiSP)
        {
            if (id != loaiSP.MaLoai)
            {
                return BadRequest("Mã danh mục không khớp.");
            }

            _context.Entry(loaiSP).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LoaiSPExists(id))
                {
                    return NotFound("Danh mục không tồn tại.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        // Xóa danh mục sản phẩm
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoaiSP(string id)
        {
            var loaiSP = await _context.LoaiSPs.FindAsync(id);
            if (loaiSP == null)
            {
                return NotFound();
            }

            _context.LoaiSPs.Remove(loaiSP);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LoaiSPExists(string id)
        {
            return _context.LoaiSPs.Any(e => e.MaLoai == id);
        }

        

    }
}
