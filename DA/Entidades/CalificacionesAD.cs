using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DA.Entidades
{
    [Table("Calificaciones")] // nombre real de la tabla
    public class CalificacionesAD
    {
        [Key]
        [Column("id_calificacion")]
        public int IdCalificacion { get; set; }

        [ForeignKey("Entrega")]
        [Column("id_entrega")]
        public int IdEntrega { get; set; }

        [Column("calificacion")]
        public decimal Calificacion { get; set; }

        [Column("comentario")]
        public string Comentario { get; set; }

        [Column("fecha_calificacion")]
        public DateTime FechaCalificacion { get; set; }

        public virtual EntregasAD Entrega { get; set; }
        [Column("estado")]
        public bool Estado { get; set; }
    }
}
