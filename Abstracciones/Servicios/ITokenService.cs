using Abstracciones.Modelos.Requests;

namespace Abstracciones.Servicios
{
    public interface ITokenService
    {
        string CrearToken(TokenRequest data);
    }
}
