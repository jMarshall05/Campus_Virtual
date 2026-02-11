using Abstracciones.Modelos.ModelosDto;
using Abstracciones.Servicios.Helpers;
using DA.Interfaces;

namespace Servicios.Helpers
{
    public class GruposHelper : IGruposHelper
    {
        private readonly IGruposDA _grupos;
        public GruposHelper(IGruposDA grupos)
        {
            _grupos = grupos;
        }
        public Task<GruposDto> BuscarGrupoPorId(int IdGrupo)
        {
            var grupo = _grupos.BuscarGruposPorId(IdGrupo);
            return grupo;
        }
    }
}
