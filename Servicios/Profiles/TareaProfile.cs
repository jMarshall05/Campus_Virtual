using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.Modelos.ModelosDto;
using AutoMapper;
using DA.Entidades;
using static Abstracciones.Modelos.Requests.TareasRequests;

namespace Servicios.Profiles
{
    public class TareaProfile : Profile
    {
        public TareaProfile()
        {

            CreateMap<TareasAD, TareaDto>();
            CreateMap<TareaDto, TareasAD>();
            CreateMap<EditarTareaRequest, TareasAD>();
            CreateMap<AgregarTareaRequest, TareasAD>();

        }
    }
}
