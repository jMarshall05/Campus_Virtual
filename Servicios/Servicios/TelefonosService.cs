using Abstracciones.Modelos.ModelosDto;
using Abstracciones.Servicios;
using DA.Entidades;
using DA.Interfaces;
using MapsterMapper;

namespace Servicios.Servicios
{
    public class TelefonosService : ITelefonosService
    {
        private readonly ITelefonosDA _telefonos;
        private readonly IMapper _mapper;
        public TelefonosService(IMapper mapper, ITelefonosDA telefonos)
        {
            _telefonos = telefonos;
            _mapper = mapper;
        }
        public async Task AgregarTelefono(List<TelefonoDto> telefono)
        {
            var telefonosDA = _mapper.Map<IEnumerable<TelefonoAD>>(telefono);
            await _telefonos.AgregarTelefono(telefonosDA);
        }

        public async Task EditarTelefono(List<TelefonoDto> telefonos)
        {
            var telefonosAD = _mapper.Map<IEnumerable<TelefonoAD>>(telefonos);
            await _telefonos.EditarTelefono(telefonosAD);
        }

        public async Task<IEnumerable<TelefonoDto>> ListarTelefonos()
        {
            var telefonos = await _telefonos.ListarTelefonos();
            return telefonos;
        }

    }
}
