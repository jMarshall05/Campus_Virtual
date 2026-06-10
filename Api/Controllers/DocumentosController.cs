using Abstracciones.Api;
using Abstracciones.Modelos.ModelosDto;
using Abstracciones.Servicios;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using static Abstracciones.Modelos.Requests.DocumentosRequest;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/docs")]
    public class DocumentosController : ControllerBase, IDocumetosController
    {
        private readonly IDocumentoService _documentos;
        private readonly IFileStorageService _fileStorage;
        public DocumentosController(IDocumentoService documentos, IFileStorageService filestorage)
        {
            _documentos = documentos;
            _fileStorage = filestorage;
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [Produces("application/json")]
        public async Task<IActionResult> AgregarDocumento([FromForm] AgregarDocumentoRequest data)
        {
            if (data == null || data.Doc.Length == 0)
                return BadRequest("Faltan Datos o el archivo");

            var url = await _fileStorage.SaveAsync(data.Doc, "Docs", true);
            var dto = data.Adapt<DocumentosDto>();
            dto.RutaArchivo = url;
            var respuesta = await _documentos.AgregarDocumento(dto);

            return Ok(new
            {
                Id = respuesta,
                Url = url
            });
        }

        [HttpDelete("{idDocumento}")]
        public async Task<IActionResult> BorrarDocumento(int idDocumento)
        {
            var doc = await _documentos.ObtenerDocumento(idDocumento);
            if (doc == null)
                return BadRequest("Este documento no existe");
            await _documentos.BorrarDocumento(idDocumento, doc);
            return NoContent();
        }

        [HttpGet("download/{Id}")]
        public async Task<IActionResult> DescargarDocumento(int Id)
        {
            var doc = await _documentos.DescargarDocumento(Id);
            if (doc == null)
                return NoContent();

            var url = await _fileStorage.GetAsync(doc.RutaArchivo);
            return Redirect(url);
        }

        [HttpPut("{idDocumento}")]
        [Consumes("multipart/form-data")]
        [Produces("application/json")]
        public async Task<IActionResult> EditarDocumento(int idDocumento, [FromForm] EditarDocumentoRequest documento)
        {
            var dto = documento.Adapt<DocumentosDto>();
            var response = await _documentos.EditarDocumento(idDocumento, dto);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> ListarDocumentos()
        {
            var response = await _documentos.ListarDocumentos();
            return Ok(response);
        }
    }
}
