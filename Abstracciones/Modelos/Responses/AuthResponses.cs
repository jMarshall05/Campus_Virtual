namespace Abstracciones.Modelos.Responses
{
    public class AuthResponses
    {
        public class LoginResponse
        {
            public string Id { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
            public string Rol { get; set; }
            public bool Estado { get; set; }
            public bool TwoFactorEnabled { get; set; }
        }
        public class TwofaResponse
        {
            public string? SecretKey { get; set; }
            public string? Qr { get; set; }
        }
    }
}
