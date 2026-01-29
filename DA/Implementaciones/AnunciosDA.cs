using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.Modelos.ModelosDto;
using DA.Entidades;
using DA.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DA.Implementaciones
{
    public class AnunciosDA : IAnunciosAD
    {
        private readonly ApplicationDbContext _elContexto;
        public AnunciosDA(ApplicationDbContext contexto)
        {
            _elContexto = contexto;
        }

        public async Task<int> AgregarAnuncio(AnunciosAD anuncio)
        {
            var entidad = await _elContexto.Anuncios.AddAsync(anuncio);
            int resultado = await _elContexto.SaveChangesAsync();
            return entidad.Entity.IdAnuncio;
        }

        public async Task CambiarEstadoAnuncio(int anuncioId)
        {
            var anuncio = await _elContexto.Anuncios.FindAsync(anuncioId);
            if (anuncio != null)
            {
                anuncio.Estado = !anuncio.Estado;
                await _elContexto.SaveChangesAsync();
            }

        }

        public async Task EditarAnuncio(AnunciosAD anuncio)
        {
            var anuncioExistente = await _elContexto.Anuncios.FindAsync(anuncio);
            if (anuncioExistente != null)
            {
                anuncioExistente.Titulo = anuncio.Titulo;
                anuncioExistente.Descripcion = anuncio.Descripcion;
                anuncioExistente.FechaEvento = anuncio.FechaEvento;
                anuncioExistente.FechaPublicacion = DateTime.UtcNow;
                anuncioExistente.Estado = anuncio.Estado;
                anuncioExistente.ImagenRuta = anuncio.ImagenRuta; ;
                await _elContexto.SaveChangesAsync();
            }
        }

        public async Task<List<AnuncioDto>> ListarAnuncios()
        {
            var anuncios = await _elContexto.Anuncios
                 .Where(a => a.Estado == true)
                 .Select(a => new AnuncioDto
                 {
                     IdAnuncio = a.IdAnuncio,
                     Titulo = a.Titulo,
                     Descripcion = a.Descripcion,
                     FechaEvento = a.FechaEvento,
                     FechaPublicacion = a.FechaPublicacion,
                     ImagenRuta = a.ImagenRuta,
                     Estado = a.Estado
                 }).ToListAsync();
            return anuncios;
        }

        public async Task<AnuncioDto> ObtenerAnuncioPorId(int id)
        {
            var anuncio = await _elContexto.Anuncios
                 .Where(a => a.IdAnuncio == id)
                 .Select(a => new AnuncioDto
                 {
                     IdAnuncio = a.IdAnuncio,
                     Titulo = a.Titulo,
                     Descripcion = a.Descripcion,
                     FechaEvento = a.FechaEvento,
                     FechaPublicacion = a.FechaPublicacion,
                     ImagenRuta = a.ImagenRuta,
                     Estado = a.Estado
                 }).FirstOrDefaultAsync();
            return anuncio;
        }
    }
}
