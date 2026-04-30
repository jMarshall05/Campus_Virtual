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

        public async Task<IEnumerable<CursoDto>> ListarCursos()
        {
           var list =await _cursos.ListarCursos();
            return list;
        }

        public async Task ModificarEstadoCurso(int idCurso)
        {
           await _cursos.ModificarEstadoCurso(idCurso);
        }

        public Task<CursoDto> ObtenerPorId(int idCurso)
        {
           var curso = _cursos.ObtenerPorId(idCurso);
            return curso;
        }
    }
}
