using Microsoft.AspNetCore.Mvc;

namespace Garagato.MVC.Controllers;

public class SalaController : Controller
{
    public IActionResult Juego()
    {
        return View("Index");
    }
}
