using Abstracciones.Modelos.ModelosDto;
using DA.Entidades;

namespace DA.Interfaces
{
    public interface IDocumentosAD
    {
        Task<int> AgregarDocumento(DocumentosAD documento);
        Task BorrarDocumento(int idDocumento);
        Task<bool> EditarDocumento(int id,DocumentosAD documento);
        Task<IEnumerable<DocumentosDto>> ListarDocumentos();
        Task<DocumentosDto> ObtenerDocumento(int Id);
    }
}
