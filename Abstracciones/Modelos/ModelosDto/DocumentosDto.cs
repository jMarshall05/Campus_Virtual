using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace Abstracciones.Modelos.ModelosDto
{
    public class DocumentosDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El título es obligatorio.")]
        [StringLength(200)]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [StringLength(500)]
        public string Descripcion { get; set; }

        [StringLength(500)]
        public string RutaArchivo { get; set; }

        [Required]
        [StringLength(100)]
        public string Categoria { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        public virtual IFormFile? Doc {  get; set; }
    }
}
