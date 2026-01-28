using Abstracciones.Api;
using Abstracciones.Excepciones;
using Abstracciones.Modelos.ModelosDA;
using Abstracciones.Modelos.ModelosDto;
using Abstracciones.Servicios;
using Microsoft.AspNetCore.Mvc;

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
        [HttpPost("Agregar")]
        public async Task<IActionResult> AgregarTarea(TareasAD tarea)
        {
            try
            {
                var resultado = await _tareas.AgregarTarea(tarea);
                return Ok(new TareaDto { IdTarea = resultado });
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
        [HttpPatch("CambiarEstado/{idTarea}")]
        public async Task<IActionResult> CambiarEstadoTarea(int idTarea)
        {
            try
            {
                await _tareas.CambiarEstadoTarea(idTarea);
                return Accepted($"Se ha cambiado el estado de la tarea con ID : {idTarea}");
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
        [HttpPut("Editar/{idTarea}")]
        public async Task<IActionResult> EditarTarea(int idTarea, TareasAD tarea)
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
        [HttpGet("Listar")]
        public async Task<IActionResult> ListarTareas()
        {
            try
            {
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
        [HttpPost("ListarPorEstudiante")]
        public async Task<IActionResult> ListarTareasPorEstudiante(EstudianteGrupoDto estudianteGrupo)
        {
            try
            {

                var resultado = await _tareas.ListarTareasPorEstudiante(estudianteGrupo);
                return Ok(resultado);
            }
            catch (BusinessException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al listar las tareas para el estudiante con ID: {EstudianteId}", estudianteGrupo.EstudianteId);
                return StatusCode(500, "Algo inesperado a sucedido");

            }
        }
        [HttpGet("ObtenerPorId/{idTarea}")]
        public async Task<IActionResult> ObtenerPorId(int idTarea)
        {
            try
            {
                var tarea = await _tareas.ObtenerPorId(idTarea);
                return Ok(tarea);
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
