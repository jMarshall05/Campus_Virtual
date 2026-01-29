using Abstracciones.Excepciones;
using Abstracciones.Modelos.ModelosDto;
using Abstracciones.Servicios;
using AutoMapper;
using DA;
using DA.Entidades;
using DA.Interfaces;
using Microsoft.AspNetCore.Identity;
using Reglas;
using static Abstracciones.Modelos.Requests.TareasRequests;

namespace Servicios.Servicios
{
    public class TareasService : ITareasService
    {
        private readonly ITareasAD _tareas;
        private readonly IMateriasDA _materias;
        private readonly IGruposDA _grupo;
        private readonly IUsuariosDA _usuarios;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public TareasService(IMapper mapper, UserManager<ApplicationUser> usermanager, ITareasAD tareas, IMateriasDA materias, IGruposDA grupo, IUsuariosDA usuarios)
        {
            _tareas = tareas;
            _materias = materias;
            _grupo = grupo;
            _usuarios = usuarios;
            _userManager = usermanager;
            _mapper = mapper;
        }
        public async Task<int> AgregarTarea(AgregarTareaRequest tarea)
        {
            var existeMateria = await _materias.ObtenerMateriaPorId(tarea.IdMateria) != null;
            MateriaReglas.ExisteMateria(existeMateria);
            var existeGrupo = await _grupo.BuscarGruposPorId(tarea.IdGrupo) != null;
            if (!existeGrupo)
            {
                throw new BusinessException("El grupo asignado no existe.");
            }
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

        public async Task<List<TareaDto>> ListarTareasPorEstudiante(EstudianteGrupoDto estudianteGrupo)
        {
            var existeEstudiante = await _userManager.FindByIdAsync(estudianteGrupo.EstudianteId) != null;
            if (!existeEstudiante)
            {
                throw new BusinessException("El estudiante no existe.");
            }
            var existeGrupo = await _grupo.BuscarGruposPorId((int)estudianteGrupo.GrupoId) != null;
            if (!existeGrupo)
            {
                throw new BusinessException("El grupo no existe.");
            }
            var resultado = await _tareas.ListarTareasPorEstudiante(estudianteGrupo);
            return resultado;
        }

        public async Task<TareaDto> ObtenerPorId(int idTarea)
        {
            var resultado = await _tareas.ObtenerPorId(idTarea);
            return resultado;
        }

    }
}
