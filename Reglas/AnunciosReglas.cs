using Abstracciones.Excepciones;

namespace Reglas
{
    public class AnunciosReglas
    {
        public static void ExisteAnuncio(bool existe)
        {
            if (!existe)
                throw new BusinessException("El anuncio no existe");
        }
    }
}
