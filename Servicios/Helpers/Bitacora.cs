using DA.Entidades;
using DA.Interfaces;

namespace Servicios.Helpers
{
    public class Bitacora
    {
        private readonly IBitacoraAD _bitacoraDA;
        public Bitacora(IBitacoraAD bitacoraDA)
        {
            _bitacoraDA = bitacoraDA;
        }

        public async Task RegistrarEvento(string Tabla, string accion, string descripcion, string usuario)
        {
            var auditoria = new BitacoraAD
            {
                accion = accion,
                descripcion = descripcion,
                Fecha = DateTime.UtcNow,
                usuario = usuario,
                Tabla = Tabla,


            };
            await _bitacoraDA.RegistrarBitacora(auditoria);

        }
    }
}
