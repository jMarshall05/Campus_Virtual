using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.DA;
using Abstracciones.Excepciones;
using Abstracciones.Modelos.ModelosDA;
using Abstracciones.Modelos.ModelosDto;
using Abstracciones.Servicios;
using Reglas;

namespace Servicios
{
    public class TareasService : ITareasService
    {
        private readonly ITareasAD _tareas;
        private readonly IMateriasDA _materias;
        private readonly IGruposDA _grupo;
        private readonly IUsuariosDA _usuarios;

        public TareasService(ITareasAD tareas, IMateriasDA materias, IGruposDA grupo, IUsuariosDA usuarios)
        {
            _tareas = tareas;
            _materias = materias;
            _grupo = grupo;
            _usuarios = usuarios;
        }
        public Task<int> AgregarTarea(TareasAD tarea)
        {
            var existeMateria = _materias.ObtenerMateriaPorId(tarea.id_materia) != null;
            TareaReglas.ExisteMateria(existeMateria);
            var existeGrupo = _grupo.BuscarGruposPorId(tarea.IdGrupo) != null;
            if (!existeGrupo)
            {
                throw new BusinessException("El grupo asignado no existe.");
            }
            var resultado = _tareas.AgregarTarea(tarea);
            return resultado;
        }

        public async Task CambiarEstadoTarea(int idTarea)
        {
            await _tareas.CambiarEstadoTarea(idTarea);
        }

        public async Task EditarTarea(int id, TareasAD tarea)
        {
            await _tareas.EditarTarea(id, tarea);
        }

        public async Task<IEnumerable<TareaDto>> ListarTareas()
        {
            var resultado = await _tareas.ListarTareas();
            return resultado;
        }

        public Task<List<TareaDto>> ListarTareasPorEstudiante(EstudianteGrupoDto estudianteGrupo)
        {
            var existeEstudiante = _usuarios.ObtenerUsuarioPorId(estudianteGrupo.EstudianteId) != null;
            if (!existeEstudiante)
            {
                throw new BusinessException("El estudiante no existe.");
            }
            var existeGrupo = _grupo.BuscarGruposPorId((int)estudianteGrupo.GrupoId) != null;
            if (!existeGrupo)
            {
                throw new BusinessException("El grupo no existe.");
            }
            var resultado = _tareas.ListarTareasPorEstudiante(estudianteGrupo);
            return resultado;
        }

        public Task<TareaDto> ObtenerPorId(int idTarea)
        {
            var resultado = _tareas.ObtenerPorId(idTarea);
            return resultado;
        }
    }
}
