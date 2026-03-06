using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.Modelos.ModelosDto;

namespace Abstracciones.Servicios
{
    public interface IDocumentoService
    {
        Task<int> AgregarDocumento(DocumentosDto documento);
        Task BorrarDocumento(int idDocumento);
        Task<bool> EditarDocumento(int idDocumento, DocumentosDto documento);
        Task<IEnumerable<DocumentosDto>> ListarDocumentos();
        Task<DocumentosDto> DescargarDocumento(int Id);

    }
}
