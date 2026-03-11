using Abstracciones.Modelos.ModelosDto;
using DA.Entidades;

namespace DA.Interfaces
{
    public interface IMateriasDA
    {
        Task<int> AgregarMateria(MateriasAD materia);
        Task EditarMateria(int IdMateria, MateriasAD materia);
        Task CambiarEstadoMateria(int materiaId);
        Task<IEnumerable<MateriaDto>> ListarMaterias();
        Task<MateriaDto> ObtenerMateriaPorId(int id);
        Task<MateriaDto> ObtenerMateriaPorNombre(string materia);

    }
}
