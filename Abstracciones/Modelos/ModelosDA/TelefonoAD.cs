using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Abstracciones.Modelos.ModelosDA
{
    [Table("Telefonos")]
    public class TelefonoAD
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }
        [Column("Id_Usuario")]
        public string IdUsuario { get; set; }
        [Column("Codigo_area")]
        public int Codigo { get; set; }
        [Column("Telefono")]
        public int Telefono { get; set; }
        [Column("Tipo")]
        public string Tipo { get; set; }
        [Column("Estado")]
        public bool Estado { get; set; } = true;
    }
}
