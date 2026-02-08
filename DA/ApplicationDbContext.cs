using System.Reflection.Emit;
using DA.Entidades;
using DA.Implementaciones;
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
        public DbSet<EstudianteGrupoAD> EstudianteGrupos { get; set; }
        public DbSet<GruposAD> Grupos { get; set; }
        public DbSet<MateriasAD> Materias { get; set; }
        public DbSet<EventoAD> Eventos { get; set; }
        public DbSet<EstudianteGrupoAD> EstudianteGrupo { get; set; }
        public DbSet<EntregasAD> Entregas { get; set; }
        public DbSet<TareasAD> Tareas { get; set; }
        public DbSet<CalificacionesAD> Calificaciones { get; set; }
        public DbSet<DocumentosAD> Documentos { get; set; }
        public DbSet<AnunciosAD> Anuncios { get; set; }
        public DbSet<CursosAD> Cursos { get; set; }
        public DbSet<BitacoraAD> Bitacora { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<TelefonoAD>().
                HasOne(t => t.Usuario).
                WithMany(t => t.Telefonos).
                HasForeignKey(t => t.IdUsuario);

            builder.Entity<EstudianteGrupoAD>()
                .HasOne(eg => eg.Estudiante)
                .WithOne(u => u.EstudianteGrupo)
                .HasForeignKey<EstudianteGrupoAD>(eg => eg.EstudianteId);

            builder.Entity<EstudianteGrupoAD>()
                .HasOne(eg => eg.Grupo)
                .WithMany(g => g.EstudianteGrupos)
                .HasForeignKey(eg => eg.GrupoId);

            builder.Entity<CalificacionesAD>()
               .Property(c => c.Calificacion)
               .HasPrecision(5, 2);


            builder.Entity<ApplicationUser>().ToTable("AspNetUsers_Core");
            builder.Entity<IdentityRole<string>>().ToTable("AspNetRoles_Core");
            builder.Entity<IdentityUserRole<string>>().ToTable("AspNetUserRoles_Core");
            builder.Entity<IdentityUserClaim<string>>().ToTable("AspNetUserClaims_Core");
            builder.Entity<IdentityUserLogin<string>>().ToTable("AspNetUserLogins_Core");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("AspNetRoleClaims_Core");
        }


    }
}
