using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.Modelos.ModelosDto;
using Microsoft.AspNetCore.Mvc;

namespace Abstracciones.Api
{
    public interface IUsuariosController
    {
        Task<IActionResult> AgregarUsuario(UsuariosDto usuario);
        Task<IActionResult> EditarUsuarioAdmin(string id, UsuariosDto usuario);
        Task<IActionResult> EditarUsuario(string id, UsuariosDto usuario);
        Task<IActionResult> ListarUsuarios();
        Task<IActionResult> ObtenerUsuarioPorId(string idUsuario);
    }
}
