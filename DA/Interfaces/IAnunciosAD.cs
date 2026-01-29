using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.Modelos.ModelosDto;
using DA.Entidades;

namespace DA.Interfaces
{
    public interface IAnunciosAD
    {
        Task<int> AgregarAnuncio(AnunciosAD anuncio);
        Task EditarAnuncio(AnunciosAD anuncio);
        Task CambiarEstadoAnuncio(int anuncioId);
        Task<List<AnuncioDto>> ListarAnuncios();
        Task<AnuncioDto> ObtenerAnuncioPorId(int id);
    }
}
