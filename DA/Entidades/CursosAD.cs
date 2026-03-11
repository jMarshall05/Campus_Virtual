using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DA.Entidades
{
    [Table("Cursos")]
    public class CursosAD
    {
        [Key]
        [Column("IdCurso")]
        public int IdCurso { get; set; }
        [Required]
        [Column("MateriaId")]
        public int MateriaId { get; set; }
        [ForeignKey(nameof(MateriaId))]
        public virtual MateriasAD Materia { get; set; }
        [Required]
        [Column("IdProfesor")]
        public string IdProfesor { get; set; }
        [ForeignKey(nameof(IdProfesor))]
        public virtual UsuariosAD Profesor { get; set; }
        [Required]
        [Column("GrupoId")]
        public int GrupoId { get; set; }
        [ForeignKey(nameof(GrupoId))]
        public virtual GruposAD Grupo { get; set; }

        [Column("estado")]
        public bool Estado { get; set; }

    }
}
