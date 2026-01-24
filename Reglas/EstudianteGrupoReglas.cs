using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.Excepciones;

namespace Reglas
{
    public class EstudianteGrupoReglas
    {
        public static void ExisteGrupo(bool existe)
        {
            if (!existe)
                throw new BusinessException("El grupo no existe");
        }
    }
}
