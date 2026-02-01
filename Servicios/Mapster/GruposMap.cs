using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.Modelos.ModelosDto;
using DA.Entidades;
using Mapster;

namespace Servicios.Mapster
{
    public class GruposMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<GruposDto, GruposAD>()
                .TwoWays();
        }
    }
}
