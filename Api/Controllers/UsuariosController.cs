using Abstracciones.Api;
using Abstracciones.Modelos.ModelosDto;
using Abstracciones.Modelos.Requests;
using Abstracciones.Servicios;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;

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
        [HttpPatch("Edit")]
        public async Task<IActionResult> EditarUsuario(string id, EditarUsuarioRequest request)
        {
            var resultado = await _usuario.EditarUsuario(id, request);

            if (resultado)
                return Accepted($"Se ha editado el usuario con ID : {id}");
            return StatusCode(500, "Error interno del servidor");
        }
        [HttpPut("AdminEdit")]
        public async Task<IActionResult> EditarUsuarioAdmin(string id, UsuariosDto usuario, int? idGrupo)
        {
            var resultado = await _usuario.EditarUsuarioAdmin(id, usuario, idGrupo);

            if (resultado)
                return Accepted($"Se ha editado el usuario con ID : {id}");
            return StatusCode(500, "Error interno del servidor");
        }
        [HttpGet("List")]
        public async Task<IActionResult> ListarUsuarios()
        {
            var usuarios = await _usuario.ListarUsuarios();
            if (usuarios != null)
                return Ok(usuarios);
            return NoContent();

        }
        [HttpGet("GetByID")]
        public async Task<IActionResult> ObtenerUsuarioPorId(string idUsuario)
        {
            var usuario = await _usuario.ObtenerUsuarioPorId(idUsuario);
            if (usuario != null)
                return Ok(usuario);
            return NotFound($"No se encontró el usuario de ID: {idUsuario}");
        }
    }
}
