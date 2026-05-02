using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.Modelos.ModelosDto;

namespace Abstracciones.Modelos.Requests
{
    public class CursosRequest
    {
        public class AgregarCursoRequest
        {
            public int MateriaId { get; set; }
            public string IdProfesor { get; set; }
            public int GrupoId { get; set; }
            public bool Estado { get; set; } = true;
        }
    }
}
