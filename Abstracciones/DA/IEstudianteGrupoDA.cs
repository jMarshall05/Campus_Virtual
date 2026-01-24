using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.Modelos.ModelosDA;
using Abstracciones.Modelos.ModelosDto;

namespace Abstracciones.DA
{
    public interface IEstudianteGrupoDA
    {
        Task<int> AgregarEstudianteGrupo(EstudianteGrupoAD estudianteGrupoDto);
        Task<bool> ActualizarEstudianteGrupo(EstudianteGrupoAD estudiante);
        Task<EstudianteGrupoDto> BuscarEstudianteGrupoPorEstudianteId(string idEstudiante);
        Task<List<EstudianteGrupoDto>> BuscarEstudianteGrupoPorGrupoId(int idGrupo);
        Task<List<EstudianteGrupoDto>> ListarEstudiantesGrupos();
        Task<List<EstudianteGrupoDto>> ListarEstudiantesPorIdGrupo(int idGrupo);
        Task<List<EstudianteGrupoDto>> ListarGruposPorIdEstudiante(string idUsuario);

    }
}
