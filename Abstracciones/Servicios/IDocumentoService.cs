using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.Modelos.ModelosDto;
using static Abstracciones.Modelos.Requests.DocumentosRequest;

namespace Abstracciones.Servicios
{
    public interface IDocumentoService
    {
        Task<int> AgregarDocumento(DocumentosDto documento);
        Task BorrarDocumento(int idDocumento, DocumentosDto doc);
        Task<bool> EditarDocumento(int idDocumento, DocumentosDto documento);
        Task<IEnumerable<DocumentosDto>> ListarDocumentos();
        Task<DocumentosDto> DescargarDocumento(int Id);
        Task<DocumentosDto> ObtenerDocumento(int Id);

    }
}
