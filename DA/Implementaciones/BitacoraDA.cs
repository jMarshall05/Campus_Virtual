using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.Entidades;
using DA.Interfaces;

namespace DA.Implementaciones
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
