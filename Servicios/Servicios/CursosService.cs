using Abstracciones.Modelos.ModelosDto;
using Abstracciones.Servicios;
using DA.Entidades;
using DA.Interfaces;
using Mapster;
using static Abstracciones.Modelos.Requests.CursosRequest;

namespace Servicios.Servicios
{
    public class CursosService : ICursosService
    {
        private readonly ICursosAD _cursos;

        public CursosService(ICursosAD cursos)
        {
            _cursos = cursos;
        }
        public async Task<int> AgregarCurso(AgregarCursoRequest curso)
        {
            var request =await _cursos.AgregarCurso(curso.Adapt<CursosAD>());
            return request;
        }

        public async Task<IEnumerable<CursoDto>> ListarCursos()
        {
           var list =await _cursos.ListarCursos();
            return list;
        }

        public async Task ModificarEstadoCurso(int idCurso)
        {
           await _cursos.ModificarEstadoCurso(idCurso);
        }

        public Task<CursoDto> ObtenerPorId(int idCurso)
        {
           var curso = _cursos.ObtenerPorId(idCurso);
            return curso;
        }
    }
}
