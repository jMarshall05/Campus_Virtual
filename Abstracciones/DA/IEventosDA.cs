using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.Modelos.ModelosDto;

namespace Abstracciones.DA
{
    public interface IEventosDA
    {
        Task<int> AgregarEvento(EventoDto evento);
        Task<int> EditarEvento(EventoDto evento);
        Task<int> CambiarEstadoEvento(int id,bool estado);
        Task<IEnumerable<EventoDto>> ListarEventos(string idUsuario);

    }
}
