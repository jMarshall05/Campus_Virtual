using System.ComponentModel;

namespace Abstracciones.Modelos.ModelosDto
{
    public class TelefonoDto
    {
        [DisplayName("Id")]
        public int Id { get; set; }
        [DisplayName("IdUsuario")]
        public string IdUsuario { get; set; }
        [DisplayName("Código")]
        public int Codigo { get; set; }
        [DisplayName("Telefono")]
        public int Telefono { get; set; }
        [DisplayName("Tipo")]
        public string Tipo { get; set; }
        [DisplayName("Estado")]
        public bool Estado { get; set; } = true;
    }
}
