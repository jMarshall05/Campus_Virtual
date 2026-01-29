using Abstracciones.Modelos.ModelosDto;
using DA.Entidades;

namespace DA.Interfaces
{
    public interface IMateriasDA
    {
        Task<int> AgregarMateria(MateriasAD materia);
        Task EditarMateria(MateriasAD materia);
        Task CambiarEstadoMateria(int materiaId);
        Task<IEnumerable<MateriaDto>> ListarMaterias();
        Task<MateriaDto> ObtenerMateriaPorId(int id);

    }
}
