using Microsoft.AspNetCore.Mvc;

namespace DynaDevFE.Controllers
{
    public class DanhMucController : Controller
    {

        public IActionResult Index(string maLoai)
        {
            ViewBag.MaLoai = maLoai;
            return View();
        }
    }
}
