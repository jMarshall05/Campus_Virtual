using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.DA;
using Abstracciones.Modelos.ModelosDA;
using Abstracciones.Modelos.ModelosDto;
using Microsoft.EntityFrameworkCore;

namespace DA
{
    public class EstudianteGrupoDA : IEstudianteGrupoDA
    {
        private readonly ApplicationDbContext _elContexto;
        public EstudianteGrupoDA(ApplicationDbContext contexto)
        {
            _elContexto = contexto;
        }
        public async Task<bool> ActualizarEstudianteGrupo(EstudianteGrupoAD estudiante)
        {
            EstudianteGrupoAD EstudianteGrupoEnBase = await _elContexto.EstudianteGrupos.FirstOrDefaultAsync(x => x.EstudianteId == estudiante.EstudianteId);
            if (EstudianteGrupoEnBase == null)
            {
                return false;
            }
            EstudianteGrupoEnBase.GrupoId = (int)estudiante.GrupoId;
            int resultado = await _elContexto.SaveChangesAsync();
            return resultado > 0;
        }

        public async Task<int> AgregarEstudianteGrupo(EstudianteGrupoAD estudianteGrupo)
        {
            await _elContexto.EstudianteGrupos.AddAsync(estudianteGrupo);
            int resultado = await _elContexto.SaveChangesAsync();
            if (resultado > 0)
            {
                return estudianteGrupo.IdEstudianteGrupo;
            }
            return 0;
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

        public async Task<List<EstudianteGrupoDto>> BuscarEstudianteGrupoPorGrupoId(int idGrupo)
        {
            var estudianteGrupo = await _elContexto.EstudianteGrupos.Where(eg => eg.GrupoId == idGrupo).Select(eg => new EstudianteGrupoDto
            {
                IdEstudianteGrupo = eg.IdEstudianteGrupo,
                EstudianteId = eg.EstudianteId,
                GrupoId = eg.GrupoId
            }).ToListAsync();
            return estudianteGrupo;
        }

        public async Task<List<EstudianteGrupoDto>> ListarEstudiantesGrupos()
        {
            var lista =await _elContexto.EstudianteGrupos.Select(eg => new EstudianteGrupoDto
            {
                IdEstudianteGrupo = eg.IdEstudianteGrupo,
                EstudianteId = eg.EstudianteId,
                GrupoId = eg.GrupoId
            }).ToListAsync();
            return lista;
        }

        public async Task<List<EstudianteGrupoDto>> ListarEstudiantesPorIdGrupo(int idGrupo)
        {
            var lista =await _elContexto.EstudianteGrupos.Where(eg => eg.GrupoId == idGrupo).Select(eg => new EstudianteGrupoDto
            {
                IdEstudianteGrupo = eg.IdEstudianteGrupo,
                EstudianteId = eg.EstudianteId,
                GrupoId = eg.GrupoId
            }).ToListAsync();
            return lista;
        }

        public async Task<List<EstudianteGrupoDto>> ListarGruposPorIdEstudiante(string idUsuario)
        {
            var lista =await _elContexto.EstudianteGrupos.Where(eg => eg.EstudianteId == idUsuario).Select(eg => new EstudianteGrupoDto
            {
                IdEstudianteGrupo = eg.IdEstudianteGrupo,
                EstudianteId = eg.EstudianteId,
                GrupoId = eg.GrupoId
            }).ToListAsync();
            return lista;
        }
    }
}
