using Abstracciones.Modelos.ModelosDto;
using DA.Entidades;
using DA.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DA.Implementaciones
{
    public class CursosDA : ICursosAD
    {
        private readonly ApplicationDbContext _elContexto;
        public CursosDA(ApplicationDbContext elContexto)
        {
            _elContexto = elContexto;
        }

        public async Task<int> AgregarCurso(CursosAD curso)
        {
            var entidad = await _elContexto.Cursos.AddAsync(curso);
            await _elContexto.SaveChangesAsync();
            return entidad.Entity.IdCurso;
        }

        public async Task<IEnumerable<CursoDto>> ListarCursos()
        {
            var cursos = await _elContexto.Cursos
                 .Select(curso => new CursoDto
                 {
                     IdCurso = curso.IdCurso,
                     MateriaId = curso.MateriaId,
                     GrupoId = curso.GrupoId,
                     ProfesorId = curso.IdProfesor,
                     NombreProfesor = curso.Profesor != null
                         ? $"{curso.Profesor.Nombre} {curso.Profesor.Apellido}"
                         : string.Empty,
                     NombreGrupo = curso.Grupo.Nombre,
                     NombreMateria = curso.Materia.Nombre,
                     Estado = curso.Estado
                 }).ToListAsync();

            return cursos;
        }

        public async Task ModificarEstadoCurso(int idCurso)
        {
            var cursoExistente = await _elContexto.Cursos.FindAsync(idCurso);
            cursoExistente.Estado = !cursoExistente.Estado;
            await _elContexto.SaveChangesAsync();

        }

        public async Task<CursoDto> ObtenerPorId(int idCurso)
        {
            var curso = await _elContexto.Cursos
                   .Where(c => c.IdCurso == idCurso)
                 .Select(curso => new CursoDto
                 {
                     IdCurso = curso.IdCurso,
                     MateriaId = curso.MateriaId,
                     GrupoId = curso.GrupoId,
                     ProfesorId = curso.IdProfesor,
                     NombreProfesor = curso.Profesor != null
                         ? $"{curso.Profesor.Nombre} {curso.Profesor.Apellido}"
                         : string.Empty,
                     NombreGrupo = curso.Grupo.Nombre,
                     NombreMateria = curso.Materia.Nombre,
                     Estado = curso.Estado
                 }).FirstOrDefaultAsync();

            return curso;
        }
    }
}
