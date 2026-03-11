using Abstracciones.Excepciones;

namespace Reglas
{
    public class GruposReglas
    {
        public static void ExisteGrupo(bool existe)
        {
            if (!existe)
                throw new BusinessException("El grupo no existe");
        }
    }
}
