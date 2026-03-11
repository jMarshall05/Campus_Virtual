using Abstracciones.Modelos.ModelosDto;
using DA.Entidades;
using DA.Interfaces;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace DA.Implementaciones
{
    public class EstudianteGrupoDA : IEstudianteGrupoDA
    {
        private readonly ApplicationDbContext _elContexto;
        public EstudianteGrupoDA(ApplicationDbContext contexto)
        {
            _elContexto = contexto;
        }
        public async Task ActualizarEstudianteGrupo(EstudianteGrupoAD estudiante)
        {
            var EstudianteGrupoEnBase = BuscarEstudianteGrupoPorEstudianteId(estudiante.EstudianteId).Adapt<EstudianteGrupoAD>();
            EstudianteGrupoEnBase.GrupoId = estudiante.GrupoId;
            await _elContexto.SaveChangesAsync();
        }

        public async Task<int> AgregarEstudianteGrupo(EstudianteGrupoAD estudianteGrupo)
        {
            var entidad = await _elContexto.EstudianteGrupos.AddAsync(estudianteGrupo);
            await _elContexto.SaveChangesAsync();
            return entidad.Entity.IdEstudianteGrupo;
        }

        public async Task<EstudianteGrupoDto> BuscarEstudianteGrupoPorEstudianteId(string idEstudiante)
        {
            var estudianteGrupo = await _elContexto.EstudianteGrupos.Where(eg => eg.EstudianteId == idEstudiante).Select(eg => new EstudianteGrupoDto
            {
                IdEstudianteGrupo = eg.IdEstudianteGrupo,
                EstudianteId = eg.EstudianteId,
                GrupoId = eg.GrupoId
            }).FirstOrDefaultAsync();
            return estudianteGrupo;
        }

        public async Task<IEnumerable<EstudianteGrupoDto>> BuscarEstudianteGrupoPorGrupoId(int idGrupo)
        {
            var estudianteGrupo = await _elContexto.EstudianteGrupos.Where(eg => eg.GrupoId == idGrupo).Select(eg => new EstudianteGrupoDto
            {
                IdEstudianteGrupo = eg.IdEstudianteGrupo,
                EstudianteId = eg.EstudianteId,
                GrupoId = eg.GrupoId
            }).ToListAsync();
            return estudianteGrupo;
        }

        public async Task<IEnumerable<EstudianteGrupoDto>> ListarEstudiantesGrupos()
        {
            var lista = await _elContexto.EstudianteGrupos.Select(eg => new EstudianteGrupoDto
            {
                IdEstudianteGrupo = eg.IdEstudianteGrupo,
                EstudianteId = eg.EstudianteId,
                GrupoId = eg.GrupoId
            }).ToListAsync();
            return lista;
        }

        public async Task<IEnumerable<EstudianteGrupoDto>> ListarEstudiantesPorIdGrupo(int idGrupo)
        {
            var lista = await _elContexto.EstudianteGrupos.Where(eg => eg.GrupoId == idGrupo).Select(eg => new EstudianteGrupoDto
            {
                IdEstudianteGrupo = eg.IdEstudianteGrupo,
                EstudianteId = eg.EstudianteId,
                GrupoId = eg.GrupoId
            }).ToListAsync();
            return lista;
        }

        public async Task<IEnumerable<EstudianteGrupoDto>> ListarGruposPorIdEstudiante(string idUsuario)
        {
            var lista = await _elContexto.EstudianteGrupos.Where(eg => eg.EstudianteId == idUsuario)
                .Select(eg => new EstudianteGrupoDto
                {
                    IdEstudianteGrupo = eg.IdEstudianteGrupo,
                    EstudianteId = eg.EstudianteId,
                    GrupoId = eg.GrupoId
                }).ToListAsync();
            return lista;
        }

    }
}
