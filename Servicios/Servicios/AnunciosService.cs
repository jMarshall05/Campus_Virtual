using Abstracciones.Modelos.ModelosDto;
using Abstracciones.Servicios;
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
                anuncio.ImagenRuta = await _fileStorage.SaveAsync(anuncio.Imagen, "Anuncios");

            anuncio.FechaPublicacion = DateTime.Now;
            return await _anuncios.AgregarAnuncio(anuncio.Adapt<AnunciosAD>());
        }

        public async Task EditarAnuncio(AnuncioDto anuncio)
        {
            var existente = await _anuncios.ObtenerAnuncioPorId(anuncio.IdAnuncio);
            AnunciosReglas.ExisteAnuncio(existente != null);

            if (anuncio.QuitarImagen)
                anuncio.ImagenRuta = null;
            else if (anuncio.Imagen != null)
                anuncio.ImagenRuta = await _fileStorage.SaveAsync(anuncio.Imagen, "Anuncios");
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
