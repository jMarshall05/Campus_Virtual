using Abstracciones.Modelos.ModelosDto;
using DA.Entidades;
using Mapster;
using static Abstracciones.Modelos.Requests.TareasRequests;

namespace Servicios.Profiles
{
    public class TareasMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<TareasAD, TareaDto>()
                 .TwoWays();
            config.NewConfig<EditarTareaRequest, TareasAD>();
            config.NewConfig<AgregarTareaRequest, TareasAD>();
        }
    }
}
