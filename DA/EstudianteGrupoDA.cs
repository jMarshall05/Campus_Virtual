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
        public Task<int> ActualizarEstudianteGrupo(EstudianteGrupoAD estudiante)
        {
            throw new NotImplementedException();
        }

        public async Task<int> AgregarEstudianteGrupo(EstudianteGrupoAD estudianteGrupo)
        {
            await _elContexto.EstudianteGrupos.AddAsync(estudianteGrupo);
            int resultado = await _elContexto.SaveChangesAsync();
            if (resultado > 0)
            {
                return estudianteGrupo.IdEstudianteGrupo;
            }
            return resultado;
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

        public Task<List<EstudianteGrupoDto>> BuscarEstudianteGrupoPorGrupoId(int idGrupo)
        {
            throw new NotImplementedException();
        }

        public Task<List<EstudianteGrupoDto>> ListarEstudiantesGrupos()
        {
            throw new NotImplementedException();
        }

        public Task<List<EstudianteGrupoDto>> ListarEstudiantesPorIdGrupo(int idGrupo)
        {
            throw new NotImplementedException();
        }

        public Task<List<EstudianteGrupoDto>> ListarGruposPorIdEstudiante(string idUsuario)
        {
            throw new NotImplementedException();
        }
    }
}
