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
        Task BorrarDocumento(int idDocumento);
        Task EditarDocumento( DocumentosAD documento);
        Task<IEnumerable<DocumentosDto>> ListarDocumentos();
    }
}
