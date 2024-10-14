using Garagato.Entidades;
using Garagato.MVC.Models;
using Garagato.MVC.Models.DataBase;
using Microsoft.AspNetCore.Mvc;

namespace Garagato.MVC.Controllers
{
    public class LoginController : Controller
    {

        private readonly DataBaseConfig _context;
        private readonly IAuthenticationService _authenticationService;

        public LoginController(DataBaseConfig context, IAuthenticationService authService)
        {
            _context = context;
            _authenticationService = authService;
        }

        public IActionResult Bienvenida()
        {
            return View("Bienvenida");
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("Login")]
        public IActionResult Login(string username, string password) {

            var usuario = _context.Usuarios.SingleOrDefault(u => u.Name == u.Name);

            if (usuario == null || usuario.Password != password) {
                ModelState.AddModelError("", "Usuario o contraseña incorrectos");
                return View("Index");
            }

            var token = _authenticationService.GenerateToken(usuario);

            Response.Cookies.Append("JwtToken", token, new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddHours(1)
            });

            return RedirectToAction("Sala");
        }

    }
}
