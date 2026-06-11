using Abstracciones.Excepciones;
using Abstracciones.Modelos.ModelosDto;
using Abstracciones.Servicios;
using Abstracciones.Validaciones;
using DA.Entidades;
using DA.Interfaces;
using Mapster;
using Reglas;

namespace Servicios.Servicios
{
    public class AnunciosService : IAnunciosService
    {
        private readonly IAnunciosAD _anuncios;
        private readonly IFileStorageService _fileStorage;

        public AnunciosService(IAnunciosAD anuncios, IFileStorageService fileStorage)
        {
            _anuncios = anuncios;
            _fileStorage = fileStorage;
        }



        public async Task<int> AgregarAnuncio(AnuncioDto anuncio)
        {
            if (anuncio.Imagen != null)
            {
                if (!FileUploadConstants.IsWithinSizeLimit(anuncio.Imagen.Length))
                    throw new BusinessException("La imagen supera el límite de 10 MB.");

                if (!FileUploadConstants.IsValidFileType(anuncio.Imagen.ContentType, FileUploadConstants.AllowedImageTypes))
                    throw new BusinessException($"Tipo de imagen no permitido: {anuncio.Imagen.ContentType}");

                anuncio.ImagenRuta = await _fileStorage.SaveAsync(anuncio.Imagen, "Anuncios",false);
            }

            anuncio.FechaPublicacion = DateTime.UtcNow;
            return await _anuncios.AgregarAnuncio(anuncio.Adapt<AnunciosAD>());
        }

        public async Task EditarAnuncio(AnuncioDto anuncio)
        {
            var existente = await _anuncios.ObtenerAnuncioPorId(anuncio.IdAnuncio);
            AnunciosReglas.ExisteAnuncio(existente != null);

            if (anuncio.QuitarImagen)
                anuncio.ImagenRuta = null;
            else if (anuncio.Imagen != null)
            {
                if (!FileUploadConstants.IsWithinSizeLimit(anuncio.Imagen.Length))
                    throw new BusinessException("La imagen supera el límite de 10 MB.");

                if (!FileUploadConstants.IsValidFileType(anuncio.Imagen.ContentType, FileUploadConstants.AllowedImageTypes))
                    throw new BusinessException($"Tipo de imagen no permitido: {anuncio.Imagen.ContentType}");

                anuncio.ImagenRuta = await _fileStorage.SaveAsync(anuncio.Imagen, "Anuncios",false);
            }
            else
                anuncio.ImagenRuta = existente.ImagenRuta;

            await _anuncios.EditarAnuncio(anuncio.Adapt<AnunciosAD>());
        }

        public async Task CambiarEstadoAnuncio(int anuncioId)
        {
            await _anuncios.CambiarEstadoAnuncio(anuncioId);
        }

        public async Task<List<AnuncioDto>> ListarAnuncios()
        {
            return await _anuncios.ListarAnuncios();
        }

        public async Task<AnuncioDto> ObtenerAnuncioPorId(int id)
        {
            return await _anuncios.ObtenerAnuncioPorId(id);
        }
    }
}
