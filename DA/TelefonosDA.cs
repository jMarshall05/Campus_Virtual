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
        public async Task AgregarTelefono(IEnumerable<TelefonoAD> telefono)
        {
            await _elContexto.Telefonos.AddRangeAsync(telefono);
            await _elContexto.SaveChangesAsync();
        }

        public async Task EditarTelefono(IEnumerable<TelefonoAD> telefonos)
        {
            foreach (var telefono in telefonos)
            {
                var telefonoExistente = await _elContexto.Telefonos.FindAsync(telefono);
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
            await _elContexto.SaveChangesAsync();
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
