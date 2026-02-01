using Abstracciones.Api;
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

    public AuthController(
        UserManager<ApplicationUser> userManager,
        TokenService tokenService,
        IUsuariosService usuarios
        )
    {
        _tokenService = tokenService;
        _usuarios = usuarios;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var token = await _usuarios.Login(request);
        if (token == null)
            return Unauthorized();

        return Ok(new { token });
    }
    [HttpPost("Register")]
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
    [HttpPost("LogOut")]
    public Task<IActionResult> Logout()
    {
        throw new NotImplementedException();
    }


}
