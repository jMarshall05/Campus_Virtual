using Abstracciones.Modelos.ModelosDto;
using DA.Entidades;
using DA.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DA.Implementaciones
{
    public class MateriasDA : IMateriasDA
    {
        private readonly ApplicationDbContext _elContexto;
        public MateriasDA(ApplicationDbContext elContexto)
        {
            _elContexto = elContexto;
        }
        public async Task<int> AgregarMateria(MateriasAD materia)
        {
            var entidad = await _elContexto.Materias.AddAsync(materia);
            await _elContexto.SaveChangesAsync();
            return entidad.Entity.IdMateria;
        }

        public async Task CambiarEstadoMateria(int materiaId)
        {
            var materiaExistente = await _elContexto.Materias.FindAsync(materiaId);
            materiaExistente.Estado = !materiaExistente.Estado;
            await _elContexto.SaveChangesAsync();
        }

        public async Task EditarMateria(MateriasAD materia)
        {
            var materiaExistente = await _elContexto.Materias.FindAsync(materia);
            materiaExistente.Nombre = materia.Nombre;
            await _elContexto.SaveChangesAsync();
        }

        public async Task<IEnumerable<MateriaDto>> ListarMaterias()
        {
            var materias = await _elContexto.Materias.Select(m => new MateriaDto
            {
                Id_Materia = m.IdMateria,
                Nombre = m.Nombre,
                Estado = m.Estado
            }).ToListAsync();
            return materias;
        }

        public Task<MateriaDto> ObtenerMateriaPorId(int id)
        {
            var materia = _elContexto.Materias.Where(t => t.IdMateria == id).Select(t => new MateriaDto
            {
                Id_Materia = t.IdMateria,
                Nombre = t.Nombre,
                Estado = t.Estado
            }).FirstOrDefaultAsync();
            return materia;
        }
    }
}
