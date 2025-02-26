using DynaDevAPI.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DynaDevAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DanhMucController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DanhMucController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        
        public IActionResult GetDanhMuc()
        {
            var danhMucs = _context.LoaiSPs.Select(dm => new
            {

                dm.MaLoai,
                dm.TenLoai,
                dm.AnhLoai,


            }).ToList();

            return Ok(danhMucs);
        }



        [HttpGet("Search")]
        public IActionResult SearchProducts([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest(new { message = "Từ khóa tìm kiếm không hợp lệ." });
            }

            var results = _context.SanPhams
                .Where(p => p.TenSanPham.Contains(query) || p.MoTa.Contains(query)) 
                .ToList();

            if (!results.Any())
            {
                return NotFound(new { message = "Không tìm thấy sản phẩm phù hợp." });
            }

            return Ok(results);
        }



    }


}
