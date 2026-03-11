using Abstracciones.Api;
using Abstracciones.Excepciones;
using Abstracciones.Modelos.Requests;
using Abstracciones.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/materias")]
    public class MateriasController : ControllerBase, IMateriaController
    {
        private readonly IMateriasService _materias;
        private readonly ILogger<MateriasController> _logger;
        public MateriasController(IMateriasService materias, ILogger<MateriasController> logger)
        {
            _materias = materias;
            _logger = logger;
        }
        [HttpPost]
        public async Task<IActionResult> AgregarMateria(MateriaRequests materia)
        {
            try
            {
                var resultado = await _materias.AgregarMateria(materia);
                return Ok($"Se a agregado la materia con id: {resultado}");
            }
            catch (BusinessException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar la materia");
                return StatusCode(500, "Ocurrió un error inesperado");
            }
        }
        [HttpPatch("{materiaId}/estado")]
        public async Task<IActionResult> CambiarEstadoMateria(int materiaId)
        {
            try
            {
                await _materias.CambiarEstadoMateria(materiaId);
                return Ok($"Estado de la ,materia de id {materiaId} cambiado");
            }
            catch (BusinessException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al editar materia");
                return StatusCode(500, "Ocurrió un error inesperado");
            }
        }
        [HttpPut("{IdMateria}")]
        public async Task<IActionResult> EditarMateria(int IdMateria, MateriaRequests materia)
        {
            try
            {
                await _materias.EditarMateria(IdMateria, materia);
                return Ok($"Materia de id {IdMateria} editada");
            }
            catch (BusinessException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al editar materia");
                return StatusCode(500, "Ocurrió un error inesperado");
            }
        }
        [HttpGet]
        public async Task<IActionResult> ListarMaterias()
        {
            try
            {
                var lista = await _materias.ListarMaterias();
                return Ok(lista);
            }
            catch (BusinessException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener materias");
                return StatusCode(500, "Ocurrió un error inesperado");

            }
        }

        [HttpGet("ByName")]
        public async Task<IActionResult> ObtenerMateriaNombre([FromQuery] string nombre)
        {
            try
            {
                var lista = await _materias.ObtenerMateriaNombre(nombre);
                return Ok(lista);
            }
            catch (BusinessException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener materia");
                return StatusCode(500, "Ocurrió un error inesperado");

            }
        }
        [HttpGet("{idMateria}")]
        public async Task<IActionResult> ObtenerMateriaPorId(int idMateria)
        {
            try
            {
                var lista = await _materias.ObtenerMateriaPorId(idMateria);
                return Ok(lista);
            }
            catch (BusinessException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener materia");
                return StatusCode(500, "Ocurrió un error inesperado");

            }
        }
    }
}
