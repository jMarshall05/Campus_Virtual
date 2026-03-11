using Abstracciones.Modelos.ModelosDto;
using DA.Entidades;

namespace DA.Interfaces
{
    public interface ICursosAD
    {
        Task<int> AgregarCurso(CursosAD curso);
        Task<IEnumerable<CursoDto>> ListarCursos();
        Task<CursoDto> ObtenerPorId(int idCurso);
        Task ModificarEstadoCurso(int idCurso);

    }
}
