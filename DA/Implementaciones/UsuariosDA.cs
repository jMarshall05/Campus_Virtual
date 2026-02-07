using System.Globalization;
using System.Text;
using Abstracciones.Excepciones;
using Abstracciones.Modelos.ModelosDto;
using Abstracciones.Modelos.Requests;
using DA.Entidades;
using DA.Interfaces;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static Abstracciones.Modelos.Responses.AuthResponses;

namespace DA.Implementaciones
{
    public class UsuariosDA : IUsuariosDA
    {
        private readonly ApplicationDbContext _elContexto;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<string>> _roleManager;
        private readonly IMapper _mapper;
        public UsuariosDA(RoleManager<IdentityRole<string>> roleManager, IMapper mapper, ApplicationDbContext Contexto, UserManager<ApplicationUser> userManager)
        {
            _elContexto = Contexto;
            _userManager = userManager;
            _mapper = mapper;
            _roleManager = roleManager;
        }

        public async Task<LoginResponse> Login(LoginRequest login)
        {
            var user = await _userManager.FindByEmailAsync(login.Email) ?? throw new BusinessException("Usuario incorrecto");
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, login.Password);
            if (!isPasswordValid)
                throw new BusinessException("Usuario o contraseña incorrectos");
            var respuesta = user.Adapt<LoginResponse>();
            respuesta.Rol = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
            return respuesta;
        }

        public async Task<string> AgregarUsuario(UsuariosAD usuario, string password)
        {
            var user = CrearUsuario(usuario);
            var resultado = await _userManager.CreateAsync(user, password);
            if (!resultado.Succeeded)
                throw new BusinessException(resultado.Errors.First().Description);
            await AsignarRol(user.Id, usuario.Rol);
            usuario.FechaDeRegistro = DateTime.UtcNow;
            var entidad = await _elContexto.Usuarios.AddAsync(usuario);

            await _elContexto.SaveChangesAsync();

            return entidad.Entity.IdUsuario;
        }

        public async Task EditarUsuario(string id, UsuariosAD usuario)
        {
            var usuarioExistente = await _elContexto.Usuarios.FindAsync(id);
            usuarioExistente.Nombre = usuario.Nombre;
            usuarioExistente.Apellido = usuario.Apellido;
            usuarioExistente.FechaDeModificacion = DateTime.Now;
            await _elContexto.SaveChangesAsync();

        }

        public async Task EditarUsuarioAdmin(string Id, UsuariosAD usuario)
        {
            var usuarioExistente = await _elContexto.Usuarios.FindAsync(Id);

            if (usuarioExistente == null)
                throw new BusinessException("Usuario no encontrado");

            usuarioExistente.Nombre = usuario.Nombre;
            usuarioExistente.Apellido = usuario.Apellido;
            usuarioExistente.FechaDeNacimiento = usuario.FechaDeNacimiento;
            usuarioExistente.Identificacion = usuario.Identificacion;
            usuarioExistente.Estado = usuario.Estado;
            usuarioExistente.TipoIdentificacion = usuario.TipoIdentificacion;
            usuarioExistente.FechaDeModificacion = DateTime.UtcNow;

            if (usuarioExistente.Email != usuario.Email)
            {
                usuarioExistente.Email = usuario.Email;
                await CambiarCorreo(Id, usuario.Email);
            }
            if (usuarioExistente.Rol != usuario.Rol)
            {
                usuarioExistente.Rol = usuario.Rol;
                await AsignarRol(Id, usuario.Rol);
            }

            await _elContexto.SaveChangesAsync();
        }

        public async Task CambiarCorreo(string idUsuario, string nuevoCorreo)
        {
            var user = await _userManager.FindByIdAsync(idUsuario);
            await _userManager.SetEmailAsync(user, nuevoCorreo);
        }

        public async Task<bool> ExisteIdentificacion(string identificacion)
        {
            var existe = await _elContexto.Usuarios.AnyAsync(u => u.Identificacion == identificacion);
            return existe;
        }

        public async Task<IEnumerable<UsuariosDto>> ListarUsuarios()
        {
            var usuariosDto = await _elContexto.Usuarios
                .Select(u => new UsuariosDto
                {
                    IdUsuario = u.IdUsuario,
                    Nombre = u.Nombre,
                    Apellido = u.Apellido,
                    Email = u.Email,
                    Telefonos = u.Telefonos.Select(t => new TelefonoDto
                    {
                        Id = t.Id,
                        IdUsuario = t.IdUsuario,
                        Codigo = t.Codigo,
                        Telefono = t.Telefono,
                        Tipo = t.Tipo,
                        Estado = t.Estado
                    }).ToList(),
                    FechaDeNacimiento = u.FechaDeNacimiento,
                    Identificacion = u.Identificacion,
                    FechaDeRegistro = u.FechaDeRegistro,
                    FechaDeModificacion = u.FechaDeModificacion,
                    Rol = u.Rol,
                    Estado = u.Estado
                })
                .ToListAsync();
            return usuariosDto;
        }

        public async Task<UsuariosDto> ObtenerUsuarioPorId(string idUsuario)
        {
            var usuario = await _elContexto.Usuarios
                .Include(u => u.Telefonos)
                .Include(u => u.EstudianteGrupo)
                .FirstOrDefaultAsync(u => u.IdUsuario == idUsuario);
            if (usuario == null)
                return null;
            var usuarioDto = new UsuariosDto
            {
                IdUsuario = usuario.IdUsuario,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Email = usuario.Email,
                Telefonos = usuario.Telefonos.Select(t => new TelefonoDto
                {
                    Id = t.Id,
                    IdUsuario = t.IdUsuario,
                    Codigo = t.Codigo,
                    Telefono = t.Telefono,
                    Tipo = t.Tipo,
                    Estado = t.Estado
                }).ToList(),
                Grupo = _elContexto.Grupos
                .Where(u => u.id_grupo == usuario.EstudianteGrupo.GrupoId)
                .Select(u => new GruposDto
                {
                    id_grupo = u.id_grupo,
                    nombre_grupo = u.nombre_grupo

                }).FirstOrDefault(),
                FechaDeNacimiento = usuario.FechaDeNacimiento,
                Identificacion = usuario.Identificacion,
                FechaDeRegistro = usuario.FechaDeRegistro,
                FechaDeModificacion = usuario.FechaDeModificacion,
                Rol = usuario.Rol,
                Estado = usuario.Estado
            };
            return usuarioDto;
        }
        private static ApplicationUser CrearUsuario(UsuariosAD usuario)
        {
            string numeroRamdon = Random.Shared.Next(0, 100).ToString("D2");

            return new ApplicationUser
            {
                UserName = (usuario.Nombre.ToUpper().First() + usuario.Apellido.Trim() + numeroRamdon).Normalize(NormalizationForm.FormD)
                .Where(c => char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                .Aggregate("", (s, c) => s + c),
                Email = usuario.Email,
                FechaDeRegistro = DateTime.UtcNow
            };
        }

        public async Task<UsuarioAuth> ObtenerUsuarioIdentityPorId(string Id)
        {
            var usuario = await _userManager.FindByIdAsync(Id);
            if (usuario == null)
                return null;

            var roles = await _userManager.GetRolesAsync(usuario);
            var rol = roles.FirstOrDefault();

            var usuarioAuth = usuario.Adapt<UsuarioAuth>();
            usuarioAuth.Rol = rol;

            return usuarioAuth;

        }
        public async Task<UsuarioAuth> ObtenerUsuarioIdentityPorEmail(string Email)
        {
            var usuario = await _userManager.FindByEmailAsync(Email);
            if (usuario == null)
                return null;

            var roles = await _userManager.GetRolesAsync(usuario);
            var rol = roles.FirstOrDefault();

            var usuarioAuth = usuario.Adapt<UsuarioAuth>();
            usuarioAuth.Rol = rol;

            return usuarioAuth;
        }

        public async Task AsignarRol(string userId, string rol)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new InvalidOperationException("Usuario no encontrado");

            var rolesActuales = await _userManager.GetRolesAsync(user);

            if (rolesActuales.Contains(rol))
                return;

            if (rolesActuales.Any())
                await _userManager.RemoveFromRolesAsync(user, rolesActuales);

            await _userManager.AddToRoleAsync(user, rol);
        }

        public async Task<IEnumerable<UsuariosDto>> ListarPorRol(string rol)
        {
            var roles = _roleManager.Roles.Select(r => r.Name).ToList();
            if (!roles.Contains(rol))
                throw new BusinessException("El rol especificado no existe");
            var usuarios = (await _elContexto.Usuarios.Where(u => u.Rol == rol).ToListAsync()).Adapt<IEnumerable<UsuariosDto>>();
            return usuarios;
        }
    }
}