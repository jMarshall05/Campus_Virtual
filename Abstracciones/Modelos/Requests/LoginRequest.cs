using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Abstracciones.Modelos.Responses.AuthResponses;

namespace Abstracciones.Modelos.Requests
{
    public class LoginRequest
    {
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [PasswordPropertyText]
        public string Password { get; set; } = string.Empty;
    }

    public class TokenRequest : LoginResponse
    {

    }
}
