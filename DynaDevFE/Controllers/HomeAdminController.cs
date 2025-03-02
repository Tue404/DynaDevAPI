using System.Net.Http;
using System.Text;
using System.Text.Json;
using DynaDevAPI.Models;
using DynaDevFE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DynaDevFE.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class HomeAdminController : Controller
    {
        public async Task<IActionResult> Index()
        {
           return View();
        }
    }
}
