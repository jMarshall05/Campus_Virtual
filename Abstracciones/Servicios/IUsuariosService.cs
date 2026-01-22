using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.Modelos.ModelosDto;
using Abstracciones.Modelos.Requests;

namespace Abstracciones.Servicios
{
    public interface IUsuariosService
    {
        Task<string> AgregarUsuario(RegisterRequest request);
        Task<int> EditarUsuarioAdmin(string id, UsuariosDto usuario);
        Task<int> EditarUsuario(string id, UsuariosDto usuario);
        Task<List<UsuariosDto>> ListarUsuarios();
        Task<UsuariosDto> ObtenerUsuarioPorId(string idUsuario);
    }
}
