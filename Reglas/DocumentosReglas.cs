using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.Excepciones;

namespace Reglas
{
    public class DocumentosReglas
    {
        public static void ExisteDoc(bool existe)
        {
            if (!existe)
            {
                throw new BusinessException("El documento no existe");
            }
        }
    }
}
