using Abstracciones.Modelos.ModelosDto;
using DA.Entidades;

namespace DA.Interfaces
{
    public interface ITelefonosDA
    {
        Task AgregarTelefono(IEnumerable<TelefonoAD> telefono);
        Task EditarTelefono(IEnumerable<TelefonoAD> telefonos);
        Task<IEnumerable<TelefonoDto>> ListarTelefonos();
        Task<bool> ExisteTelefono(int codigo, long telefono);

    }
}
