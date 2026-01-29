using Abstracciones.Modelos.ModelosDto;
using DA.Entidades;

namespace DA.Interfaces
{
    public interface ICalificacionesAD
    {
        Task<int> AgregarCalificacion(CalificacionesAD calificacion);
        Task EditarCalificacion(CalificacionesAD calificacion);
        Task EliminarCalificacion(int id_calificacion);
        Task<IEnumerable<CalificacionesDto>> ListarCalificaciones();
        Task<IEnumerable<CalificacionesDto>> ListarCalificacionesPorGrupo(int idGrupo);
        Task<IEnumerable<CalificacionesDto>> ListarCalificacionesPorEstudiante(string idEstudiante);

    }
}
