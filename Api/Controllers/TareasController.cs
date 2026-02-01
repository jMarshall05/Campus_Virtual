using Abstracciones.Api;
using Abstracciones.Excepciones;
using Abstracciones.Modelos.ModelosDto;
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
        private readonly ILogger<TareasController> _logger;
        public TareasController(ITareasService tareas, ILogger<TareasController> logger)
        {
            _tareas = tareas;
            _logger = logger;
        }
        [HttpPost]
        public async Task<IActionResult> AgregarTarea(AgregarTareaRequest tarea)
        {
            try
            {
                var resultado = await _tareas.AgregarTarea(tarea);
                return Ok($"Se inserto correctamente la tarea id: {resultado}");
            }
            catch (BusinessException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar la tarea");
                return StatusCode(500, "Ocurrió un error inesperado");
            }

        }
        [HttpPatch("{idTarea}/estado")]
        public async Task<IActionResult> CambiarEstadoTarea(int idTarea)
        {
            try
            {
                await _tareas.CambiarEstadoTarea(idTarea);
                return Ok($"Se ha cambiado el estado de la tarea con ID : {idTarea}");
            }
            catch (BusinessException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cambiar el estado de la tarea con ID: {TareaId}", idTarea);
                return StatusCode(500, "Algo inesperado a sucedido");
            }
        }
        [HttpPut("{idTarea}")]
        public async Task<IActionResult> EditarTarea(int idTarea, EditarTareaRequest tarea)
        {
            try
            {
                await _tareas.EditarTarea(idTarea, tarea);
                return Accepted($"Se ha editado la tarea con ID : {idTarea}");
            }
            catch (BusinessException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al editar la tarea con ID: {TareaId}", idTarea);
                return StatusCode(500, "Algo inesperado a sucedido");
            }
        }
        [HttpGet]
        public async Task<IActionResult> ListarTareas()
        {
            try
            {
                var headers = HttpContext.Request.Headers;
                var lista = await _tareas.ListarTareas();
                return Ok(lista);
            }
            catch (BusinessException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al listar las tareas");
                return StatusCode(500, "Algo inesperado a sucedido");
            }
        }
        [HttpGet("grupo/{IdGrupo}")]
        public async Task<IActionResult> ListarTareasPorGrupo(int IdGrupo)
        {
            try
            {

                var resultado = await _tareas.ListarTareasPorGrupo(IdGrupo);
                return Ok(resultado);
            }
            catch (BusinessException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al listar las tareas para el estudiante con ID: {EstudianteId}", IdGrupo);
                return StatusCode(500, "Algo inesperado a sucedido");

            }
        }
        [HttpGet("{idTarea}")]
        public async Task<IActionResult> ObtenerPorId(int idTarea)
        {
            try
            {
                var tarea = await _tareas.ObtenerPorId(idTarea);
                if (tarea != null)
                    return Ok(tarea);
                return NotFound($"No existe tarea con id : {idTarea}");
            }
            catch (BusinessException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la tarea con ID: {TareaId}", idTarea);
                return StatusCode(500, "Algo inesperado a sucedido");
            }
        }
    }
}
