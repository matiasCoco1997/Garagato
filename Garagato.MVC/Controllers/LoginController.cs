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
using System.Security.Claims;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace Garagato.MVC.Controllers
{
    public class loginController : Controller
    {
        private readonly IUsuarioServicio _usuarioService;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        public loginController(IUsuarioServicio usuarioService, IConfiguration configuration, HttpClient httpClient)
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


        [HttpGet]
        public IActionResult Inicio()
        {
            var redirectUrl = Url.Action("LoQueReciboDeGoogle", "Login");
            var properties = new AuthenticationProperties
            {
                RedirectUri = redirectUrl 
            };
            return new ChallengeResult("Google", properties);
        }

        [HttpGet]
        public async Task<IActionResult> LoQueReciboDeGoogle()
        {
            var result = await HttpContext.AuthenticateAsync("Google");
            if (result?.Principal != null)
            {
                var claims = result.Principal.Identities
                    .FirstOrDefault()?
                    .Claims.Select(claim => new
                    {
                        claim.Type,
                        claim.Value
                    });

                var email = result.Principal.FindFirst(ClaimTypes.Email)?.Value;

                Response.Cookies.Append("AuthToken", "token-generado", new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict
                });

                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Login");
        }
    }
}
