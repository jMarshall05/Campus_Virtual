using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.Modelos.ModelosDA;
using Abstracciones.Modelos.ModelosDto;

namespace Abstracciones.Servicios
{
    public interface ITareasService
    {
        Task<int> AgregarTarea(TareasAD tarea);
        Task EditarTarea(int id, TareasAD tarea);
        Task CambiarEstadoTarea(int idTarea);
        Task<IEnumerable<TareaDto>> ListarTareas();
        Task<TareaDto> ObtenerPorId(int idTarea);
        Task<List<TareaDto>> ListarTareasPorEstudiante(EstudianteGrupoDto estudianteGrupo);
    }
}
