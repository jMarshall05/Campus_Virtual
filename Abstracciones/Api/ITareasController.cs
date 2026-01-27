using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.Modelos.ModelosDA;
using Abstracciones.Modelos.ModelosDto;
using Microsoft.AspNetCore.Mvc;

namespace Abstracciones.Api
{
    public interface ITareasController
    {
        Task<IActionResult> AgregarTarea(TareasAD tarea);
        Task<IActionResult> EditarTarea(int id, TareasAD tarea);
        Task<IActionResult> CambiarEstadoTarea(int idTarea);
        Task<IActionResult> ListarTareas();
        Task<IActionResult> ObtenerPorId(int idTarea);
        Task<IActionResult> ListarTareasPorEstudiante(EstudianteGrupoDto estudianteGrupo);
    }
}
