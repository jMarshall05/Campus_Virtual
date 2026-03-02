using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.Modelos.ModelosDto;
using Microsoft.AspNetCore.Mvc;

namespace Abstracciones.Api
{
    public interface ICursosController
    {
        Task<IActionResult> AgregarCurso(CursoDto curso);
        Task<IActionResult> ListarCursos();
        Task<IActionResult> ObtenerPorId(int idCurso);
        Task<IActionResult> ModificarEstadoCurso(int idCurso);
    }
}
