using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.Modelos.ModelosDto;
using Abstracciones.Modelos.Requests;
using static Abstracciones.Modelos.Requests.UsuariosRequests;

namespace Abstracciones.Servicios
{
    public interface IUsuariosService
    {
        Task<string> AgregarUsuario(RegisterRequest request);
        Task EditarUsuarioAdmin(string id, UsuariosDto usuario, int? idGrupo);
        Task EditarUsuario(string id, EditarUsuarioRequest request);
        Task<IEnumerable<UsuariosDto>> ListarUsuarios();
        Task<UsuariosDto> ObtenerUsuarioPorId(string idUsuario);
    }
}
