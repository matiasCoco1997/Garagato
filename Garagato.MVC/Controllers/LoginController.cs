using Garagato.Logica;
using Garagato.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;

namespace Garagato.MVC.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUsuarioServicio _usuarioService;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        public LoginController(IUsuarioServicio usuarioService, IConfiguration configuration, HttpClient httpClient)
        {
            _usuarioService = usuarioService;
            _configuration = configuration;
            _httpClient = httpClient;
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


        public IActionResult Inicio()
        {
            var redirectUrl = Url.Action("RedirectGoogle", "login");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet]
        public async Task<IActionResult> RedirectGoogle()
        {
            var result = await HttpContext.AuthenticateAsync("Google");

            if (result.Succeeded)
            {
               
                return RedirectToAction("Index", "Home");
            }

            
            var errorMessage = result.Failure?.Message ?? "No se recibió ningún resultado";
            Console.WriteLine($"Error en la autenticación de Google: {errorMessage}");

         
            return RedirectToAction("Bienvenida", "Login");
        }
    }
}
