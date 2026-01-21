using System.ComponentModel;

namespace Abstracciones.Modelos.ModelosDto
{
    public class CursoDto
    {
        [DisplayName("Id del Curso")]
        public int IdCurso { get; set; }
        [DisplayName("Materia")]
        public int MateriaId { get; set; }
        [DisplayName("Grupo")]
        public int GrupoId { get; set; }
        [DisplayName("Profesor")]
        public string ProfesorId { get; set; }

        public string NombreProfesor { get; set; }
        public string NombreGrupo { get; set; }
        public string NombreMateria { get; set; }

        [DisplayName("Estado")]
        public bool Estado { get; set; }
    }
}
