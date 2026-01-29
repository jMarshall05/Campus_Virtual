using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abstracciones.Modelos.ModelosDto;

namespace DA.Entidades
{
    [Table("Telefonos")]
    public class TelefonoAD
    {
        [Key]
        [Column("Id")]
        public int? Id { get; set; }
        [Column("Id_Usuario")]
        public string IdUsuario { get; set; }
        [Column("Codigo_area")]
        public int Codigo { get; set; }
        [Column("Telefono")]
        public long Telefono { get; set; }
        [Column("Tipo")]
        public string Tipo { get; set; }
        [Column("Estado")]
        public bool Estado { get; set; } = true;
        [ForeignKey(nameof(IdUsuario))]
        public virtual UsuariosAD Usuario { get; set; }
    }
}
