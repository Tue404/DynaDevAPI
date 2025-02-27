using DynaDevAPI.Data;
using DynaDevAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DynaDevAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderHistoryController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public OrderHistoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: api/OrderHistory/{customerId}
        [HttpGet("{MaKH}")]
        public async Task<ActionResult<IEnumerable<object>>> GetOrderHistory(string MaKH)
        {
            var orders = await _db.DonHangs
     .Where(d => d.MaKH == MaKH)
     .Select(d => new
     {
         d.MaDH,
         PaymentStatus = d.PaymentStatus != null ? d.PaymentStatus.Name : "Chưa cập nhật",
         OrderStatus = d.OrderStatus != null ? d.OrderStatus.Name : "Chưa cập nhật",
         d.DiaChiNhanHang,
         d.ThoiGianDatHang,
         d.TongTien,
         Products = d.ChiTietDonHangs.Select(c => new
         {
             c.SanPham.TenSanPham,
             c.SoLuong,
             c.Gia,
             Total = c.SoLuong * c.Gia
         }).ToList()
     })
     .ToListAsync();


            if (orders == null || !orders.Any())
            {
                return NotFound(new { message = "Không tìm thấy đơn hàng nào." });
            }

            return Ok(orders);
        }

    }
}
