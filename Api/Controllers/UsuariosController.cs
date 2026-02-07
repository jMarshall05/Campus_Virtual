using Abstracciones.Api;
using Abstracciones.Excepciones;
using Abstracciones.Servicios;
using Microsoft.AspNetCore.Authorization;
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
        [HttpPatch("{IdUsuario}")]
        public async Task<IActionResult> EditarUsuario(string IdUsuario, EditarUsuarioRequest request)
        {
            try
            {
                await _usuario.EditarUsuario(IdUsuario, request);
                return Ok($"Se ha editado el usuario con ID : {IdUsuario}");

            }
            catch (BusinessException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al editar el usuario con ID: {UserId}", IdUsuario);
                return StatusCode(500, "Algo inesperado a sucedido");

            }
        }
        [HttpPut("{IdUsuario}/admin")]
        public async Task<IActionResult> EditarUsuarioAdmin(string IdUsuario, EditarUsuarioAdminRequest usuario, int? idGrupo)
        {
            try
            {
                await _usuario.EditarUsuarioAdmin(IdUsuario, usuario, idGrupo);
                return Ok($"Se ha editado el usuario con ID : {IdUsuario}");

            }
            catch (BusinessException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al editar el usuario con ID: {UserId}", IdUsuario);
                return StatusCode(500, "Algo inesperado a sucedido");

            }
        }
        [HttpGet("ByRol")]
        public async Task<ActionResult> ListarPorRol([FromQuery] string rol)
        {
            var usuarios = await _usuario.ListarPorRol(rol);
            if (usuarios != null)
                return Ok(usuarios);
            return NoContent();


        }

        [HttpGet]
        //[Authorize(Roles = "Administradores")]
        public async Task<IActionResult> ListarUsuarios()
        {
            var usuarios = await _usuario.ListarUsuarios();
            if (usuarios != null)
                return Ok(usuarios);
            return NoContent();

        }
        [HttpGet("{IdUsuario}")]
        public async Task<IActionResult> ObtenerUsuarioPorId(string idUsuario)
        {
            var usuario = await _usuario.ObtenerUsuarioPorId(idUsuario);
            if (usuario != null)
                return Ok(usuario);
            return NotFound($"No se encontró el usuario de ID: {idUsuario}");
        }
    }
}
