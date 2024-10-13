using Garagato.MVC.Models.Authentication.Dtos;
using Garagato.Entidades;

namespace Garagato.MVC.Models.Authentication.Provider
{
    public static class UsuarioMapper {
        public static Usuario ToEntity(UsuarioDto usuarioDto) {
            return new Usuario
            {
                Name = usuarioDto.Name,
                Email = usuarioDto.Email,
                Password = usuarioDto.Password
            };
        }
    }
}
