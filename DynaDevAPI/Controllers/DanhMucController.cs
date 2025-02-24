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

    }
}
