using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.Modelos.ModelosDto;
using AutoMapper;
using DA.Entidades;

namespace Servicios.Profiles
{
    public class TelefonosProfile : Profile
    {
        public TelefonosProfile()
        {
            CreateMap<TelefonoDto, TelefonoAD>()
             .ForMember(dest => dest.Usuario, opt => opt.Ignore())
             .ReverseMap();
        }
    }
}