using Garagato.Entidades;
using Garagato.MVC.Models.Authentication.Dtos;
using Garagato.MVC.Models.Authentication.Provider;
using Garagato.MVC.Models.Authentication;
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

       

        [HttpPost("Register")]
        public IActionResult Register(UsuarioDto usuarioDto) {
            if (ModelState.IsValid) {
                if (_context.Usuarios.Any(us => us.Name == usuarioDto.Name || us.Email == usuarioDto.Email)) {
                    ModelState.AddModelError("", "El nombre de usuario o el correo electronico ya están en uso");
                    return View(usuarioDto);
                }

                var usuario = UsuarioMapper.ToEntity(usuarioDto);

                _context.Usuarios.Add(usuario);
                _context.SaveChanges();

                return RedirectToAction("Bienvenida");
            }

            return View(usuarioDto);
        }
    }
}
