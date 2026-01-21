using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Modelos.ModelosDto
{
    public class UsuariosDto
    {
        [Key]
        [DisplayName("Id de Usuario")]
        public string IdUsuario { get; set; }
        [DisplayName("Nombre")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Nombre { get; set; }
        [DisplayName("Apellido")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Apellido { get; set; }
        [DisplayName("Email")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
        public string Email { get; set; }
        [DisplayName("Teléfonos")]
        public List<TelefonoDto> Telefonos { get; set; } = new List<TelefonoDto>();
        [DisplayName("Fecha de Nacimiento")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public DateTime FechaDeNacimiento { get; set; }
        [DisplayName("Identificacion")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Identificacion { get; set; }
        [DisplayName("Fecha de Registro")]
        public DateTime FechaDeRegistro { get; set; }
        [DisplayName("Fecha de Modificación")]
        public DateTime? FechaDeModificacion { get; set; }
        [DisplayName("Rol")]
        public string Rol { get; set; }

        [DisplayName("Tipo de Identificacion")]
        public string TipoIdentificacion { get; set; }
        [DisplayName("Estado")]
        public bool Estado { get; set; }

        

    }
}
