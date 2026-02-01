using Abstracciones.Modelos.ModelosDto;
using Abstracciones.Modelos.Requests;
using DA.Entidades;
using static Abstracciones.Modelos.Responses.AuthResponses;

namespace DA.Interfaces
{
    public interface IUsuariosDA
    {
        Task<string> AgregarUsuario(UsuariosAD usuario, string password);
        Task EditarUsuarioAdmin(string id, UsuariosAD usuario);
        Task EditarUsuario(string id, UsuariosAD usuario);
        Task<IEnumerable<UsuariosDto>> ListarUsuarios();
        Task<UsuariosDto> ObtenerUsuarioPorId(string idUsuario);
        Task<bool> ExisteIdentificacion(string identificacion);
        Task<UsuarioAuth> ObtenerUsuarioIdentityPorId(string Id);
        Task<UsuarioAuth> ObtenerUsuarioIdentityPorEmail(String Email);
        Task AsignarRol(string userId, string rol);
        Task<LoginResponse> Login(LoginRequest login);


    }
}
