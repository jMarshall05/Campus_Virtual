using Abstracciones.Api;
using Abstracciones.Excepciones;
using Abstracciones.Modelos.ModelosDto;
using Abstracciones.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/courses")]
    public class CursosController : ControllerBase, ICursosController
    {
        private readonly ICursosService _cursos;
        private readonly ILogger<ICursosController> _logger;
        public CursosController(ICursosService cursos, ILogger<ICursosController> logger)
        {
            _cursos = cursos;
            _logger = logger;

        }


        [HttpPost]
        public async Task<IActionResult> AgregarCurso(CursoDto curso)
        {
            try
            {
                var response = await _cursos.AgregarCurso(curso);
                return Ok($"Se a agrgado correctamente el curso , id: {response}");
            }
            catch (BusinessException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al Agregar Documento");
                return StatusCode(500, "Ocurrió un error inesperado");
            }
        }
        [HttpGet]
        public async Task<IActionResult> ListarCursos()
        {
            try
            {
                var cursos = await _cursos.ListarCursos();
                return Ok(cursos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al Agregar Documento");
                return StatusCode(500, "Ocurrió un error inesperado");

            }
        }
        [HttpPatch("{idCurso}")]
        public async Task<IActionResult> ModificarEstadoCurso(int idCurso)
        {
            try
            {
                await _cursos.ModificarEstadoCurso(idCurso);
                return Ok();
            }
            catch (BusinessException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al Agregar Documento");
                return StatusCode(500, "Ocurrió un error inesperado");
            }
        }
        [HttpGet("{idCurso}")]
        public async Task<IActionResult> ObtenerPorId(int idCurso)
        {
            try
            {
                var curso = await _cursos.ObtenerPorId(idCurso);
                return Ok(curso);
            }
            catch (BusinessException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al Agregar Documento");
                return StatusCode(500, "Ocurrió un error inesperado");

            }
        }
    }
}
