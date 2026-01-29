namespace Abstracciones.Modelos.Requests
{
    public class TareasRequests
    {
        public class AgregarTareaRequest
        {

            public string Titulo { get; set; }
            public string Descripcion { get; set; }
            public DateTime FechaEntrega { get; set; }
            public string? ArchivoAdjunto { get; set; }
            public int IdGrupo { get; set; }
            public int IdMateria { get; set; }
            public string asignado_por { get; set; }
        }
        public class EditarTareaRequest
        {
            public string Titulo { get; set; }
            public string Descripcion { get; set; }
            public DateTime FechaEntrega { get; set; }
            public string? ArchivoAdjunto { get; set; }
            public int IdGrupo { get; set; }
        }
    }
}
