using Abstracciones.Modelos.ModelosDto;
using DA.Entidades;
using Mapster;

namespace Servicios.Profiles
{
    public class TelefonosMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<TelefonoDto, TelefonoAD>()
                .Ignore(dest => dest.Usuario)
                .TwoWays();
        }
    }
}