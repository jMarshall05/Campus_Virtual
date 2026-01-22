using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.Modelos.ModelosDto;

namespace Abstracciones.Modelos.Requests
{
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
        public List<TelefonoDto> Telefonos { get; set; }

        [Required]
        public DateTime FechaDeNacimiento { get; set; }

        [Required]
        public string Identificacion { get; set; }

        [Required]
        [MinLength(12)]
        public string Password { get; set; }

        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Rol { get; set; }

        [Required]
        public string TipoIdentificacion { get; set; }
    }

}
