using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.Modelos.ModelosDto;
using DA.Entidades;

namespace DA.Interfaces
{
    public interface ICursosAD
    {
        Task<int> AgregarCurso(CursosAD curso);
        Task<IEnumerable<CursoDto>> ListarCursos();
        Task<CursoDto> ObtenerPorId(int idCurso);
        Task ModificarEstadoCurso(int idCurso);

    }
}
