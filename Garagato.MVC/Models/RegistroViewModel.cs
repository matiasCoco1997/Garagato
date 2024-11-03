using System.ComponentModel.DataAnnotations;

namespace Garagato.MVC.Models
{
    public class RegistroViewModel
    {
        [Required(ErrorMessage = "El campo Nombre de usuario es requerido.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo Email es requerido.")]
        public string Mail { get; set; }

        [Required(ErrorMessage = "El campo Contraseña es requerido.")]
        public string Contrasena { get; set; }

        [Required(ErrorMessage = "El campo Repetir contraseña es requerido.")]
        public string RepetirContrasena { get; set; }
    }
}
