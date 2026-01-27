using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.DA;
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
        public TareasService(ITareasAD tareas, IMateriasDA materias)
        {
            _tareas = tareas;
            _materias = materias;
        }
        public Task<int> AgregarTarea(TareasAD tarea)
        {
            var existeMateria = _materias.ObtenerMateriaPorId(tarea.id_materia) !=null ;
            TareaReglas.ExisteMateria(existeMateria);
            var resultado = _tareas.AgregarTarea(tarea);
            return resultado;
        }

        public Task CambiarEstadoTarea(int idTarea)
        {
            throw new NotImplementedException();
        }

        public Task EditarTarea(int id, TareasAD tarea)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TareaDto>> ListarTareas()
        {
            throw new NotImplementedException();
        }

        public Task<List<TareaDto>> ListarTareasPorEstudiante(EstudianteGrupoDto estudianteGrupo)
        {
            throw new NotImplementedException();
        }

        public Task<TareaDto> ObtenerPorId(int idTarea)
        {
            throw new NotImplementedException();
        }
    }
}
