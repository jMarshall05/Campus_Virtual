using Abstracciones.Api;
using Abstracciones.Excepciones;
using Abstracciones.Modelos.ModelosDto;
using Abstracciones.Servicios;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static Abstracciones.Modelos.Requests.DocumentosRequest;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/docs")]
    public class DocumentosController : ControllerBase, IDocumetosController
    {
        private readonly IDocumentoService _documentos;
        private readonly IFileStorageService _fileStorage;
        private readonly ILogger<DocumentosController> _logger;
        public DocumentosController(ILogger<DocumentosController> logger, IDocumentoService documentos, IFileStorageService filestorage)
        {
            _documentos = documentos;
            _fileStorage = filestorage;
            _logger = logger;
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        [Produces("application/json")]
        public async Task<IActionResult> AgregarDocumento([FromForm] AgregarDocumentoRequest data)
        {
            try
            {

                if (data == null || data.Doc.Length == 0)
                    return BadRequest("Faltan Datos o el archivo");

                var url = await _fileStorage.SaveAsync(data.Doc, "Docs");
                var dto = data.Adapt<DocumentosDto>();
                dto.RutaArchivo = url;
                var respuesta = await _documentos.AgregarDocumento(dto);

                return Ok(new
                {
                    Id = respuesta,
                    Url = url
                });
            }
            catch (BusinessException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al Agregar Documento");
                return StatusCode(500, "Ocurrió un error inesperado");
            }
        }
        [HttpDelete("{idDocumento}")]
        public async Task<IActionResult> BorrarDocumento(int idDocumento)
        {
            try
            {

                var doc = await _documentos.ObtenerDocumento(idDocumento);
                if (doc == null)
                    return BadRequest("Este documento no existe");
                await _documentos.BorrarDocumento(idDocumento, doc);
                return NoContent();
            }
            catch (BusinessException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al borrar Documento");
                return StatusCode(500, "Ocurrió un error inesperado");
            }

        }

        [HttpGet("download/{Id}")]
        public async Task<IActionResult> DescargarDocumento(int Id)
        {
            try
            {
                var doc = await _documentos.DescargarDocumento(Id);
                if (doc == null)
                    return NoContent();

                var file = await _fileStorage.GetAsync(doc.RutaArchivo);
                var provider = new FileExtensionContentTypeProvider();

                if (!provider.TryGetContentType(doc.RutaArchivo, out string contentType))
                {
                    contentType = "application/octet-stream";
                }
                var fileName = Path.GetFileName(doc.RutaArchivo);

                var nombreOriginal = fileName.Contains('_')
                    ? fileName[(fileName.IndexOf('_') + 1)..]
                    : fileName;

                return PhysicalFile(file, contentType, nombreOriginal);
            }
            catch (BusinessException ex)
            {

                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al Descargar Documento");
                return StatusCode(500, "Ocurrió un error inesperado");
            }
        }

        [HttpPut("{idDocumento}")]
        [Consumes("multipart/form-data")]
        [Produces("application/json")]
        public async Task<IActionResult> EditarDocumento(int idDocumento, [FromForm] EditarDocumentoRequest documento)
        {
            try
            {
                var dto = documento.Adapt<DocumentosDto>();
                var response = await _documentos.EditarDocumento(idDocumento, dto);
                return Ok();
            }
            catch (BusinessException ex)
            {

                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al Descargar Documento");
                return StatusCode(500, "Ocurrió un error inesperado");
            }

        }
        [HttpGet]
        public async Task<IActionResult> ListarDocumentos()
        {
            try
            {
                var response = await _documentos.ListarDocumentos();
                return Ok(response);
            }
            catch (BusinessException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al Buscar Documentos");
                return StatusCode(500, "Ocurrió un error inesperado");
            }
        }
    }
}
