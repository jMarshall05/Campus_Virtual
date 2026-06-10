using Abstracciones.Api;
using Abstracciones.Modelos.ModelosDto;
using Abstracciones.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/announcements")]
    public class AnunciosController : ControllerBase, IAnunciosController
    {
        private readonly IAnunciosService _anuncios;
        private readonly IFileStorageService _fileStorage;

        public AnunciosController(IFileStorageService fileStorage, IAnunciosService anuncios)
        {
            _fileStorage = fileStorage;
            _anuncios = anuncios;
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [Produces("application/json")]
        public async Task<IActionResult> AgregarAnuncio([FromForm] AnuncioDto anuncio)
        {
            var id = await _anuncios.AgregarAnuncio(anuncio);
            return Ok(id);
        }

        [HttpPut]
        [Consumes("multipart/form-data")]
        [Produces("application/json")]
        public async Task<IActionResult> EditarAnuncio([FromForm] AnuncioDto anuncio)
        {
            await _anuncios.EditarAnuncio(anuncio);
            return Ok($"Anuncio {anuncio.IdAnuncio} actualizado correctamente");
        }

        [HttpPatch("{anuncioId}")]
        public async Task<IActionResult> EliminarAnuncio(int anuncioId)
        {
            await _anuncios.CambiarEstadoAnuncio(anuncioId);
            return Ok($"Estado del anuncio {anuncioId} actualizado");
        }

        [HttpGet]
        public async Task<IActionResult> ListarAnuncios()
        {
            var lista = await _anuncios.ListarAnuncios();
            return Ok(lista);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerAnuncioPorId(int id)
        {
            var anuncio = await _anuncios.ObtenerAnuncioPorId(id);
            if (anuncio == null)
                return NotFound($"No se encontró el anuncio con ID {id}");
            return Ok(anuncio);
        }

        [HttpGet("{id}/image")]
        public async Task<IActionResult> ObtenerImagen(int id)
        {
            var img = await _anuncios.ObtenerAnuncioPorId(id);
            if (img == null || img.ImagenRuta == null)
                return NoContent();

            var file = await _fileStorage.GetAsync(img.ImagenRuta);
            var provider = new FileExtensionContentTypeProvider();

            if (!provider.TryGetContentType(img.ImagenRuta, out string contentType))
            {
                contentType = "application/octet-stream";
            }
            var fileName = Path.GetFileName(img.ImagenRuta);

            var nombreOriginal = fileName.Contains('_')
                ? fileName[(fileName.IndexOf('_') + 1)..]
                : fileName;

            return PhysicalFile(file, contentType, nombreOriginal);
        }
    }
}
