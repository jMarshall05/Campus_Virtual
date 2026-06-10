using Abstracciones.Api;
using Abstracciones.Modelos.Requests;
using Abstracciones.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/materias")]
    public class MateriasController : ControllerBase, IMateriaController
    {
        private readonly IMateriasService _materias;
        public MateriasController(IMateriasService materias)
        {
            _materias = materias;
        }

        [HttpPost]
        public async Task<IActionResult> AgregarMateria(MateriaRequests materia)
        {
            var resultado = await _materias.AgregarMateria(materia);
            return Ok($"Se a agregado la materia con id: {resultado}");
        }

        [HttpPatch("{materiaId}/estado")]
        public async Task<IActionResult> CambiarEstadoMateria(int materiaId)
        {
            await _materias.CambiarEstadoMateria(materiaId);
            return Ok($"Estado de la materia de id {materiaId} cambiado");
        }

        [HttpPut("{IdMateria}")]
        public async Task<IActionResult> EditarMateria(int IdMateria, MateriaRequests materia)
        {
            await _materias.EditarMateria(IdMateria, materia);
            return Ok($"Materia de id {IdMateria} editada");
        }

        [HttpGet]
        public async Task<IActionResult> ListarMaterias()
        {
            var lista = await _materias.ListarMaterias();
            return Ok(lista);
        }

        [HttpGet("ByName")]
        public async Task<IActionResult> ObtenerMateriaNombre([FromQuery] string nombre)
        {
            var lista = await _materias.ObtenerMateriaNombre(nombre);
            return Ok(lista);
        }

        [HttpGet("{idMateria}")]
        public async Task<IActionResult> ObtenerMateriaPorId(int idMateria)
        {
            var lista = await _materias.ObtenerMateriaPorId(idMateria);
            return Ok(lista);
        }
    }
}
