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

        Task<bool> AgregarTelefono(List<TelefonoDto> telefono);
        Task<bool> EditarTelefono(List<TelefonoDto> telefonos);
        IEnumerable<TelefonoDto> ListarTelefonos();
    }
}
