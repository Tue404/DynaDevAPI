using DynaDevAPI.Data;
using DynaDevAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Net;

namespace DynaDevAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public CartController(ApplicationDbContext db)
        {
            _db = db;
        }
        private const string CartSessionKey = "Cart";


        // Thêm sản phẩm vào giỏ hàng
        [HttpPost]
        public IActionResult UpdateCart([FromBody] List<CartItemDto> cart)
        {
            if (cart == null || cart.Count == 0)
            {
                return BadRequest("Giỏ hàng trống!");
            }

            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true 
            };
            string cartJson = JsonSerializer.Serialize(cart, options);
            HttpContext.Session.SetString("cart", cartJson);

            return Ok(new { message = "Giỏ hàng đã được cập nhật!", cart });
        }
    }
}
