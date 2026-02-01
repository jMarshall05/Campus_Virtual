using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Abstracciones.Modelos.Responses.AuthResponses;

namespace Abstracciones.Servicios
{
    public interface ITokenService
    {
        string CrearToken(LoginResponse user);
    }
}
