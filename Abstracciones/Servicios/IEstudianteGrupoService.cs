using Abstracciones.Modelos.ModelosDto;

namespace Abstracciones.Servicios
{
    public interface IEstudianteGrupoService
    {
        Task<int> AgregarEstudianteGrupo(EstudianteGrupoDto estudianteGrupoDto);
        Task ActualizarEstudianteGrupo(EstudianteGrupoDto estudiante);
        Task<EstudianteGrupoDto> BuscarEstudianteGrupoPorEstudianteId(string idEstudiante);
        Task<IEnumerable<EstudianteGrupoDto>> BuscarEstudianteGrupoPorGrupoId(int idGrupo);
        Task<IEnumerable<EstudianteGrupoDto>> ListarEstudiantesGrupos();
        Task<IEnumerable<EstudianteGrupoDto>> ListarEstudiantesPorIdGrupo(int idGrupo);
        Task<IEnumerable<EstudianteGrupoDto>> ListarGruposPorIdEstudiante(string idUsuario);
    }
}
