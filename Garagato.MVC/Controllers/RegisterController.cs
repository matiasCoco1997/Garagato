using Garagato.Logica;
using Garagato.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace Garagato.MVC.Controllers
{
    public class RegisterController : Controller
    {
        private readonly IUsuarioServicio _usuarioService;

        public RegisterController(IUsuarioServicio usuarioService)
        {
            _usuarioService = usuarioService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registrar(RegistroViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Contrasena != model.RepetirContrasena)
                {
                    ModelState.AddModelError(string.Empty, "Las contraseñas no coinciden.");
                    return View("Index", model);
                }

                if (await _usuarioService.ExisteUsuarioAsync(model.Nombre, model.Mail))
                {
                    ModelState.AddModelError(string.Empty, "El nombre de usuario o el correo electrónico ya existen.");
                    return View("Index", model);
                }

                await _usuarioService.RegistrarUsuarioAsync(model.Nombre, model.Mail, model.Contrasena);
                return RedirectToAction("Index", "Login");
            }

            return View("Index", model);
        }
    }
}
