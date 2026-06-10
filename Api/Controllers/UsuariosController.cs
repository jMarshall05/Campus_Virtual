using Abstracciones.Api;
using Abstracciones.Servicios;
using Microsoft.AspNetCore.Mvc;
using static Abstracciones.Modelos.Requests.UsuariosRequests;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsuariosController : ControllerBase, IUsuariosController
    {
        private readonly IUsuariosService _usuario;
        public UsuariosController(IUsuariosService usuario)
        {
            _usuario = usuario;
        }

        [HttpPatch("{IdUsuario}")]
        public async Task<IActionResult> EditarUsuario(string IdUsuario, EditarUsuarioRequest request)
        {
            await _usuario.EditarUsuario(IdUsuario, request);
            return Ok($"Se ha editado el usuario con ID : {IdUsuario}");
        }

        [HttpPut("{IdUsuario}/admin")]
        public async Task<IActionResult> EditarUsuarioAdmin(string IdUsuario, [FromBody] EditarUsuarioAdminRequest usuario, [FromQuery] int? idGrupo)
        {
            await _usuario.EditarUsuarioAdmin(IdUsuario, usuario, idGrupo);
            return Ok($"Se ha editado el usuario con ID : {IdUsuario}");
        }

        [HttpGet("ByRol")]
        public async Task<IActionResult> ListarPorRol([FromQuery] string rol)
        {
            var usuarios = await _usuario.ListarPorRol(rol);
            if (usuarios != null)
                return Ok(usuarios);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> ListarUsuarios()
        {
            var usuarios = await _usuario.ListarUsuarios();
            if (usuarios != null)
                return Ok(usuarios);
            return NoContent();
        }

        [HttpGet("{IdUsuario}")]
        public async Task<IActionResult> ObtenerUsuarioPorId(string IdUsuario)
        {
            var usuario = await _usuario.ObtenerUsuarioPorId(IdUsuario);
            if (usuario != null)
                return Ok(usuario);
            return NotFound($"No se encontró el usuario de ID: {IdUsuario}");
        }

        [HttpGet("exportarPdf/{IdUsuario}")]
        public async Task<IActionResult> ExportarUsuarioPDF(string IdUsuario)
        {
            var pdf = await _usuario.ExportarUsuarioPDF(IdUsuario, null);
            if (pdf != null)
                return File(pdf, "application/pdf");
            return NotFound($"No se encontró el usuario de ID: {IdUsuario}");
        }

        [HttpGet("exportarPdf")]
        public async Task<IActionResult> ExportarUsuariosGeneralPDF()
        {
            var pdf = await _usuario.ExportarUsuariosGeneralPDF(null);
            if (pdf != null)
                return File(pdf, "application/pdf");
            return NoContent();
        }

        [HttpGet("exportarQr")]
        public IActionResult QrExportar(string? IdUsuario)
        {
            if (!string.IsNullOrEmpty(IdUsuario))
            {
                var url = Url.Action("ExportarUsuarioPDF", "Usuarios", new { IdUsuario }, Request.Scheme);
                var qr = _usuario.QrExportar(url);
                return File(qr, "image/png");
            }
            var link = Url.Action("ExportarUsuariosGeneralPDF", "Usuarios", null, Request.Scheme);
            var Qr = _usuario.QrExportar(link);
            return File(Qr, "image/png");
        }
    }
}
