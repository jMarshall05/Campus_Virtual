using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DA.Entidades
{
    [Table("anuncios")]
    public class AnunciosAD
    {
        [Key]
        [Column("id_anuncio")]
        public int IdAnuncio { get; set; }

        [Column("titulo")]
        [Required]
        [StringLength(150)]
        public string Titulo { get; set; }

        [Column("descripcion")]
        [Required]
        public string Descripcion { get; set; }

        [Column("fecha_evento")]
        [Required]
        public DateTime FechaEvento { get; set; }

        [Column("fecha_publicacion")]
        [Required]
        public DateTime FechaPublicacion { get; set; }
        [Column("imagen_ruta")]

        public string ImagenRuta { get; set; }
        [Column("estado")]
        public bool Estado { get; set; }
    }
}
