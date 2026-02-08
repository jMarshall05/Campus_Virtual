using Abstracciones.Modelos.ModelosDto;
using DA.Entidades;

namespace DA.Interfaces
{
    public interface IGruposDA
    {
        Task<int> AgregarGrupo(GruposAD grupo);
        Task EditarGrupo(GruposAD grupo);
        Task<IEnumerable<GruposDto>> ListarGrupos();
        Task<GruposDto> BuscarGruposPorId(int idGrupo);
    }
}
