using Abstracciones.Modelos.ModelosDto;
using Abstracciones.Servicios;
using DA.Entidades;
using DA.Interfaces;
using MapsterMapper;
using Reglas;
using static Abstracciones.Modelos.Requests.TareasRequests;

namespace Servicios.Servicios
{
    public class TareasService : ITareasService
    {
        private readonly ITareasAD _tareas;
        private readonly IMateriasService _materias;
        private readonly IGruposService _grupo;
        private readonly IMapper _mapper;

        public TareasService(IMapper mapper, ITareasAD tareas, IMateriasService materias, IGruposService grupo)
        {
            _tareas = tareas;
            _materias = materias;
            _grupo = grupo;
            _mapper = mapper;
        }
        public async Task<int> AgregarTarea(AgregarTareaRequest tarea)
        {
            var existeMateria = await _materias.ObtenerMateriaPorId(tarea.IdMateria) != null;
            MateriaReglas.SiExiste(existeMateria);
            var existeGrupo = await _grupo.BuscarGruposPorId(tarea.IdGrupo) != null;
            GruposReglas.ExisteGrupo(existeGrupo);
            var resultado = await _tareas.AgregarTarea(_mapper.Map<TareasAD>(tarea));
            return resultado;
        }

        public async Task CambiarEstadoTarea(int idTarea)
        {
            var existe = await ObtenerPorId(idTarea) != null;
            TareaReglas.ExisteTarea(existe);
            await _tareas.CambiarEstadoTarea(idTarea);
        }

        public async Task EditarTarea(int id, EditarTareaRequest tarea)
        {
            var existe = await ObtenerPorId(id) != null;
            TareaReglas.ExisteTarea(existe);
            await _tareas.EditarTarea(id, _mapper.Map<TareasAD>(tarea));
        }

        public async Task<IEnumerable<TareaDto>> ListarTareas()
        {
            var resultado = await _tareas.ListarTareas();
            return resultado;
        }

        public async Task<List<TareaDto>> ListarTareasPorGrupo(int IdGrupo)
        {
            var existeGrupo = await _grupo.BuscarGruposPorId(IdGrupo) != null;
            GruposReglas.ExisteGrupo(existeGrupo);
            var resultado = await _tareas.ListarTareasPorGrupo(IdGrupo);
            return resultado;
        }

        public async Task<TareaDto> ObtenerPorId(int idTarea)
        {
            var resultado = await _tareas.ObtenerPorId(idTarea);
            return resultado;
        }

    }
}
