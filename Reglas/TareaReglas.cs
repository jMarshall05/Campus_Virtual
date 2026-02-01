using Abstracciones.Excepciones;

namespace Reglas
{
    public class TareaReglas
    {

        public static void ExisteTarea(bool existe)
        {
            if (!existe)
                throw new BusinessException("La Tarea no existe");

        }

    }
}
