using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Garagato.MVC.Controllers
{
    public class LoginController : Controller
    {
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

            //simulacion de autenticacion, se deberia usar un servicio de bd y otro aparte para la logic de auth
            if (username == "admin" && password == "123") {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes("m1#7h1sIsA-Sup3rSgaragato3cr3tK3yF0rJWT@1234");
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new System.Security.Claims.ClaimsIdentity(new Claim[] {
                        new Claim(ClaimTypes.Name,username),
                        new Claim(ClaimTypes.Role, "Admin")
                    }),
                    Expires = DateTime.UtcNow.AddHours(1),
                    Issuer = "localhost:7258",
                    Audience = "localhost:7258",
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                return Redirect("/Home");
            }

            return View("/Login/Index");
        }
    }
}
