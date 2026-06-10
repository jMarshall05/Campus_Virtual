using Abstracciones.Modelos.ModelosDto;
using DA.Entidades;
using Mapster;

namespace Servicios.Profiles
{
    public class UsuariosMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<UsuariosAD, UsuariosDto>()
                .Map(dest => dest.Telefonos, src => src.Telefonos != null
                    ? src.Telefonos.Select(t => new TelefonoDto
                    {
                        Id = t.Id,
                        IdUsuario = t.IdUsuario,
                        Codigo = t.Codigo,
                        Telefono = t.Telefono,
                        Tipo = t.Tipo,
                        Estado = t.Estado
                    }).ToList()
                    : new List<TelefonoDto>())
                .Map(dest => dest.Grupo, src => src.EstudianteGrupo != null
                    ? new GruposDto
                    {
                        idGrupo = src.EstudianteGrupo.Grupo.idGrupo,
                        Nombre = src.EstudianteGrupo.Grupo.Nombre
                    }
                    : null);
        }
    }
}
