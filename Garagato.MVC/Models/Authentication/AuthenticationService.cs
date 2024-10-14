using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Garagato.Entidades;

namespace Garagato.MVC.Models
{

    public interface IAuthenticationService {
        public string GenerateToken(Usuario user);
    }

    public class AuthenticationService : IAuthenticationService
    {

        private readonly IConfiguration _configuration;

        public AuthenticationService() {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsetings.json", optional: false, reloadOnChange: true);
        }

        public string GenerateToken(Usuario user) {

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["SecretKey"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                 new Claim(ClaimTypes.Name, user.Name)
                }),          
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _configuration["Issuer"],
                Audience = _configuration["Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }

    }
}
