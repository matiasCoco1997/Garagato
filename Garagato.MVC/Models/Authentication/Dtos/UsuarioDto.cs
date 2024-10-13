using System.ComponentModel.DataAnnotations;

namespace Garagato.MVC.Models.Authentication.Dtos
{
    public class UsuarioDto
    {

       
        public long Id { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
        public string Name { get; set; }

        [Required(ErrorMessage = "El correo electronico es obligatorio")]
        [EmailAddress(ErrorMessage = "El formato del correo electronico no es valido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
        public string Password { get; set; }

    }
}
