using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.Excepciones;

namespace Reglas
{
    public static class UsuarioReglas
    {
        public static void ValidarIdentificacionUnica(bool existe)
        {
            if (existe)
                throw new BusinessException("La identificación ya está registrada.");
        }
        public static void ValidarEmailUnico(bool existe)
        {
            if (existe)
                throw new BusinessException("El correo electrónico ya está registrado.");
        }
    }
}
