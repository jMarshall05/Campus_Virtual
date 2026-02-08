using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Modelos.ModelosDto
{
    public class GruposDto
    {
        [Key]
        public int id_grupo { get; set; }

        [Required(ErrorMessage = "El nombre del grupo es obligatorio.")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "La descripción del grupo es obligatoria.")]
        public string Descripcion { get; set; }
        public string? creado_por { get; set; }
        public bool estado { get; set; } = true;
        [Required(ErrorMessage = "La fecha de creación es obligatoria.")]
        public DateTime FechaDeCreacion { get; set; }
        public DateTime? FechaDeModificacion { get; set; }
        public string? modificado_por { get; set; }

    }
}
