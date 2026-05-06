using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.Modelos.ModelosDto;
using Microsoft.AspNetCore.Mvc;

namespace Abstracciones.Api
{
    public interface IAnunciosController
    {
        Task<IActionResult> AgregarAnuncio(AnuncioDto anuncio);
        Task<IActionResult> EditarAnuncio(AnuncioDto anuncio);
        Task<IActionResult> EliminarAnuncio(int anuncioId);
        Task<IActionResult> ListarAnuncios();
        Task<IActionResult> ObtenerAnuncioPorId(int id);
        Task<IActionResult> ObtenerImagen(int id);
    }
}
