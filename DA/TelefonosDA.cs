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
    public class TelefonosDA : ITelefonosDA
    {
        private readonly ApplicationDbContext _elContexto;
        public TelefonosDA(ApplicationDbContext elContexto)
        {
            _elContexto = elContexto;
        }
        public async Task<bool> AgregarTelefono(List<TelefonoAD> telefono)
        {
            foreach (var tel in telefono)
            {

                _elContexto.Telefonos.Add(tel);
            }
            var cambios = await _elContexto.SaveChangesAsync();
            if (cambios > 0)
            {
                return true;
            }
            return false;
        }

        public Task<bool> EditarTelefono(List<TelefonoAD> telefonos)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TelefonoDto> ListarTelefonos()
        {
            throw new NotImplementedException();
        }
    }
}
