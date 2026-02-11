using Abstracciones.Api;
using Abstracciones.Excepciones;
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
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            IUsuariosService usuarios,
            ILogger<AuthController> logger
            )
        {
            _usuarios = usuarios;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {

                var token = await _usuarios.Login(request);
                if (token == null)
                    return Unauthorized("Usuario o contraseña incorrectos");

                return Ok(new { token });
            }
            catch (BusinessException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al hacer login");
                return StatusCode(500, "Ocurrió un error inesperado");
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest register)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }
            var result = await _usuarios.AgregarUsuario(register);
            if (string.IsNullOrEmpty(result))
            {
                return BadRequest("Algo ha fallado");
            }
            return Ok($"Usuarios agregado con exito Id : {result}");

        }
        [HttpPost("2fa/disable")]
        public async Task<IActionResult> DisableAuthenticator(string IdUsuario)
        {
            try
            {
                var token = await _usuarios.DisableAuthenticator(IdUsuario);
                return Ok(new{ token });
            }
            catch (BusinessException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al desactivar 2FA");
                return StatusCode(500, "Ocurrió un error inesperado");
            }
        }
        [HttpPost("2fa/enable")]
        public async Task<IActionResult> EnableAuthenticator(string IdUsuario)
        {
            try
            {
                var response = await _usuarios.EnableAuthenticator(IdUsuario);
                return Ok(response);
            }
            catch (BusinessException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al habilitar 2FA");
                return StatusCode(500, "Ocurrió un error inesperado");
            }
        }
        [HttpPost("2fa/verify")]
        public async Task<IActionResult> VerifyTwoFa(string IdUsuario, string code)
        {
            try
            {
                var token = await _usuarios.VerifyTwoFa(IdUsuario, code);
                return Ok(new { token });
            }
            catch (BusinessException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al habilitar 2FA");
                return StatusCode(500, "Ocurrió un error inesperado");
            }
        }
    }
}