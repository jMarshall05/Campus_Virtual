using Abstracciones.Modelos.ModelosDto;

namespace Abstracciones.Servicios
{
    public interface ITelefonosService
    {

        Task AgregarTelefono(List<TelefonoDto> telefono);
        Task EditarTelefono(List<TelefonoDto> telefonos);
        Task<IEnumerable<TelefonoDto>> ListarTelefonos();
        Task<bool> ExisteTelefono(int codigo, long telefono);
    }
}
