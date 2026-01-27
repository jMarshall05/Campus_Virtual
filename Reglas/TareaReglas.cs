using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reglas
{
    public class TareaReglas
    {
        public static void ExisteMateria(bool existe)
        {
            if (!existe)
                throw new Abstracciones.Excepciones.BusinessException("La materia no existe");
        }
    }
}
