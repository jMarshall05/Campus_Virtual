using Abstracciones.Api;
using Abstracciones.Servicios;
using Microsoft.AspNetCore.Mvc;
using static Abstracciones.Modelos.Requests.CursosRequest;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/courses")]
    public class CursosController : ControllerBase, ICursosController
    {
        private readonly ICursosService _cursos;
        public CursosController(ICursosService cursos)
        {
            _cursos = cursos;
        }

        [HttpPost]
        public async Task<IActionResult> AgregarCurso(AgregarCursoRequest request)
        {
            var response = await _cursos.AgregarCurso(request);
            return Ok($"Se agregó correctamente el curso, id: {response}");
        }

        [HttpGet]
        public async Task<IActionResult> ListarCursos()
        {
            var cursos = await _cursos.ListarCursos();
            return Ok(cursos);
        }

        [HttpPatch("{idCurso}")]
        public async Task<IActionResult> ModificarEstadoCurso(int idCurso)
        {
            await _cursos.ModificarEstadoCurso(idCurso);
            return Ok();
        }

        [HttpGet("{idCurso}")]
        public async Task<IActionResult> ObtenerPorId(int idCurso)
        {
            var curso = await _cursos.ObtenerPorId(idCurso);
            return Ok(curso);
        }

        [HttpGet("exportarPdf")]
        public async Task<IActionResult> ExportarCursosPDF()
        {
            var pdf = await _cursos.ExportarCursosPDF(null);
            if (pdf != null)
                return File(pdf, "application/pdf");
            return NoContent();
        }
    }
}
