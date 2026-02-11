using Abstracciones.Modelos.Requests;
using Microsoft.AspNetCore.Mvc;
using static Abstracciones.Modelos.Requests.UsuariosRequests;

namespace Abstracciones.Api
{
    public interface IAuthController
    {
        Task<IActionResult> Login(LoginRequest request);
        Task<IActionResult> Register(RegisterRequest register);
        Task<IActionResult> DisableAuthenticator(string IdUsuario);
        Task<IActionResult> EnableAuthenticator(string IdUsuario);
        Task<IActionResult> VerifyTwoFa(string IdUsuario,string code);

    }
}
