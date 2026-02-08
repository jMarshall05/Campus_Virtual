using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.Modelos.ModelosDto;

namespace Abstracciones.Servicios.Helpers
{
    public interface IGruposHelper
    {
        Task<GruposDto> BuscarGrupoPorId(int IdGrupo);
    }
}
