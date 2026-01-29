namespace Reglas
{
    public class MateriaReglas
    {
        public static void ExisteMateria(bool existe)
        {
            if (!existe)
                throw new Abstracciones.Excepciones.BusinessException("La materia no existe");
        }
    }
}
