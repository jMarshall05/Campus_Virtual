using Microsoft.AspNetCore.Mvc;
using static Abstracciones.Modelos.Requests.GruposRequests;

namespace Abstracciones.Api
{
    public interface IGruposController
    {
        Task<IActionResult> AgregarGrupo(string IdUsuario, AgregarGrupoRequest grupo);
        Task<IActionResult> EditarGrupo(string IdUsuario, EditarGrupoRequest grupo);
        Task<IActionResult> ListarGrupos();
        Task<IActionResult> BuscarGruposPorId(int idGrupo);
    }
}
