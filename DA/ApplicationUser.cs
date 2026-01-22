using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace DA
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime FechaDeRegistro { get; set; }
    }
}
