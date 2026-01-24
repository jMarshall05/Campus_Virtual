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
        Task<bool> EditarUsuarioAdmin(string id, UsuariosDto usuario, int? idGrupo);
        Task<bool> EditarUsuario(string id, EditarUsuarioRequest request);
        Task<List<UsuariosDto>> ListarUsuarios();
        Task<UsuariosDto> ObtenerUsuarioPorId(string idUsuario);
    }
}
