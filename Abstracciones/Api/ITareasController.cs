using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.Modelos.ModelosDto;
using Abstracciones.Modelos.Requests;
using Microsoft.AspNetCore.Mvc;
using static Abstracciones.Modelos.Requests.TareasRequests;

namespace Abstracciones.Api
{
    public interface ITareasController
    {
        Task<IActionResult> AgregarTarea(AgregarTareaRequest tarea);
        Task<IActionResult> EditarTarea(int id, EditarTareaRequest tarea);
        Task<IActionResult> CambiarEstadoTarea(int idTarea);
        Task<IActionResult> ListarTareas();
        Task<IActionResult> ObtenerPorId(int idTarea);
        Task<IActionResult> ListarTareasPorEstudiante(EstudianteGrupoDto estudianteGrupo);
    }
}
