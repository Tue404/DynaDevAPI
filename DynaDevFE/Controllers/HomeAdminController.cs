using Microsoft.AspNetCore.Mvc;

namespace DynaDevFE.Controllers
{
    public class HomeAdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
