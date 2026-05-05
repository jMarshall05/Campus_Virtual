using Abstracciones.Api;
using Abstracciones.Excepciones;
using Abstracciones.Modelos.ModelosDto;
using Abstracciones.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/announcements")]
    [Authorize(Roles = "Administradores")]
    public class AnunciosController : ControllerBase, IAnunciosController
    {
        private readonly IAnunciosService _anuncios;
        private readonly ILogger<AnunciosController> _logger;

        public AnunciosController(IAnunciosService anuncios, ILogger<AnunciosController> logger)
        {
            _anuncios = anuncios;
            _logger = logger;
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [Produces("application/json")]
        public async Task<IActionResult> AgregarAnuncio([FromForm] AnuncioDto anuncio)
        {
            try
            {
                var id = await _anuncios.AgregarAnuncio(anuncio);
                return Ok(id);
            }
            catch (BusinessException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar el anuncio");
                return StatusCode(500, "Ocurrió un error inesperado");
            }
        }

        [HttpPut]
        [Consumes("multipart/form-data")]
        [Produces("application/json")]
        public async Task<IActionResult> EditarAnuncio([FromForm] AnuncioDto anuncio)
        {
            try
            {
                await _anuncios.EditarAnuncio(anuncio);
                return Ok($"Anuncio {anuncio.IdAnuncio} actualizado correctamente");
            }
            catch (BusinessException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al editar el anuncio");
                return StatusCode(500, "Ocurrió un error inesperado");
            }
        }

        [HttpPatch("{anuncioId}")]
        public async Task<IActionResult> EliminarAnuncio(int anuncioId)
        {
            try
            {
                await _anuncios.CambiarEstadoAnuncio(anuncioId);
                return Ok($"Estado del anuncio {anuncioId} actualizado");
            }
            catch (BusinessException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cambiar estado del anuncio");
                return StatusCode(500, "Ocurrió un error inesperado");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ListarAnuncios()
        {
            try
            {
                var lista = await _anuncios.ListarAnuncios();
                return Ok(lista);
            }
            catch (BusinessException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al listar anuncios");
                return StatusCode(500, "Ocurrió un error inesperado");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerAnuncioPorId(int id)
        {
            try
            {
                var anuncio = await _anuncios.ObtenerAnuncioPorId(id);
                if (anuncio == null)
                    return NotFound($"No se encontró el anuncio con ID {id}");
                return Ok(anuncio);
            }
            catch (BusinessException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el anuncio");
                return StatusCode(500, "Ocurrió un error inesperado");
            }
        }
    }
}
