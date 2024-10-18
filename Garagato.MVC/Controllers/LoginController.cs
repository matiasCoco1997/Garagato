using Garagato.Logica;
using Garagato.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;

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
            if (Request.Cookies["AuthToken"] != null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View("Bienvenida");
        }

        public IActionResult Index()
        {
            if (Request.Cookies["AuthToken"] != null)
            {
                return RedirectToAction("Index", "Home");
            }

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
                    var token = await _usuarioService.GenerarTokenAsync(usuario);

                    Response.Cookies.Append("AuthToken", token, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict
                    });

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
