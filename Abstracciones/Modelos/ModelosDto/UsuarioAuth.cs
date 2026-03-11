namespace Abstracciones.Modelos.ModelosDto
{
    public class UsuarioAuth
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Rol { get; set; }
        public string? GoogleAuthenticatorSecretTemp { get; set; }
        public string? GoogleAuthenticatorSecretKey { get; set; }
        public bool TwoFactorEnabled { get; set; }


    }
}
