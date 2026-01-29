namespace Reglas
{
    public class TareaReglas
    {

        public static void ExisteTarea(bool existe)
        {
            if (!existe)
                throw new Abstracciones.Excepciones.BusinessException("La Tarea no existe");

        }

    }
}
