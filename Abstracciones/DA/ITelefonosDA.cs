using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.Modelos.ModelosDA;
using Abstracciones.Modelos.ModelosDto;

namespace Abstracciones.DA
{
    public interface ITelefonosDA
    {
        Task AgregarTelefono(IEnumerable<TelefonoAD> telefono);
        Task EditarTelefono(IEnumerable<TelefonoAD> telefonos);
        Task<IEnumerable<TelefonoDto>> ListarTelefonos();
        Task<bool> ExisteTelefono(int codigo, long telefono);

    }
}
