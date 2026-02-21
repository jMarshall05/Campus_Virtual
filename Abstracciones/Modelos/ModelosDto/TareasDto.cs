using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace Abstracciones.Modelos.ModelosDto
{
    public class TareaDto
    {
        [Key]
        [DisplayName("ID de Tarea")]
        public int IdTarea { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [DisplayName("Título")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [DisplayName("Descripción")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [DisplayName("Fecha de Entrega")]
        public DateTime FechaEntrega { get; set; }

        public int IdMateria { get; set; }

        [DisplayName("Archivo Adjunto")]
        public string? ArchivoAdjunto { get; set; }

        [DisplayName("Fecha de Modificación")]
        public DateTime? FechaModificacion { get; set; }

        [DisplayName("Fecha de Publicación")]
        public DateTime FechaPublicacion { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un grupo.")]
        [DisplayName("Grupo")]
        public int idGrupo { get; set; }

        [ForeignKey("id_grupo")]
        public virtual GruposDto Grupo { get; set; }

        [NotMapped]
        public virtual IFormFile Archivo { get; set; }

        [NotMapped]
        [DisplayName("Grupo Asignado")]
        public string Nombre_grupo { get; set; } // Cambiado de NombreGrupo a nombre_grupo

        public CalificacionesDto Calificacion { get; set; }
        [DisplayName("Asignado Por")]
        public string asignado_por { get; set; }
        [DisplayName("Estado")]
        public bool Estado { get; set; } = true;
    }
}
