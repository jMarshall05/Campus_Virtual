using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.Modelos.ModelosDto;

namespace Abstracciones.Servicios
{
    public interface IAnunciosService
    {
        Task<int> AgregarAnuncio(AnuncioDto anuncio);
        Task EditarAnuncio(AnuncioDto anuncio);
        Task CambiarEstadoAnuncio(int anuncioId);
        Task<List<AnuncioDto>> ListarAnuncios();
        Task<AnuncioDto> ObtenerAnuncioPorId(int id);
    }
}
