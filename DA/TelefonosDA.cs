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
        public async Task<bool> AgregarTelefono(IEnumerable<TelefonoAD> telefono)
        {
            foreach (var tel in telefono)
            {

                await _elContexto.Telefonos.AddAsync(tel);
            }
            var cambios = await _elContexto.SaveChangesAsync();
            if (cambios > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> EditarTelefono(IEnumerable<TelefonoAD> telefonos)
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

        public async Task<IEnumerable<TelefonoDto>> ListarTelefonos()
        {
            var telefonosDto = await _elContexto.Telefonos
                .Select(t => new TelefonoDto
                {
                    Id = t.Id,
                    IdUsuario = t.IdUsuario,
                    Codigo = t.Codigo,
                    Telefono = t.Telefono,
                    Tipo = t.Tipo,
                    Estado = t.Estado
                }).ToListAsync();
            return telefonosDto;
        }
    }
}
