using Garagato.Data.EF;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text;

namespace Garagato.Logica
{

    public interface IUsuarioServicio
    {
        List<Usuario> ObtenerUsuarios();
        void AgregarUsuario(Usuario usuario);

        Usuario ObtenerUsuarioLogueado(string token);

        Usuario ObtenerUsuarioPorId(string id); // Cambiar int a string
        void ActualizarUsuario(Usuario usuario);
        Task RegistrarUsuarioAsync(string nombre, string email, string contraseña);
        Task<bool> ExisteUsuarioAsync(string nombre, string mail);
        Task<Usuario> ValidarUsuarioAsync(string nombre, string contrasena);
        Task<string> GenerarTokenAsync(Usuario usuario);
    }
    public class UsuarioServicio : IUsuarioServicio
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly GaragatoDatabaseContext _context;

        public UsuarioServicio(GaragatoDatabaseContext context, UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        public List<Usuario> ObtenerUsuarios()
        {
            return _context.Usuarios.ToList();
        }

        public void AgregarUsuario(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();
        }

        public Usuario ObtenerUsuarioPorId(string id) // Cambiar int a string
        {
            return _context.Usuarios.Find(id); // Cambiar el método Find para usar string
        }

        public void ActualizarUsuario(Usuario usuario)
        {
            _context.Usuarios.Update(usuario);
            _context.SaveChanges();
        }

        public async Task<bool> ExisteUsuarioAsync(string nombre, string mail)
        {
            return await _context.Usuarios.AnyAsync(u => u.Nombre == nombre || u.Mail == mail);
        }

        public async Task RegistrarUsuarioAsync(string nombre, string email, string contrasena)
        {
            // Validar que la contraseña no sea nula o vacía
            if (string.IsNullOrWhiteSpace(contrasena))
            {
                throw new ArgumentException("La contraseña no puede ser nula o vacía.", nameof(contrasena));
            }

            // Crear el objeto Usuario
            var usuario = new Usuario
            {
                UserName = nombre,
                Email = email
                // Asegúrate de no incluir la propiedad Contrasena aquí
            };

            // Crear el usuario usando UserManager
            var result = await _userManager.CreateAsync(usuario, contrasena);

            // Verificar si la creación fue exitosa
            if (result.Succeeded)
            {
                // Puedes realizar acciones adicionales si es necesario
            }
            else
            {
                // Lanzar una excepción con los errores
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }

        public async Task<Usuario> ValidarUsuarioAsync(string nombre, string contrasena)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Nombre == nombre && u.Contrasena == contrasena);
        }

        public async Task<string> GenerarTokenAsync(Usuario usuario)
        {
            var claims = new[]
            {
            new Claim("UserId", usuario.Id), // Cambiar a usuario.Id
            new Claim(JwtRegisteredClaimNames.Sub, usuario.Nombre),
            new Claim(JwtRegisteredClaimNames.Email, usuario.Mail),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345superSecretKey@345"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "http://localhost:7258",
                audience: "http://localhost:7258",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public Usuario ObtenerUsuarioLogueado(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "UserId");

            if (userIdClaim != null)
            {
                var userId = userIdClaim.Value; // Cambiar a string directamente
                var usuarioLogueado = this.ObtenerUsuarioPorId(userId); // Usar el nuevo método
                return usuarioLogueado;
            }
            return null;
        }
    }
}
