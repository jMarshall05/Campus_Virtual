using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.Modelos.ModelosDA;
using Abstracciones.Modelos.ModelosDto;

namespace Abstracciones.DA
{
    public interface ICalificacionesAD
    {
        Task<int> AgregarCalificacion(CalificacionesAD calificacion);
        Task<int> EditarCalificacion(int id, CalificacionesAD calificacion);
        Task<int> EliminarCalificacion(int id_calificacion);
        Task<IEnumerable<CalificacionesDto>> ListarCalificaciones();
        Task<IEnumerable<CalificacionesDto>> ListarCalificacionesPorGrupo(int idGrupo);
        Task<IEnumerable<CalificacionesDto>> ListarCalificacionesPorEstudiante(string idEstudiante);

    }
}
