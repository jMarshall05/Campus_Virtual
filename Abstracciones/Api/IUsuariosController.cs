using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.Modelos.ModelosDto;
using Abstracciones.Modelos.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Abstracciones.Api
{
    public interface IUsuariosController
    {
        Task<IActionResult> EditarUsuarioAdmin(string id, UsuariosDto usuario, int? idGrupo);
        Task<IActionResult> EditarUsuario(string id, EditarUsuarioRequest request);
        Task<IActionResult> ListarUsuarios();
        Task<IActionResult> ObtenerUsuarioPorId(string idUsuario);
    }
}
