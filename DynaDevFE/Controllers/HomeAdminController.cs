using System.Net.Http;
using System.Text;
using System.Text.Json;
using DynaDevAPI.Models;
using DynaDevFE.Models;
using Microsoft.AspNetCore.Mvc;

namespace DynaDevFE.Controllers
{
    public class HomeAdminController : Controller
    {

        public async Task<IActionResult> Index()
        {
           return View();
        }


    }
}
