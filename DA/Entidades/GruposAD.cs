using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DA.Entidades
{
    [Table("grupos")]
    public class GruposAD
    {

        [Key]
        [Column("id_grupo")]
        public int idGrupo { get; set; }
        [Column("nombre_grupo")]
        public string Nombre { get; set; }
        [Column("descripcion")]
        public string Descripcion { get; set; }
        [Column("creado_por")]
        public string? creado_por { get; set; }
        [Column("estado")]
        public bool Estado { get; set; }
        [Column("FechaDeCreacion")]
        public DateTime FechaDeCreacion { get; set; }
        [Column("FechaDeModificacion")]
        public DateTime? FechaDeModificacion { get; set; }

        [Column("modificado_por")]
        public string? modificado_por { get; set; }
        public virtual ICollection<EstudianteGrupoAD> EstudianteGrupos { get; set; }
    }
}
