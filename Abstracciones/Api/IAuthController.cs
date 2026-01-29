using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.Modelos.ModelosDto;
using Abstracciones.Modelos.Requests;
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
