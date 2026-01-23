using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.DA;
using Abstracciones.Modelos.ModelosDA;
using Abstracciones.Modelos.ModelosDto;
using Microsoft.EntityFrameworkCore;

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

        public async Task<bool> EditarTelefono(List<TelefonoAD> telefonos)
        {
            foreach (var telefono in telefonos)
            {
                var telefonoExistente = await _elContexto.Telefonos
                    .FirstOrDefaultAsync(t => t.Id == telefono.Id);
                if (telefonoExistente != null &&
                      telefonoExistente.Codigo == telefono.Codigo &&
                      telefonoExistente.Telefono == telefono.Telefono &&
                      telefonoExistente.Tipo == telefono.Tipo &&
                      telefonoExistente.Estado == telefono.Estado)
                {
                }
                else if (telefonoExistente != null)
                {
                    telefonoExistente.Codigo = telefono.Codigo;
                    telefonoExistente.Telefono = telefono.Telefono;
                    telefonoExistente.Tipo = telefono.Tipo;
                    telefonoExistente.Estado = telefono.Estado;
                    _elContexto.Entry(telefonoExistente).State = EntityState.Modified;

                }
            }

            var cambios = await _elContexto.SaveChangesAsync();
            if (cambios > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> ExisteTelefono(int codigo, long telefono)
        {
            var existe = await _elContexto.Telefonos.AnyAsync(t => t.Codigo == codigo && t.Telefono == telefono);
            return existe;
        }

        public IEnumerable<TelefonoDto> ListarTelefonos()
        {
            throw new NotImplementedException();
        }
    }
}
