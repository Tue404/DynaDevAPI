using DynaDevAPI.Data;
using DynaDevAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DynaDevAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public OrderController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: api/Order
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetAllDonHangs()
        {
            var donHangs = await _db.DonHangs
                .Include(d => d.PaymentStatus)
                .Include(d => d.OrderStatus)
                .Select(d => new
                {
                    d.MaDH,
                    d.MaKH,
                    PaymentStatus = d.PaymentStatus.Name,
                    OrderStatus = d.OrderStatus.Name,
                    d.DiaChiNhanHang,
                    d.ThoiGianDatHang,
                    d.TongTien,
                    MaNV = d.NhanVien != null ? d.NhanVien.MaNV : null
                })
                .ToListAsync();

            return Ok(donHangs);
        }

        [HttpPut("ChangeOrderStatus/{id}")]
        public async Task<IActionResult> ChangeOrderStatus(string id, [FromBody] int newStatusId)
        {
            var order = await _db.DonHangs.FindAsync(id);

            if (order == null)
            {
                return NotFound(new { message = "Không tìm thấy đơn hàng." });
            }

            // Kiểm tra trạng thái hợp lệ
            var validStatus = await _db.OrderStatuses.AnyAsync(s => s.Id == newStatusId);
            if (!validStatus)
            {
                return BadRequest(new { message = "Trạng thái đơn hàng không hợp lệ." });
            }

            // Cập nhật trạng thái đơn hàng
            order.OrderStatusId = newStatusId;

            try
            {
                await _db.SaveChangesAsync();
                return Ok(new { message = "Cập nhật trạng thái đơn hàng thành công!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi cập nhật.", error = ex.Message });
            }
        }


        // PUT: api/Order/ChangePaymentStatus/{id}
        [HttpPut("ChangePaymentStatus/{id}")]
        public async Task<IActionResult> ChangePaymentStatus(string id, [FromBody] int newPaymentStatusId)
        {
            var order = await _db.DonHangs.FindAsync(id);

            if (order == null)
            {
                return NotFound(new { message = "Không tìm thấy đơn hàng." });
            }

            // Kiểm tra trạng thái thanh toán hợp lệ
            var isValidPaymentStatus = await _db.PaymentStatuses.AnyAsync(s => s.Id == newPaymentStatusId);
            if (!isValidPaymentStatus)
            {
                return BadRequest(new { message = "Trạng thái thanh toán không hợp lệ." });
            }

            order.PaymentStatusId = newPaymentStatusId;

            try
            {
                await _db.SaveChangesAsync();
                return Ok(new { message = "Cập nhật trạng thái thanh toán thành công!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi cập nhật.", error = ex.Message });
            }
        }


        // DELETE: api/Order/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(string id)
        {
            var order = await _db.DonHangs.FindAsync(id);

            if (order == null)
            {
                return NotFound(new { message = "Không tìm thấy đơn hàng." });
            }

            _db.DonHangs.Remove(order);

            try
            {
                await _db.SaveChangesAsync();
                return Ok(new { message = "Xóa đơn hàng thành công!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi xóa.", error = ex.Message });
            }
        }
    }
}