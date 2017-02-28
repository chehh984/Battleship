using Microsoft.AspNetCore.Mvc;

namespace Battleship.Controllers
{
    public class HomeController : Controller
    {
        public HomeController() { }

        public IActionResult Index()
        {
            return View();
        }
    }
}