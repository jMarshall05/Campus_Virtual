using Abstracciones.Modelos.ModelosDto;
using Abstracciones.Modelos.Requests;
using Abstracciones.Servicios;
using DA.Entidades;
using DA.Interfaces;
using Mapster;
using MapsterMapper;
using Reglas;

namespace Servicios.Servicios
{
    public class MateriasService : IMateriasService
    {
        private readonly IMateriasDA _materias;
        private readonly IMapper _mapper;

        public MateriasService(IMapper mapper, IMateriasDA materias)
        {
            _mapper = mapper;
            _materias = materias;
        }

        public async Task<int> AgregarMateria(MateriaRequests materia)
        {
            var existe = await ObtenerMateriaNombre(materia.Nombre) != null;
            MateriaReglas.SiExiste(existe);
            var resultado = await _materias.AgregarMateria(materia.Adapt<MateriasAD>());
            return resultado;
        }

        public async Task CambiarEstadoMateria(int materiaId)
        {
            var existe = await ObtenerMateriaPorId(materiaId) != null;
            MateriaReglas.NoExiste(existe);
            await _materias.CambiarEstadoMateria(materiaId);
        }

        public async Task EditarMateria(int IdMateria, MateriaRequests materia)
        {
            var existe = await ObtenerMateriaPorId(IdMateria) != null;
            MateriaReglas.NoExiste(existe);
            await _materias.EditarMateria(IdMateria, materia.Adapt<MateriasAD>());
        }

        public async Task<IEnumerable<MateriaDto>> ListarMaterias()
        {
            var materias = await _materias.ListarMaterias();
            return materias;
        }

        public async Task<MateriaDto> ObtenerMateriaNombre(string nombre)
        {
            var materia = await _materias.ObtenerMateriaPorNombre(nombre);
            return materia;
        }
        public async Task<MateriaDto> ObtenerMateriaPorId(int id)
        {
            var materia = await _materias.ObtenerMateriaPorId(id);
            return materia;
        }
    }
}