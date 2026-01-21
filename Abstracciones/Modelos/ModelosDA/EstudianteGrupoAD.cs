using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Abstracciones.Modelos.ModelosDA
{
    [Table("EstudianteGrupo")]
    public class EstudianteGrupoAD
    {
        [Key]
        [Column("IdEstudianteGrupo")]
        public int IdEstudianteGrupo { get; set; }

        [Column("EstudianteId")]
        public string EstudianteId { get; set; }

        [Column("GrupoId")]
        public int GrupoId { get; set; }

        [ForeignKey("GrupoId")]
        public virtual GruposAD Grupo { get; set; }
    }
}