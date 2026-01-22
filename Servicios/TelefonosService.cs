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
        public Task<bool> AgregarTelefono(List<TelefonoDto> telefono)
        {
            var telefonosDA = telefono.Select(t => new TelefonoAD
            {
                IdUsuario = t.IdUsuario,
                Codigo = t.Codigo,
                Telefono = t.Telefono,
                Tipo = t.Tipo,
                Estado = t.Estado
            }).ToList();
            return _telefonos.AgregarTelefono(telefonosDA);
        }

        public Task<bool> EditarTelefono(List<TelefonoDto> telefonos)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TelefonoDto> ListarTelefonos()
        {
            throw new NotImplementedException();
        }
    }
}
