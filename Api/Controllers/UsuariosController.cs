using Abstracciones.Api;
using Abstracciones.Modelos.ModelosDto;
using Abstracciones.Modelos.Requests;
using Abstracciones.Servicios;
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
        [HttpPatch("edit")]
        public async Task<IActionResult> EditarUsuario(string id, EditarUsuarioRequest request)
        {
            var resultado = await _usuario.EditarUsuario(id, request);

            if(resultado)
                return Accepted();
            return StatusCode(500, "Error interno del servidor");
        }
        [HttpPut("AdminEdit")]
        public Task<IActionResult> EditarUsuarioAdmin(string id, UsuariosDto usuario)
        {
            throw new NotImplementedException();
        }
        [HttpGet("List")]
        public Task<IActionResult> ListarUsuarios()
        {
            throw new NotImplementedException();
        }
        [HttpGet("GetByID")]
        public Task<IActionResult> ObtenerUsuarioPorId(string idUsuario)
        {
            throw new NotImplementedException();
        }
    }
}
