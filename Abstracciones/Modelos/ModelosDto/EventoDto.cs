using System.ComponentModel;

namespace Abstracciones.Modelos.ModelosDto
{
    public class EventoDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        public string IdUsuario { get; set; }

        [DisplayName("Estado")]
        public bool Estado { get; set; } = true;
    }
}
