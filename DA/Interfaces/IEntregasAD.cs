using Abstracciones.Modelos.ModelosDto;
using DA.Entidades;

namespace DA.Interfaces
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
