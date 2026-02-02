using Abstracciones.Api;
using Abstracciones.Excepciones;
using Abstracciones.Modelos.Requests;
using Abstracciones.Servicios;
using DA;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static Abstracciones.Modelos.Requests.UsuariosRequests;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase, IAuthController
{
    private readonly TokenService _tokenService;
    private readonly IUsuariosService _usuarios;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        UserManager<ApplicationUser> userManager,
        TokenService tokenService,
        IUsuariosService usuarios,
        ILogger<AuthController> logger
        )
    {
        _tokenService = tokenService;
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
    [HttpPost("logOut")]
    public Task<IActionResult> Logout()
    {
        throw new NotImplementedException();
    }


}
