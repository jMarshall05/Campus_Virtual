using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DA.Entidades
{
    [Table("Documentos")]
    public class DocumentosAD
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }
        [Column("Titulo")]
        [Required(ErrorMessage = "El título es obligatorio.")]
        [StringLength(200)]
        public string Titulo { get; set; }
        [Column("Descripcion")]
        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [StringLength(500)]
        public string Descripcion { get; set; }
        [Column("RutaArchivo")]
        [StringLength(500)]
        public string RutaArchivo { get; set; }

        [Column("Categoria")]
        [Required]
        [StringLength(100)]
        public string Categoria { get; set; }

        [Column("FechaRegistro")]
        [DataType(DataType.DateTime)]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;


    }
}
