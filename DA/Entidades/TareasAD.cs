using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DA.Entidades
{
    [Table("tareas")]
    public class TareasAD
    {
        [Key]
        [Column("id_tarea")]
        public int IdTarea { get; set; }

        [Column("id_materia")]
        [Required]
        public int IdMateria { get; set; }

        [ForeignKey(nameof(IdMateria))]
        public virtual MateriasAD Materia { get; set; }

        [Column("titulo")]
        [Required]
        [StringLength(150)]
        public string Titulo { get; set; }

        [Column("descripcion")]
        [Required]
        public string? Descripcion { get; set; }

        [Column("fecha_entrega")]
        [Required]
        public DateTime FechaEntrega { get; set; }

        [Column("archivo_adjunto")]
        public string? ArchivoAdjunto { get; set; }

        [Column("fecha_modificacion")]
        public DateTime? FechaModificacion { get; set; }

        [Column("FechaPublicacion")]
        public DateTime FechaPublicacion { get; set; }
        [Column("IdGrupo")]
        public int IdGrupo { get; set; }

        [ForeignKey(nameof(IdGrupo))]
        public virtual GruposAD Grupo { get; set; }

        [Column("asignado_por")]
        public string asignado_por { get; set; }

        [ForeignKey(nameof(asignado_por))]
        public virtual UsuariosAD Usuario { get; set; }
        [Column("estado")]
        public bool Estado { get; set; }

    }
}