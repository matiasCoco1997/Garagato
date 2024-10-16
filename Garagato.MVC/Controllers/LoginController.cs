using Garagato.Logica;
using Garagato.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace Garagato.MVC.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUsuarioServicio _usuarioService;

        public LoginController(IUsuarioServicio usuarioService)
        {
            _usuarioService = usuarioService;
        }

        public IActionResult Bienvenida()
        {
            return View("Bienvenida");
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var usuario = await _usuarioService.ValidarUsuarioAsync(model.Nombre, model.Contrasena);
                if (usuario != null)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrecto.");
                }
            }

            return View("Index", model);
        }
    }
}
