using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.Modelos.ModelosDto;

namespace Abstracciones.Servicios
{
    public interface ITelefonosService
    {

        Task AgregarTelefono(List<TelefonoDto> telefono);
        Task EditarTelefono(List<TelefonoDto> telefonos);
        Task<IEnumerable<TelefonoDto>> ListarTelefonos();
    }
}
