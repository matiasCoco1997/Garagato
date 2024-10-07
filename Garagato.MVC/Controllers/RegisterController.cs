using Microsoft.AspNetCore.Mvc;

namespace Garagato.MVC.Controllers
{
    public class RegisterController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
