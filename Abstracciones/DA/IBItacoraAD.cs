using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.Modelos.ModelosDA;

namespace Abstracciones.DA
{
    public interface IBItacoraAD
    {
        Task RegistrarBitacora(BitacoraAD Bitacora);

    }
}
