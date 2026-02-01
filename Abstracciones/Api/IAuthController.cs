using Abstracciones.Modelos.Requests;
using Microsoft.AspNetCore.Mvc;
using static Abstracciones.Modelos.Requests.UsuariosRequests;

namespace Abstracciones.Api
{
    public interface IAuthController
    {
        Task<IActionResult> Login(LoginRequest request);
        Task<IActionResult> Register(RegisterRequest register);
        Task<IActionResult> Logout();
    }
}
