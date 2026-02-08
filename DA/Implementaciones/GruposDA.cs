using Abstracciones.Modelos.ModelosDto;
using DA.Entidades;
using DA.Interfaces;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

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
            return entidad.Entity.id_grupo;
        }

        public async Task<GruposDto> BuscarGruposPorId(int idGrupo)
        {
            var grupo = _mapper.Map<GruposDto>(await _elContexto.Grupos.FindAsync(idGrupo)) ?? null;
            return grupo;
        }

        public async Task EditarGrupo( GruposAD grupo)
        {
            var grupoExistente = await _elContexto.Grupos.FindAsync(grupo.id_grupo);
            grupoExistente.Nombre = grupo.Nombre;
            grupoExistente.Descripcion = grupo.Descripcion;
            grupoExistente.modificado_por = grupo.modificado_por;
            grupoExistente.FechaDeModificacion = DateTime.Now;
            grupoExistente.estado = grupo.estado;
            await _elContexto.SaveChangesAsync();
        }

        public async Task<IEnumerable<GruposDto>> ListarGrupos()
        {
            var grupos = await _elContexto.Grupos.Select(grupo => new GruposDto
            {
                id_grupo = grupo.id_grupo,
                Nombre = grupo.Nombre,
                Descripcion = grupo.Descripcion,
                creado_por = grupo.creado_por,
                estado = grupo.estado,
                FechaDeCreacion = grupo.FechaDeCreacion,
                FechaDeModificacion = grupo.FechaDeModificacion,
                modificado_por = grupo.modificado_por
            }).ToListAsync();
            return grupos;
        }
    }
}
