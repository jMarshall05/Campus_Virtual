namespace Abstracciones.Modelos.ModelosDto
{
    public class UsuariosDto
    {
        public string IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public List<TelefonoDto> Telefonos { get; set; }
        public DateTime FechaDeNacimiento { get; set; }
        public string Identificacion { get; set; }
        public DateTime FechaDeRegistro { get; set; }
        public DateTime? FechaDeModificacion { get; set; }
        public string Rol { get; set; }
        public string TipoIdentificacion { get; set; }
        public bool Estado { get; set; }

        public virtual GruposDto? Grupo { get; set; }

    }
}
