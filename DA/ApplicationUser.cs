using Microsoft.AspNetCore.Identity;

namespace DA
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime FechaDeRegistro { get; set; }
        public string? GoogleAuthenticatorSecretKey { get; set; }
        public string? GoogleAuthenticatorSecretTemp { get; set; }

    }
}
