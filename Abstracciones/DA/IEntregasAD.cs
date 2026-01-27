using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.Modelos.ModelosDA;
using Abstracciones.Modelos.ModelosDto;

namespace Abstracciones.DA
{
    public interface IEntregasAD
    {
        Task<int> AgregarEntrega(EntregasAD entrega);
        Task EditarEntrega(EntregasAD entrega);
        Task EliminarEntrega(int id_entrega);
        Task<IEnumerable<EntregasDto>> ListarEntregas();
        Task<IEnumerable<EntregasDto>> ListarEntregasPorGrupo(int idGrupo);
        Task<IEnumerable<EntregasDto>> ListarEntregasPorEstudiante(string idEstudiante);

    }
}
