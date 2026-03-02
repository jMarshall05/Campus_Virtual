using Abstracciones.Api;
using Abstracciones.Excepciones;
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
        private readonly ILogger<DocumentosController> _logger;
        public DocumentosController(ILogger<DocumentosController> logger,IDocumentoService documentos, IFileStorageService filestorage)
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
                _logger.LogError(ex, "Error al buscar grupos");
                return StatusCode(500, "Ocurrió un error inesperado");
            }
        }
        [HttpDelete]
        public Task<IActionResult> BorrarDocumento(int idDocumento)
        {
            throw new NotImplementedException();
        }
        [HttpPut("{idDocumento}")]
        public Task<IActionResult> EditarDocumento(int idDocumento, DocumentosDto documento)
        {
            throw new NotImplementedException();
        }
        [HttpGet]
        public Task<IActionResult> ListarDocumentos()
        {
            throw new NotImplementedException();
        }
    }
}
