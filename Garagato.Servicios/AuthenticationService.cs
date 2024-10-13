using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Garagato.MVC;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration;

namespace Garagato.Servicios
{

    interface IAuthenticationService {
        public string GenerateToken(Usuarios user);
    }

    internal class AuthenticationService : IAuthenticationService
    {

        private readonly IConfiguration _configuration;

        public AuthenticationService() {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsetings.json", optional: false, reloadOnChange: true);
        }

        public string GenerateToken(Usuarios user) {

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["SecretKey"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                        new Claim(ClaimTypes.Name,username)),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _configuration["Issuer"],
                Audience = _configuration["Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

        }

    }
}
