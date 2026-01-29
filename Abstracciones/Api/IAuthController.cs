using Abstracciones.Modelos.ModelosDto;
using Microsoft.AspNetCore.Mvc;
using static Abstracciones.Modelos.Requests.UsuariosRequests;

namespace Abstracciones.Api
{
    public interface IAuthController
    {
        Task<IActionResult> Login(LoginRequestDto request);
        Task<IActionResult> Register(RegisterRequest register);
        Task<IActionResult> Logout();
    }
}
