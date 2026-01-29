using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.Entidades;

namespace DA.Interfaces
{
    public interface IBItacoraAD
    {
        Task RegistrarBitacora(BitacoraAD Bitacora);

    }
}
