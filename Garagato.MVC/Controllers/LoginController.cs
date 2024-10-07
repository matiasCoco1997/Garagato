using Microsoft.AspNetCore.Mvc;

namespace Garagato.MVC.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login() {
            return View("/Home");
        }
    }
}
