using Abstracciones.Api;
using Abstracciones.Modelos.Requests;
using Abstracciones.Servicios;
using DA;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static Abstracciones.Modelos.Requests.UsuariosRequests;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase, IAuthController
    {
        private readonly IUsuariosService _usuarios;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            IUsuariosService usuarios
            )
        {
            _usuarios = usuarios;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var token = await _usuarios.Login(request);
            if (token == null)
                return Unauthorized("Usuario o contraseña incorrectos");
            return Ok(new { token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest register)
        {
            var result = await _usuarios.AgregarUsuario(register);
            if (string.IsNullOrEmpty(result))
                return BadRequest("Algo ha fallado");
            return Ok(new { id = result, mensaje = "Usuario agregado con éxito" });
        }

        [HttpPost("2fa/disable")]
        public async Task<IActionResult> DisableAuthenticator(string IdUsuario)
        {
            var token = await _usuarios.DisableAuthenticator(IdUsuario);
            return Ok(new { token });
        }

        [HttpPost("2fa/enable")]
        public async Task<IActionResult> EnableAuthenticator(string IdUsuario)
        {
            var response = await _usuarios.EnableAuthenticator(IdUsuario);
            return Ok(response);
        }

        [HttpPost("2fa/verify")]
        public async Task<IActionResult> VerifyTwoFa(string IdUsuario, string code)
        {
            var token = await _usuarios.VerifyTwoFa(IdUsuario, code);
            return Ok(new { token });
        }

        [HttpPost("2fa/login")]
        public async Task<IActionResult> Login2fa(string idUsuario, string code)
        {
            var token = await _usuarios.Login2fa(idUsuario, code);
            return Ok(new { token });
        }
    }
}