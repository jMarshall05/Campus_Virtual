using Abstracciones.Modelos.ModelosDto;
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
