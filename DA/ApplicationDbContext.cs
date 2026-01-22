using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.Modelos.ModelosDA;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DA
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<UsuariosAD> Usuarios { get; set; }
        public DbSet<TelefonoAD> Telefonos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>().ToTable("AspNetUsers_Core");
            builder.Entity<IdentityRole<string>>().ToTable("AspNetRoles_Core");
            builder.Entity<IdentityUserRole<string>>().ToTable("AspNetUserRoles_Core");
            builder.Entity<IdentityUserClaim<string>>().ToTable("AspNetUserClaims_Core");
            builder.Entity<IdentityUserLogin<string>>().ToTable("AspNetUserLogins_Core");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("AspNetRoleClaims_Core");
        }


    }
}
