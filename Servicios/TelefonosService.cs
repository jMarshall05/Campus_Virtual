using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.DA;
using Abstracciones.Modelos.ModelosDA;
using Abstracciones.Modelos.ModelosDto;
using Abstracciones.Servicios;
using DA;

namespace Servicios
{
    public class TelefonosService : ITelefonosService
    {
        private readonly ITelefonosDA _telefonos;
        public TelefonosService(ITelefonosDA telefonos)
        {
            _telefonos = telefonos;
        }
        public async Task AgregarTelefono(List<TelefonoDto> telefono)
        {
            var telefonosDA = telefono.Select(t => new TelefonoAD
            {
                IdUsuario = t.IdUsuario,
                Codigo = t.Codigo,
                Telefono = t.Telefono,
                Tipo = t.Tipo,
                Estado = t.Estado
            }).ToList();
            await _telefonos.AgregarTelefono(telefonosDA);
        }

        public async Task EditarTelefono(List<TelefonoDto> telefonos)
        {
            var telefonosAD = telefonos.Select(ConvertirAD).ToList();
            await _telefonos.EditarTelefono(telefonosAD);
        }

        public async Task<IEnumerable<TelefonoDto>> ListarTelefonos()
        {
            var telefonos = await _telefonos.ListarTelefonos();
            return telefonos;
        }

        private static TelefonoAD ConvertirAD(TelefonoDto telefono)
        {
            return new TelefonoAD
            {
                Id = telefono.Id,
                IdUsuario = telefono.IdUsuario,
                Codigo = telefono.Codigo,
                Telefono = telefono.Telefono,
                Tipo = telefono.Tipo,
                Estado = telefono.Estado
            };
        }
    }
}
