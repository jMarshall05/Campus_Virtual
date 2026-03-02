using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.Modelos.ModelosDto;
using Abstracciones.Servicios;
using DA.Entidades;
using DA.Interfaces;
using Mapster;
using MapsterMapper;

namespace Servicios.Servicios
{
    public class CursosService : ICursosService
    {
        private readonly ICursosAD _cursos;
        private readonly IMapper _mapper;
        public CursosService(ICursosAD cursos, IMapper mapper)
        {
            _cursos = cursos;
            _mapper = mapper;
        }
        public async Task<int> AgregarCurso(CursoDto curso)
        {
            var request =await _cursos.AgregarCurso(curso.Adapt<CursosAD>());
            return request;
        }

        public Task<IEnumerable<CursoDto>> ListarCursos()
        {
            throw new NotImplementedException();
        }

        public Task ModificarEstadoCurso(int idCurso)
        {
            throw new NotImplementedException();
        }

        public Task<CursoDto> ObtenerPorId(int idCurso)
        {
            throw new NotImplementedException();
        }
    }
}
