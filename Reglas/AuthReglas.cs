using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.Excepciones;

namespace Reglas
{
    public class AuthReglas
    {

        public static void ExisteKey(bool existe)
        {
            if (existe)
                throw new BusinessException("El usuario ya usa 2FA");
        }
        public static void NoExisteKey(bool existe)
        {
            if (existe)
                throw new BusinessException("El usuario no usa 2FA");
        }
        public static void UsuarioInactivo(bool estado)
        {
            if (!estado)
                throw new BusinessException("Usuario Inactivo, contacte su administrador");

        }
    }
}
