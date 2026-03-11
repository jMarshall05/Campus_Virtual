using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Abstracciones.Modelos.Requests
{
    public class DocumentosRequest
    {
        public class AgregarDocumentoRequest
        {
            public string Titulo { get; set; }
            public string Descripcion { get; set; }
            public string Categoria { get; set; }
            public IFormFile Doc { get; set; }
        }
        public class EditarDocumentoRequest : AgregarDocumentoRequest
        {
            public IFormFile? Doc { get; set; }

        }
    }
}
