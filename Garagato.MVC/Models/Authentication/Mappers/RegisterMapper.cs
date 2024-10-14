using Garagato.Entidades;
using Garagato.MVC.Models.Authentication.Dto;
using System.Security.Cryptography;
using System.Text;

namespace Garagato.MVC.Models.Authentication.Mappers
{
    public class RegisterMapper
    {
        // Mapea de UsuarioDto a Usuario (para crear o actualizar un usuario)
        public static Usuario ToEntity(UsuarioDto dto)
        {
            if (dto == null) return null;

            return new Usuario
            {
                Name = dto.Name,
                Email = dto.Email,
                Password = HashPassword(dto.Password)  // Opcional: hash de la contraseña
            };
        }

        // Mapea de Usuario a UsuarioDto (por ejemplo, para mostrar un usuario en la vista)
        public static UsuarioDto ToDto(Usuario entity)
        {
            if (entity == null) return null;

            return new UsuarioDto
            {
                Name = entity.Name,
                Email = entity.Email,
                // La contraseña no se incluye al devolver el DTO
            };
        }

        // Método para hash de contraseñas (puedes usar la librería que prefieras)
        private static string HashPassword(string password)
        {
            // Aquí deberías implementar o usar una librería para el hashing
            // Esto es solo un ejemplo simplificado
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

    }
}
