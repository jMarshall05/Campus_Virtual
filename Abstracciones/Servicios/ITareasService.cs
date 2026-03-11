using Abstracciones.Modelos.ModelosDto;
using static Abstracciones.Modelos.Requests.TareasRequests;

namespace Abstracciones.Servicios
{
    public interface ITareasService
    {
        Task<int> AgregarTarea(AgregarTareaRequest tarea);
        Task EditarTarea(int id, EditarTareaRequest tarea);
        Task CambiarEstadoTarea(int idTarea);
        Task<IEnumerable<TareaDto>> ListarTareas();
        Task<TareaDto> ObtenerPorId(int idTarea);
        Task<List<TareaDto>> ListarTareasPorGrupo(int IdGrupo);
    }
}
