using Abstracciones.Modelos.ModelosDto;
using DA.Entidades;

namespace DA.Interfaces
{
    public interface ITareasAD
    {
        Task<int> AgregarTarea(TareasAD tarea);
        Task EditarTarea(int id, TareasAD tarea);
        Task CambiarEstadoTarea(int idTarea);
        Task<IEnumerable<TareaDto>> ListarTareas();
        Task<TareaDto> ObtenerPorId(int idTarea);
        Task<List<TareaDto>> ListarTareasPorGrupo(int IdGrupo);

    }
}
