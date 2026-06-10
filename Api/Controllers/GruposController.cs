using Abstracciones.Api;
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
        public GruposController(IGruposService gruposService)
        {
            _grupos = gruposService;
        }

        [HttpPost]
        public async Task<IActionResult> AgregarGrupo(string IdUsuario, AgregarGrupoRequest grupo)
        {
            var resultado = await _grupos.AgregarGrupo(IdUsuario, grupo);
            return Ok($"Se agrego el grupo de id : {resultado}");
        }

        [HttpGet("{IdGrupo}")]
        public async Task<IActionResult> BuscarGruposPorId(int IdGrupo)
        {
            var grupo = await _grupos.BuscarGruposPorId(IdGrupo);
            return Ok(grupo);
        }

        [HttpPut]
        public async Task<IActionResult> EditarGrupo(string IdUsuario, EditarGrupoRequest grupo)
        {
            await _grupos.EditarGrupo(IdUsuario, grupo);
            return Ok($"Se ha editado correctamente el grupo de id {grupo.idGrupo}");
        }

        [HttpGet]
        public async Task<IActionResult> ListarGrupos()
        {
            var lista = await _grupos.ListarGrupos();
            return Ok(lista);
        }

        [HttpGet("exportarPdf/{IdGrupo}")]
        public async Task<IActionResult> ExportarGrupoPDF(int IdGrupo)
        {
            var pdf = await _grupos.ExportarGrupoPDF(IdGrupo, null);
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
