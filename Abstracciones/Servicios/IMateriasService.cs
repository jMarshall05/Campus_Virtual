using Abstracciones.Modelos.ModelosDto;
using Abstracciones.Modelos.Requests;

namespace Abstracciones.Servicios
{
    public interface IMateriasService
    {

        Task<int> AgregarMateria(MateriaRequests materia);
        Task EditarMateria(int IdMateria, MateriaRequests materia);
        Task CambiarEstadoMateria(int materiaId);
        Task<IEnumerable<MateriaDto>> ListarMaterias();
        Task<MateriaDto> ObtenerMateriaPorId(int id);
        Task<MateriaDto> ObtenerMateriaNombre(string nombre);


    }
}
