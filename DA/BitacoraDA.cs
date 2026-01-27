using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.DA;
using Abstracciones.Modelos.ModelosDA;

namespace DA
{
    public class BitacoraDA : IBItacoraAD
    {
        private readonly ApplicationDbContext _elContexto;
        public BitacoraDA(ApplicationDbContext elContexto)
        {
            _elContexto = elContexto;
        }
        public async Task RegistrarBitacora(BitacoraAD Bitacora)
        {
            var entidad = await _elContexto.Bitacora.AddAsync(Bitacora);
            await _elContexto.SaveChangesAsync();

        }
    }
}
