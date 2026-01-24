using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Abstracciones.Modelos.ModelosDA
{
    [Table("Usuarios_tb")]
    public class UsuariosAD
    {
        [Key]
        [Column("IdUsuario")]
        public string IdUsuario { get; set; }
        [Column("Nombre")]
        public string Nombre { get; set; }
        [Column("Apellido")]
        public string Apellido { get; set; }
        [Column("Email")]
        public string Email { get; set; }
        [Column("FechaDeNacimiento")]
        public DateTime FechaDeNacimiento { get; set; }
        [Column("Identificacion")]
        public string Identificacion { get; set; }
        [Column("FechaDeRegistro")]
        public DateTime FechaDeRegistro { get; set; }
        [Column("FechaDeModificacion")]
        public DateTime? FechaDeModificacion { get; set; }
        [Column("Rol")]
        public string Rol { get; set; }
        [Column("Estado")]
        public bool Estado { get; set; }
        [Column("TipoIdentificacion")]
        public string TipoIdentificacion { get; set; }

        public virtual List<TelefonoAD> Telefonos { get; set; }

    }
}
