using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DA.Entidades
{
    [Table("Bitacora")]
    public class BitacoraAD
    {
        [Key]
        [Column("IdBitacora")]
        public string IdBitacora { get; set; }

        [Column("Accion")]
        public string accion { get; set; }
        [Column("Descripcion")]
        public string descripcion { get; set; }
        [Column("Fecha")]
        public DateTime Fecha { get; set; }
        [Column("Usuario")]
        public string usuario { get; set; }
        [Column("Tabla")]
        public string Tabla { get; set; }
    }
}
