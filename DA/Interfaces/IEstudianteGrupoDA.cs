using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.Modelos.ModelosDto;
using DA.Entidades;

namespace DA.Interfaces
{
    public interface IEstudianteGrupoDA
    {
        Task<int> AgregarEstudianteGrupo(EstudianteGrupoAD estudianteGrupoDto);
        Task ActualizarEstudianteGrupo(EstudianteGrupoAD estudiante);
        Task<EstudianteGrupoDto> BuscarEstudianteGrupoPorEstudianteId(string idEstudiante);
        Task<IEnumerable<EstudianteGrupoDto>> BuscarEstudianteGrupoPorGrupoId(int idGrupo);
        Task<IEnumerable<EstudianteGrupoDto>> ListarEstudiantesGrupos();
        Task<IEnumerable<EstudianteGrupoDto>> ListarEstudiantesPorIdGrupo(int idGrupo);
        Task<IEnumerable<EstudianteGrupoDto>> ListarGruposPorIdEstudiante(string idUsuario);

    }
}
