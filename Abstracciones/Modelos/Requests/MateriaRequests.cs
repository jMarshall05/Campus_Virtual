using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Modelos.Requests
{
    public class MateriaRequests
    {
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Nombre { get; set; }
    }
}
