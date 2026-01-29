using System.Globalization;
using System.Text;
using Abstracciones.Api;
using Abstracciones.Modelos.ModelosDto;
using Abstracciones.Servicios;
using DA;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase, IAuthController
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly TokenService _tokenService;
    private readonly IUsuariosService _usuarios;
    private readonly ITelefonosService _telefonos;

    public AuthController(
        UserManager<ApplicationUser> userManager,
        TokenService tokenService,
        IUsuariosService usuarios,
        ITelefonosService telefonos)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _usuarios = usuarios;
        _telefonos = telefonos;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
            return Unauthorized();

        if (!await _userManager.CheckPasswordAsync(user, request.Password))
            return Unauthorized();

        var token = await _tokenService.CrearToken(user);
        return Ok(new { token });
    }
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] Abstracciones.Modelos.Requests.UsuariosRequests.RegisterRequest register)
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
