using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.Modelos.ModelosDto;
using Abstracciones.Modelos.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Abstracciones.Api
{
    public interface IMateriaController
    {
        Task<IActionResult> AgregarMateria(MateriaRequests materia);
        Task<IActionResult> EditarMateria(int IdMateria, MateriaRequests materia);
        Task<IActionResult> CambiarEstadoMateria(int materiaId);
        Task<IActionResult> ListarMaterias();
        Task<IActionResult> ObtenerMateriaPorId(int id);
        Task<IActionResult> ObtenerMateriaNombre(string nombre);

    }
}
