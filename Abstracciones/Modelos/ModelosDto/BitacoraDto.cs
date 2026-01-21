using System;

namespace Abstracciones.Modelos.ModelosDto
{
    public class BitacoraDto
    {
        public string IdBitacora { get; set; }
        public DateTime Fecha { get; set; }
        public string Usuario { get; set; }
        public string Accion { get; set; }
        public string Tabla { get; set; }
        public string Descripcion { get; set; }
    }
}
