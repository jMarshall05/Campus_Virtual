namespace Abstracciones.Modelos.ModelosDto
{
    public class TelefonoDto
    {
        public int? Id { get; set; }
        public string? IdUsuario { get; set; }
        public int Codigo { get; set; }
        public long Telefono { get; set; }
        public string Tipo { get; set; }
        public bool Estado { get; set; } = true;
    }
}
