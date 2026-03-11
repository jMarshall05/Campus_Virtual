namespace Abstracciones.Modelos.ModelosDto
{
    public class EntregasDto
    {
        public int id_entrega { get; set; }
        public int id_tarea { get; set; }
        public string id_estudiante { get; set; }
        public string archivo_entregado { get; set; }
        public DateTime fecha_entrega { get; set; }
        public bool estado { get; set; }
        public TareaDto Tarea { get; set; }
        public UsuariosDto Estudiante { get; set; }

        public CalificacionesDto Calificacion { get; set; } // para la calificación de la entrega
    }
}
