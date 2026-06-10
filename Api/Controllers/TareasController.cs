using Abstracciones.Api;
using Abstracciones.Servicios;
using Microsoft.AspNetCore.Mvc;
using static Abstracciones.Modelos.Requests.TareasRequests;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/tareas")]
    public class TareasController : ControllerBase, ITareasController
    {
        private readonly ITareasService _tareas;
        public TareasController(ITareasService tareas)
        {
            _tareas = tareas;
        }

        [HttpPost]
        public async Task<IActionResult> AgregarTarea(AgregarTareaRequest tarea)
        {
            var resultado = await _tareas.AgregarTarea(tarea);
            return Ok($"Se inserto correctamente la tarea id: {resultado}");
        }

        [HttpPatch("{idTarea}/estado")]
        public async Task<IActionResult> CambiarEstadoTarea(int idTarea)
        {
            await _tareas.CambiarEstadoTarea(idTarea);
            return Ok($"Se ha cambiado el estado de la tarea con ID : {idTarea}");
        }

        [HttpPut("{idTarea}")]
        public async Task<IActionResult> EditarTarea(int idTarea, EditarTareaRequest tarea)
        {
            await _tareas.EditarTarea(idTarea, tarea);
            return Accepted($"Se ha editado la tarea con ID : {idTarea}");
        }

        [HttpGet]
        public async Task<IActionResult> ListarTareas()
        {
            var lista = await _tareas.ListarTareas();
            return Ok(lista);
        }

        [HttpGet("grupo/{IdGrupo}")]
        public async Task<IActionResult> ListarTareasPorGrupo(int IdGrupo)
        {
            var resultado = await _tareas.ListarTareasPorGrupo(IdGrupo);
            return Ok(resultado);
        }

        [HttpGet("{idTarea}")]
        public async Task<IActionResult> ObtenerPorId(int idTarea)
        {
            var tarea = await _tareas.ObtenerPorId(idTarea);
            if (tarea != null)
                return Ok(tarea);
            return NotFound($"No existe tarea con id : {idTarea}");
        }
    }
}
