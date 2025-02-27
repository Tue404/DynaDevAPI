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
        [HttpGet("OrderHistory/{MaKH}")]
        public async Task<ActionResult<IEnumerable<object>>> GetOrderHistory(string MaKH)
        {
            var orders = await _db.DonHangs
                .Where(d => d.MaKH == MaKH)  // Lọc theo mã khách hàng
                .Include(d => d.PaymentStatus)     // Bao gồm thông tin trạng thái thanh toán
                .Include(d => d.OrderStatus)       // Bao gồm thông tin trạng thái đơn hàng
                .Select(d => new
                {
                    d.MaDH,
                    PaymentStatus = d.PaymentStatus.Name,
                    OrderStatus = d.OrderStatus.Name,
                    d.DiaChiNhanHang,   // Địa chỉ giao hàng
                    d.ThoiGianDatHang,
                    d.TongTien,
                    /*d.DateChanged,*/      // Ngày thay đổi (có thể là thời gian thay đổi trạng thái hoặc thời gian cập nhật)
                   /* d.ChangedBy,  */      // Người thay đổi
                    Products = d.ChiTietDonHangs.Select(c => new
                    {
                        c.SanPham.TenSanPham,
                        c.SoLuong,
                        c.Gia,
                        Total = c.SoLuong * c.Gia
                    })
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
