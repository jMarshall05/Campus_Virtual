using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.Modelos.ModelosDto;

namespace Abstracciones.Modelos.Requests
{
    public class EditarUsuarioRequest
    {
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Apellido { get; set; }
        public List<TelefonoDto> Telefonos { get; set; }

    }
}
