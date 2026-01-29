using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.Modelos.ModelosDto;
using AutoMapper;
using DA.Entidades;
using static Abstracciones.Modelos.Requests.UsuariosRequests;

namespace Servicios.Profiles
{
    public class UsuariosProfile : Profile
    {
        public UsuariosProfile()
        {
            CreateMap<UsuariosDto, UsuariosAD>()
                .ReverseMap();

            CreateMap<EditarUsuarioRequest, UsuariosAD>()
                .ForMember(dest => dest.EstudianteGrupo, opt => opt.Ignore());


            CreateMap<RegisterRequest, UsuariosAD>()
                .ForMember(u => u.FechaDeModificacion, opt => opt.Ignore())
                .ForMember(u=>u.EstudianteGrupo,opt=>opt.Ignore())
                .ForMember(u=>u.Telefonos,opt=>opt.Ignore());
        }
    }
}
