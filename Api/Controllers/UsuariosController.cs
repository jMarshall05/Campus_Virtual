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
            try
            {
                await _usuario.EditarUsuario(id, request);
                return Accepted($"Se ha editado el usuario con ID : {id}");

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);

            }
        }
        [HttpPut("AdminEdit")]
        public async Task<IActionResult> EditarUsuarioAdmin(string id, UsuariosDto usuario, int? idGrupo)
        {
            try
            {
                await _usuario.EditarUsuarioAdmin(id, usuario, idGrupo);
                return Accepted($"Se ha editado el usuario con ID : {id}");

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message );
            }
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
