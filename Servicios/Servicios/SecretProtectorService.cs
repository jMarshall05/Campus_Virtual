using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;

namespace Servicios.Servicios
{
    public class SecretProtectorService
    {
        private readonly IDataProtector _protector;
        public SecretProtectorService(IDataProtectionProvider provider)
        {
            _protector = provider.CreateProtector("MyApp.2FA.Secrets");
        }
        public string Protect(string secret)
      => _protector.Protect(secret);

        public string Unprotect(string protectedSecret)
            => _protector.Unprotect(protectedSecret);
    }
}

