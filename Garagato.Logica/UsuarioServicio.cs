using Garagato.Data.EF;
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

        Usuario ObtenerUsuarioPorId(int id);
        void ActualizarUsuario(Usuario usuario);
        Task RegistrarUsuarioAsync(string nombre, string email, string contraseña);
        Task<bool> ExisteUsuarioAsync(string nombre, string mail);
        Task<Usuario> ValidarUsuarioAsync(string nombre, string contrasena);
        Task<string> GenerarTokenAsync(Usuario usuario);
    }
    public class UsuarioServicio : IUsuarioServicio
    {
        private GaragatoDatabaseContext _context;

        public UsuarioServicio(GaragatoDatabaseContext context)
        {
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

        public Usuario ObtenerUsuarioPorId(int id)
        {
            return _context.Usuarios.Find(id);
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
            var usuario = new Usuario
            {
                Nombre = nombre,
                Mail = email,
                Contrasena = contrasena 
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task<Usuario> ValidarUsuarioAsync(string nombre, string contrasena)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Nombre == nombre && u.Contrasena == contrasena);
        }

        public async Task<string> GenerarTokenAsync(Usuario usuario) {

            var claims = new[] {
                new Claim("UserId", usuario.Id.ToString()),
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
                var userId = int.Parse(userIdClaim.Value);
                var usuarioLogueado = this.ObtenerUsuarioPorId(userId);
                return usuarioLogueado;
            }
            return null;
        }
    }
}
