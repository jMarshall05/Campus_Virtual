using Abstracciones.Excepciones;

namespace Reglas
{
    public class MateriaReglas
    {
        public static void SiExiste(bool existe)
        {
            if (existe)
                throw new BusinessException("La materia ya existe");
        }
        public static void NoExiste(bool existe)
        {
            if (!existe)
                throw new BusinessException("La materia ya existe");
        }
    }
}
