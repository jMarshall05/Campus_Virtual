using System;
using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Modelos.ModelosDto
{
    public class GruposDto
    {
        [Key]
        [Display(Name = "ID de Grupo")]
        public int id_grupo { get; set; }
        [Display(Name = "Nombre del Grupo")]
        [Required(ErrorMessage = "El nombre del grupo es obligatorio.")]
        public string nombre_grupo { get; set; }
        [Display(Name = "Descripción del Grupo")]
        [Required(ErrorMessage = "La descripción del grupo es obligatoria.")]
        public string descripcion { get; set; }
        [Display(Name = "Creador")]
        public string? creado_por { get; set; }
        [Display(Name = "Estado")]
        public bool estado { get; set; }
        [Display(Name = "Fecha de Creación")]
        [Required(ErrorMessage = "La fecha de creación es obligatoria.")]
        public DateTime FechaDeCreacion { get; set; }
        [Display(Name = "Fecha de Modificación")]
        public DateTime? FechaDeModificacion { get; set; }

        [Display(Name = "Modificado por")]
        public string? modificado_por { get; set; }

    }
}
