using Abstracciones.Modelos.ModelosDto;
using static Abstracciones.Modelos.Requests.GruposRequests;

namespace Abstracciones.Servicios
{
    public interface IGruposService
    {
        Task<int> AgregarGrupo(string IdUsuario, AgregarGrupoRequest grupo);
        Task EditarGrupo(string IdUsuario, EditarGrupoRequest grupo);
        Task<IEnumerable<GruposDto>> ListarGrupos();
        Task<GruposDto> BuscarGruposPorId(int idGrupo);
        byte[] QrExportar(string url);
        Task<byte[]> ExportarGrupoPDF(int IdGrupo, string? rutaLogo);
    }
}
