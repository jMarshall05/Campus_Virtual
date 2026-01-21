using System.Collections.Generic;

namespace Abstracciones.Modelos.ModelosDto
{
    public class UsuariosGruposDto
    {
        public List<UsuariosDto> usuarios { get; set; }
        public GruposDto grupo { get; set; }
    }
}
