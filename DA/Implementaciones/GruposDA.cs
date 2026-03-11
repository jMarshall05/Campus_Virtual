using Abstracciones.Modelos.ModelosDto;
using DA.Entidades;
using DA.Interfaces;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DA.Implementaciones
{
    public class GruposDA : IGruposDA
    {
        private readonly ApplicationDbContext _elContexto;
        private readonly IMapper _mapper;
        public GruposDA(ApplicationDbContext elContexto, IMapper mapper)
        {
            _elContexto = elContexto;
            _mapper = mapper;
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
            grupoExistente.FechaDeModificacion = DateTime.Now;
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
