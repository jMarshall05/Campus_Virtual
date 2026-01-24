using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.DA;
using Abstracciones.Modelos.ModelosDA;
using Abstracciones.Modelos.ModelosDto;

namespace DA
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
            var resultado = await _elContexto.SaveChangesAsync();
            if (resultado <= 0)
            {
                throw new Exception("No se pudo agregar la materia");
            }
            return entidad.Entity.IdMateria;
        }

        public Task<bool> CambiarEstadoMateria(int materiaId, bool estado)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EditarMateria(MateriasAD materia)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MateriaDto>> ListarMaterias()
        {
            throw new NotImplementedException();
        }

        public Task<MateriaDto> ObtenerMateriaPorId(int id)
        {
            throw new NotImplementedException();
        }
    }
}
