using System.ComponentModel.DataAnnotations;
using Abstracciones.Modelos.ModelosDto;

namespace Abstracciones.Modelos.Requests
{
    public class UsuariosRequests
    {
        public class EditarUsuarioRequest
        {
            [Required(ErrorMessage = "El campo {0} es obligatorio.")]
            public string Nombre { get; set; }
            [Required(ErrorMessage = "El campo {0} es obligatorio.")]
            public string Apellido { get; set; }
            public List<TelefonoDto> Telefonos { get; set; }


        }
        public class EditarUsuarioAdminRequest
        {
            [Required]
            public string Nombre { get; set; }
            [Required]
            public string Apellido { get; set; }
            [Required]
            [EmailAddress]
            public string Email { get; set; }
            [Required]
            public string Identificacion { get; set; }
            [Required]
            public string TipoIdentificacion { get; set; }
            [Required]
            public string Rol { get; set; }

            [Required]
            public DateTime FechaDeNacimiento { get; set; }
            [Required]
            public bool Estado { get; set; }
            public List<TelefonoDto> Telefonos { get; set; }

        }
        public class RegisterRequest
        {
            [Required]
            public string Nombre { get; set; }
            [Required]
            public string Apellido { get; set; }
            [Required]
            [EmailAddress]
            public string Email { get; set; }
            [Required]
            public string Identificacion { get; set; }
            [Required]
            public string TipoIdentificacion { get; set; }
            [Required]
            public string Rol { get; set; }
            [Required]
            public DateTime FechaDeNacimiento { get; set; }
            public List<TelefonoDto> Telefonos { get; set; }
            [Required]
            [MinLength(12)]
            public string Password { get; set; }

            [Compare("Password")]
            public string ConfirmPassword { get; set; }
        }
    }
}
