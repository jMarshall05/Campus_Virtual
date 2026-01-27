using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.Modelos.ModelosDA;
using Abstracciones.Modelos.ModelosDto;

namespace Abstracciones.DA
{
    public interface IGruposDA
    {
        Task<int> AgregarGrupo(GruposAD grupo);
        Task EditarGrupo(int id, GruposAD grupo);
        Task<IEnumerable<GruposDto>> ListarGrupos();
        Task<GruposDto> BuscarGruposPorId(int idGrupo);
    }
}
