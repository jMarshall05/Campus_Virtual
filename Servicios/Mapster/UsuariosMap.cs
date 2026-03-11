using Abstracciones.Modelos.ModelosDto;
using DA.Entidades;
using Mapster;
using static Abstracciones.Modelos.Requests.UsuariosRequests;

namespace Servicios.Mapster
{
    public class UsuariosMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<UsuariosAD, UsuariosDto>()
                .TwoWays();

            config.NewConfig<EditarUsuarioRequest, UsuariosAD>()
                .Ignore(dest => dest.EstudianteGrupo);

            config.NewConfig<RegisterRequest, UsuariosAD>()
                .Ignore(dest => dest.FechaDeModificacion)
                .Ignore(dest => dest.EstudianteGrupo)
                .Ignore(dest => dest.Telefonos);
        }
    }
}
