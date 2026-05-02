using Abstracciones.Api;
using Abstracciones.Excepciones;
using Abstracciones.Servicios;
using Microsoft.AspNetCore.Mvc;
using static Abstracciones.Modelos.Requests.GruposRequests;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/groups")]
    public class GruposController : ControllerBase, IGruposController
    {
        private readonly IGruposService _grupos;
        private readonly ILogger<GruposController> _logger;
        public GruposController(IGruposService gruposService, ILogger<GruposController> logger)
        {
            _logger = logger;
            _grupos = gruposService;
        }
        [HttpPost]
        public async Task<IActionResult> AgregarGrupo(string IdUsuario, AgregarGrupoRequest grupo)
        {
            try
            {
                var resultado = await _grupos.AgregarGrupo(IdUsuario, grupo);
                return Ok($"Se agrego el grupo de id : {resultado}");
            }
            catch (BusinessException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar el grupo");
                return StatusCode(500, "Ocurrió un error inesperado");
            }
        }
        [HttpGet("{IdGrupo}")]
        public async Task<IActionResult> BuscarGruposPorId(int IdGrupo)
        {
            try
            {
                var grupo = await _grupos.BuscarGruposPorId(IdGrupo);
                return Ok(grupo);
            }
            catch (BusinessException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar el grupo");
                return StatusCode(500, "Ocurrió un error inesperado");
            }
        }
        [HttpPut]
        public async Task<IActionResult> EditarGrupo(string IdUsuario, EditarGrupoRequest grupo)
        {
            try
            {
                await _grupos.EditarGrupo(IdUsuario, grupo);
                return Ok($"Se ha editado correctamente el grupo de id {grupo.idGrupo}");
            }
            catch (BusinessException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al editar el grupo");
                return StatusCode(500, "Ocurrió un error inesperado");
            }
        }
        [HttpGet]
        public async Task<IActionResult> ListarGrupos()
        {
            try
            {
                var lista = await _grupos.ListarGrupos();
                return Ok(lista);
            }
            catch (BusinessException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar grupos");
                return StatusCode(500, "Ocurrió un error inesperado");
            }
        }

        [HttpGet("exportarPdf/{IdGrupo}")]
        public async Task<IActionResult> ExportarGrupoPDF(int IdGrupo)
        {
            var pdf = await _grupos.ExportarGrupoPDF(IdGrupo, null);//no paso logo
            if (pdf != null)
                return File(pdf, "application/pdf");

            return NotFound($"No se encontró el grupo de ID: {IdGrupo}");
        }
        [HttpGet("exportarQr")]
        public IActionResult QrExportar(int IdGrupo)
        {
            var url = Url.Action("ExportarGrupoPDF", "Grupos", new { IdGrupo }, Request.Scheme);
            var qr = _grupos.QrExportar(url);
            return File(qr, "image/png");

        }
    }
}
