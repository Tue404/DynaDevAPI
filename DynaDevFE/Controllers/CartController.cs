using DynaDevFE.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace DynaDevFE.Controllers
{
    public class CartController : Controller
    {
        [HttpPost]
        public IActionResult UpdateCart([FromBody] List<CartViewModel> cartItems)
        {
            TempData["Cart"] = JsonSerializer.Serialize(cartItems);
            return Ok();
        }
        public IActionResult Index()
        {
            var cartData = TempData["Cart"] as string;
            var cartItems = string.IsNullOrEmpty(cartData) ? new List<CartViewModel>() : JsonSerializer.Deserialize<List<CartViewModel>>(cartData);
            return View(cartItems);
        }
    }
}
