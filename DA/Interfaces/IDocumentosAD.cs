using Abstracciones.Modelos.ModelosDto;
using DA.Entidades;

namespace DA.Interfaces
{
    public interface IDocumentosAD
    {
        Task<int> AgregarDocumento(DocumentosAD documento);
        Task BorrarDocumento(int idDocumento);
        Task EditarDocumento(DocumentosAD documento);
        Task<IEnumerable<DocumentosDto>> ListarDocumentos();
    }
}
