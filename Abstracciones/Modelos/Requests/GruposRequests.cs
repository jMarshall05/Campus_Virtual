namespace Abstracciones.Modelos.Requests
{
    public class GruposRequests
    {
        public class AgregarGrupoRequest
        {
            public string Nombre { get; set; }
            public string Descripcion { get; set; }
        }
        public class EditarGrupoRequest
        {
            public int IdGrupo { get; set; }
            public string Nombre { get; set; }
            public string Descripcion { get; set; }
        }
    }
}
