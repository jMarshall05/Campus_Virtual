using Abstracciones.Modelos.ModelosDto;
using Microsoft.AspNetCore.Mvc;
using static Abstracciones.Modelos.Requests.UsuariosRequests;

namespace Abstracciones.Api
{
    public interface IUsuariosController
    {
        Task<IActionResult> EditarUsuarioAdmin(string id, EditarUsuarioAdminRequest usuario, int? idGrupo);
        Task<IActionResult> EditarUsuario(string id, EditarUsuarioRequest request);
        Task<IActionResult> ListarUsuarios();
        Task<IActionResult> ObtenerUsuarioPorId(string idUsuario);
        Task<ActionResult> ListarPorRol(string rol);

    }
}
