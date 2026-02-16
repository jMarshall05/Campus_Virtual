using Abstracciones.Modelos.ModelosDto;
using Abstracciones.Modelos.Requests;
using DA.Entidades;

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
        Task<TokenRequest> Login(LoginRequest login);
        Task<IEnumerable<UsuariosDto>> ListarPorRol(string rol);
        Task<TokenRequest> DisableAuthenticator(string IdUsuario);
        Task EnableAuthenticator(string IdUsuario, string googleKey);
        Task<TokenRequest> VerifyTwoFa(string IdUsuario);
        Task<bool> ObtenerEstadoUsuario(string IdUsuario);

    }
}
