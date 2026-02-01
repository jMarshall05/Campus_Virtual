using Abstracciones.Excepciones;

namespace Reglas
{
    public class MateriaReglas
    {
        public static void ExisteMateria(bool existe)
        {
            if (!existe)
                throw new BusinessException("La materia no existe");
        }
    }
}
