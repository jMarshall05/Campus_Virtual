using Abstracciones.Modelos.ModelosDto;

namespace Abstracciones.Servicios.Helpers
{
    public interface IGruposHelper
    {
        Task<GruposDto> BuscarGrupoPorId(int IdGrupo);
    }
}
