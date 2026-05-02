using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.Modelos.ModelosDto;
using static Abstracciones.Modelos.Requests.CursosRequest;

namespace Abstracciones.Servicios
{
    public interface ICursosService
    {
        Task<int> AgregarCurso(AgregarCursoRequest curso);
        Task<IEnumerable<CursoDto>> ListarCursos();
        Task<CursoDto> ObtenerPorId(int idCurso);
        Task ModificarEstadoCurso(int idCurso);
        Task<byte[]> ExportarCursosPDF(string? rutaLogo);
    }
}
