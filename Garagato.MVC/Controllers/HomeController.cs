using Garagato.Logica;
using Garagato.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Garagato.MVC.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISalaServicio _salaService;
        private readonly IUsuarioServicio _usuarioService;

        public HomeController(ILogger<HomeController> logger, ISalaServicio salaServicio, IUsuarioServicio usuarioServicio)
        {
            _logger = logger;
            _salaService = salaServicio;
            _usuarioService = usuarioServicio;
        }

        public IActionResult Index()
        {
            var token = Request.Cookies["AuthToken"];

            if (token != null)
            {
                ViewBag.usuarioLogueado = _usuarioService.ObtenerUsuarioLogueado(token);

                var salasActivas = _salaService.ObtenerSalas();

                if (salasActivas != null)
                {
                    ViewBag.salasActivas = salasActivas;
                }
            }

            return View();
        }

        [HttpGet]
        public IActionResult Logout()
        {
            if (Request.Cookies["AuthToken"] != null)
            {
                // Eliminar la cookie del token JWT
                Response.Cookies.Delete("AuthToken");
                return RedirectToAction("Index", "Login");
            }

            return RedirectToAction("Index", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
