using Abstracciones.Modelos.ModelosDto;
using Abstracciones.Modelos.Requests;
using static Abstracciones.Modelos.Requests.UsuariosRequests;
using static Abstracciones.Modelos.Responses.AuthResponses;

namespace Abstracciones.Servicios
{
    public interface IUsuariosService
    {
        Task<string> AgregarUsuario(RegisterRequest request);
        Task EditarUsuarioAdmin(string id, EditarUsuarioAdminRequest usuario, int? idGrupo);
        Task EditarUsuario(string id, EditarUsuarioRequest request);
        Task<IEnumerable<UsuariosDto>> ListarUsuarios();
        Task<UsuariosDto> ObtenerUsuarioPorId(string idUsuario);
        Task<string> Login(LoginRequest login);
        Task<IEnumerable<UsuariosDto>> ListarPorRol(string rol);

    }
}
