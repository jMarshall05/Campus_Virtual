using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Abstracciones.Modelos.ModelosDA
{
    [Table("Entregas")] // nombre real de la tabla
    public class EntregasAD
    {
        [Key]
        [Column("id_entrega")]
        public int IdEntrega { get; set; }

        [Column("id_tarea")]
        public int IdTarea { get; set; }

        [Column("id_estudiante")]
        public string IdEstudiante { get; set; }

        [Column("archivo_entregado")]
        public string ArchivoEntregado { get; set; }

        [Column("fecha_entrega")]
        public DateTime FechaEntrega { get; set; }

        [Column("estado")]
        public bool Estado { get; set; }
    }
}