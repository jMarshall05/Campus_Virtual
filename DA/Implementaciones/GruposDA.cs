using Abstracciones.Modelos.ModelosDto;
using DA.Entidades;
using DA.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DA.Implementaciones
{
    public class GruposDA : IGruposDA
    {
        private readonly ApplicationDbContext _elContexto;

        public GruposDA(ApplicationDbContext elContexto)
        {
            _elContexto = elContexto;
        }
        public async Task<int> AgregarGrupo(GruposAD grupo)
        {
            var entidad = await _elContexto.Grupos.AddAsync(grupo);
            await _elContexto.SaveChangesAsync();
            return entidad.Entity.idGrupo;
        }

        public async Task<GruposDto> BuscarGruposPorId(int idGrupo)
        {
            var grupo = await _elContexto.Grupos
                .Include(eg => eg.EstudianteGrupos)
                .ThenInclude(e => e.Estudiante).
                Where(g => g.idGrupo == idGrupo).Select(grupo => new GruposDto
                {
                    idGrupo = grupo.idGrupo,
                    Nombre = grupo.Nombre,
                    Descripcion = grupo.Descripcion,
                    Estudiantes = grupo.EstudianteGrupos.
                    Select(e => new UsuariosDto
                    {
                        IdUsuario = e.Estudiante.IdUsuario,
                        Nombre = e.Estudiante.Nombre,
                        Apellido = e.Estudiante.Apellido,
                        Email = e.Estudiante.Email,
                        Identificacion = e.Estudiante.Identificacion

                    }).ToList(),
                    creado_por = grupo.creado_por,
                    Estado = grupo.Estado,
                    FechaDeCreacion = grupo.FechaDeCreacion,
                    FechaDeModificacion = grupo.FechaDeModificacion,
                    modificado_por = grupo.modificado_por
                }).FirstOrDefaultAsync();
            return grupo;
        }

        public async Task EditarGrupo(GruposAD grupo)
        {
            var grupoExistente = await _elContexto.Grupos.FindAsync(grupo.idGrupo);
            grupoExistente.Nombre = grupo.Nombre;
            grupoExistente.Descripcion = grupo.Descripcion;
            grupoExistente.modificado_por = grupo.modificado_por;
            grupoExistente.FechaDeModificacion = DateTime.UtcNow;
            grupoExistente.Estado = grupo.Estado;
            await _elContexto.SaveChangesAsync();
        }

        public async Task<IEnumerable<GruposDto>> ListarGrupos()
        {
            var grupos = await _elContexto.Grupos
                .Include(g => g.EstudianteGrupos)
                .ThenInclude(eg => eg.Estudiante)
                .Select(grupo => new GruposDto
                {
                    idGrupo = grupo.idGrupo,
                    Nombre = grupo.Nombre,
                    Descripcion = grupo.Descripcion,
                    Estudiantes = grupo.EstudianteGrupos.
                    Select(e => new UsuariosDto
                    {
                        IdUsuario = e.Estudiante.IdUsuario,
                        Nombre = e.Estudiante.Nombre,
                        Apellido = e.Estudiante.Apellido,
                        Email = e.Estudiante.Email,
                        Identificacion = e.Estudiante.Identificacion

                    }).ToList(),
                    creado_por = grupo.creado_por,
                    Estado = grupo.Estado,
                    FechaDeCreacion = grupo.FechaDeCreacion,
                    FechaDeModificacion = grupo.FechaDeModificacion,
                    modificado_por = grupo.modificado_por
                }).ToListAsync();
            return grupos;
        }
    }
}
