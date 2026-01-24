using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.Modelos.ModelosDA;
using Abstracciones.Modelos.ModelosDto;

namespace Abstracciones.DA
{
    public interface ITareasAD
    {
        Task<int> AgregarTarea(TareasAD tarea);
        Task<bool> EditarTarea(int id, TareasAD tarea);
        Task<bool> EliminarTarea(int idTarea);
        Task<IEnumerable<TareaDto>> ListarTareasAsync();
        Task<TareaDto> ObtenerPorIdAsync(int idTarea);
        Task<List<TareaDto>> ListarTareasPorEstudiante(string idEstudiante);

    }
}
