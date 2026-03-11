using Abstracciones.Excepciones;
using Abstracciones.Modelos.ModelosDto;

namespace Reglas
{
    public static class UsuarioReglas
    {
        public static void ValidarIdentificacionUnica(bool existe)
        {
            if (existe)
                throw new BusinessException("La identificación ya está registrada.");
        }

        public static void ValidarTelefonoUnico(bool existe, TelefonoDto telefono)
        {
            if (existe)
                throw new BusinessException($"El número de teléfono: +{telefono.Codigo} {telefono.Telefono} ya está registrado.");
        }
        public static void ValidarEmailUnico(bool existe)
        {
            if (existe)
                throw new BusinessException("El correo electrónico ya está registrado.");
        }
        public static void ValidarUsuario(bool existe)
        {
            if (!existe)
                throw new BusinessException("El usuario no existe .");
        }

    }
}
