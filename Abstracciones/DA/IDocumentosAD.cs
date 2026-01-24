using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.Modelos.ModelosDA;
using Abstracciones.Modelos.ModelosDto;

namespace Abstracciones.DA
{
    public interface IDocumentosAD
    {
        Task<int> AgregarDocumento(DocumentosAD documento);
        Task<bool> CambiarEstadoDocumento(int idDocumento, bool estado);
        Task<bool> EditarDocumento(int idDocumento, DocumentosAD documento);
        Task<IEnumerable<DocumentosDto>> ListarDocumentos();
    }
}
