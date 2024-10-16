using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Garagato.MVC.Controllers;

[Authorize]
public class SalaController : Controller
{
    public IActionResult Juego()
    {
        return View("Index");
    }
}
