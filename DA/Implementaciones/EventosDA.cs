using Abstracciones.Modelos.ModelosDto;
using DA.Entidades;
using DA.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DA.Implementaciones
{
    public class EventosDA : IEventosDA
    {
        private readonly ApplicationDbContext _elContexto;
        public EventosDA(ApplicationDbContext elContexto)
        {
            _elContexto = elContexto;
        }
        public async Task<int> AgregarEvento(EventoAD evento)
        {
            var nuevoEvento = await _elContexto.Eventos.AddAsync(evento);
            await _elContexto.SaveChangesAsync();
            return nuevoEvento.Entity.Id;
        }

        public async Task CambiarEstadoEvento(int id)
        {
            var entidad = await _elContexto.Eventos.FindAsync(id);
            entidad.Estado = !entidad.Estado;
            await _elContexto.SaveChangesAsync();
        }

        public async Task EditarEvento(EventoAD evento)
        {
            var entidad = await _elContexto.Eventos.FindAsync(evento.Id);
            entidad.Titulo = evento.Titulo;
            entidad.FechaInicio = evento.FechaInicio;
            entidad.FechaFin = evento.FechaFin;
            await _elContexto.SaveChangesAsync();
        }

        public async Task<IEnumerable<EventoDto>> ListarEventos(string idUsuario)
        {
            var eventos = await _elContexto.Eventos
                .Where(e => e.IdUsuario == idUsuario && e.Estado)
                .Select(e => new EventoDto
                {
                    Id = e.Id,
                    Titulo = e.Titulo,
                    FechaInicio = e.FechaInicio,
                    FechaFin = e.FechaFin,
                    IdUsuario = e.IdUsuario,
                    Estado = e.Estado
                }).ToListAsync();
            return eventos;
        }
    }
}
