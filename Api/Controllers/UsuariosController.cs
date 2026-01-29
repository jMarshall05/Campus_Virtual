using Abstracciones.Api;
using Abstracciones.Excepciones;
using Abstracciones.Modelos.ModelosDto;
using Abstracciones.Modelos.Requests;
using Abstracciones.Servicios;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using static Abstracciones.Modelos.Requests.UsuariosRequests;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsuariosController : ControllerBase, IUsuariosController
    {
        private readonly IUsuariosService _usuario;
        private readonly ILogger<TareasController> _logger;
        public UsuariosController(IUsuariosService usuario, ILogger<TareasController> logger)
        {
            _usuario = usuario;
            _logger = logger;
        }
        [HttpPatch("Edit")]
        public async Task<IActionResult> EditarUsuario(string id, EditarUsuarioRequest request)
        {
            try
            {
                await _usuario.EditarUsuario(id, request);
                return Accepted($"Se ha editado el usuario con ID : {id}");

            }
            catch (BusinessException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al editar el usuario con ID: {UserId}", id);
                return StatusCode(500, "Algo inesperado a sucedido");

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
            catch (BusinessException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al editar el usuario con ID: {UserId}", id);
                return StatusCode(500, "Algo inesperado a sucedido");

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
