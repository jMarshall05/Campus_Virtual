using static Abstracciones.Modelos.Responses.AuthResponses;

namespace Abstracciones.Servicios
{
    public interface ITokenService
    {
        string CrearToken(LoginResponse user);
    }
}
