using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.Modelos.ModelosDto;
using DA.Entidades;

namespace DA.Interfaces
{
    public interface IEventosDA
    {
        Task<int> AgregarEvento(EventoAD evento);
        Task EditarEvento(EventoAD evento);
        Task CambiarEstadoEvento(int id);
        Task<IEnumerable<EventoDto>> ListarEventos(string idUsuario);

    }
}
