using Garagato.Data.EF;
using Microsoft.EntityFrameworkCore;

namespace Garagato.Logica
{

    public interface IUsuarioServicio
    {
        List<Usuario> ObtenerUsuarios();
        void AgregarUsuario(Usuario usuario);
        Usuario ObtenerUsuarioPorId(int id);
        void ActualizarUsuario(Usuario usuario);
        Task RegistrarUsuarioAsync(string nombre, string email, string contraseña);
        Task<bool> ExisteUsuarioAsync(string nombre, string mail);
        Task<Usuario> ValidarUsuarioAsync(string nombre, string contrasena);
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

        public async Task RegistrarUsuarioAsync(string nombre, string email, string contraseña)
        {
            var usuario = new Usuario
            {
                Nombre = nombre,
                Mail = email,
                Contrasena = contraseña 
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task<Usuario> ValidarUsuarioAsync(string nombre, string contrasena)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Nombre == nombre && u.Contrasena == contrasena);
        }
    }
}
