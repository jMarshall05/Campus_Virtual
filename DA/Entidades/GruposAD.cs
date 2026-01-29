using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DA.Entidades
{
    [Table("grupos")]
    public class GruposAD
    {

        [Key]
        [Column("id_grupo")]
        public int id_grupo { get; set; }
        [Column("nombre_grupo")]
        public string nombre_grupo { get; set; }
        [Column("descripcion")]
        public string descripcion { get; set; }
        [Column("creado_por")]
        public string creado_por { get; set; }
        [Column("estado")]
        public bool estado { get; set; }
        [Column("FechaDeCreacion")]
        public DateTime FechaDeCreacion { get; set; }
        [Column("FechaDeModificacion")]
        public DateTime? FechaDeModificacion { get; set; }

        [Column("modificado_por")]
        public string modificado_por { get; set; }

    }
}
