using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.Modelos.ModelosDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Abstracciones.Modelos.Requests.DocumentosRequest;

namespace Abstracciones.Api
{
    public interface IDocumetosController
    {
        Task<IActionResult> AgregarDocumento(AgregarDocumentoRequest data);
        Task<IActionResult> BorrarDocumento(int idDocumento);
        Task<IActionResult> EditarDocumento(int idDocumento, EditarDocumentoRequest documento);
        Task<IActionResult> ListarDocumentos();
        Task<IActionResult> DescargarDocumento(int Id);

    }
}
