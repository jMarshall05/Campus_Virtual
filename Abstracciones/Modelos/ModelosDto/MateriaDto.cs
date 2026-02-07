using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Modelos.ModelosDto
{
    public class MateriaDto
    {
        [Key]
        [DisplayName("ID de Materia")]
        public int IdMateria { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [DisplayName("Nombre")]
        public string Nombre { get; set; }

        [DisplayName("Estado")]
        public bool Estado { get; set; }


    }
}