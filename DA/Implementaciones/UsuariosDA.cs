using Abstracciones.Modelos.ModelosDto;
using DA.Entidades;
using DA.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DA.Implementaciones
{
    public class UsuariosDA : IUsuariosDA
    {
        private readonly ApplicationDbContext _elContexto;
        public UsuariosDA(ApplicationDbContext Contexto)
        {
            _elContexto = Contexto;
        }

        public async Task<string> AgregarUsuario(UsuariosAD usuario)
        {

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

        public async Task EditarUsuarioAdmin(string id, UsuariosAD usuario)
        {

            var usuarioExistente = await _elContexto.Usuarios.FindAsync(usuario);
            usuarioExistente.Nombre = usuario.Nombre;
            usuarioExistente.Apellido = usuario.Apellido;
            usuarioExistente.Email = usuario.Email;
            usuarioExistente.FechaDeNacimiento = usuario.FechaDeNacimiento;
            usuarioExistente.FechaDeModificacion = DateTime.Now;
            usuarioExistente.Rol = usuario.Rol;
            usuarioExistente.Identificacion = usuario.Identificacion;
            usuarioExistente.Estado = usuario.Estado;
            usuarioExistente.TipoIdentificacion = usuario.TipoIdentificacion;
            await _elContexto.SaveChangesAsync();


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
    }
}