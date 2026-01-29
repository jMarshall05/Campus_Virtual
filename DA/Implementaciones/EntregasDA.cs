using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.Modelos.ModelosDto;
using DA.Entidades;
using DA.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DA.Implementaciones
{
    public class EntregasDA : IEntregasAD
    {
        private readonly ApplicationDbContext _elContexto;
        public EntregasDA(ApplicationDbContext elContexto)
        {
            _elContexto = elContexto;
        }
        public async Task<int> AgregarEntrega(EntregasAD entrega)
        {
            var entidad = await _elContexto.Entregas.AddAsync(entrega);
            await _elContexto.SaveChangesAsync();
            return entidad.Entity.IdEntrega;
        }

        public async Task EditarEntrega(EntregasAD entrega)
        {
            var entidad = await _elContexto.Entregas.FindAsync(entrega);
            entidad.IdTarea = entrega.IdTarea;
            entidad.IdEstudiante = entrega.IdEstudiante;
            entidad.ArchivoEntregado = entrega.ArchivoEntregado;
            entidad.FechaEntrega = DateTime.Now;
            entidad.Estado = entrega.Estado;
            await _elContexto.SaveChangesAsync();
        }

        public async Task EliminarEntrega(int id_entrega)
        {
            var entrega = await _elContexto.Entregas.FindAsync(id_entrega);
            entrega.Estado = false;
            await _elContexto.SaveChangesAsync();
        }

        public async Task<IEnumerable<EntregasDto>> ListarEntregas()
        {
            var entregas = await _elContexto.Entregas
                .Where(e => e.Estado == true)
                .Select(e => new EntregasDto
                {
                    id_entrega = e.IdEntrega,
                    id_tarea = e.IdTarea,
                    id_estudiante = e.IdEstudiante,
                    archivo_entregado = e.ArchivoEntregado,
                    fecha_entrega = e.FechaEntrega,
                    estado = e.Estado
                }).ToListAsync();
            return entregas;
        }

        public async Task<IEnumerable<EntregasDto>> ListarEntregasPorEstudiante(string idEstudiante)
        {
            var entregas = await _elContexto.Entregas
                .Where(e => e.IdEstudiante == idEstudiante && e.Estado == true)
                .Select(e => new EntregasDto
                {
                    id_entrega = e.IdEntrega,
                    id_tarea = e.IdTarea,
                    id_estudiante = e.IdEstudiante,
                    archivo_entregado = e.ArchivoEntregado,
                    fecha_entrega = e.FechaEntrega,
                    estado = e.Estado
                }).ToListAsync();
            return entregas;
        }

        public async Task<IEnumerable<EntregasDto>> ListarEntregasPorGrupo(int idGrupo)
        {
            var entregas = await _elContexto.Entregas
                .Where(e =>  _elContexto.Tareas
                .Any(t => t.IdGrupo == idGrupo && t.IdTarea == e.IdTarea))
                .Select(e => new EntregasDto
                {
                    id_entrega = e.IdEntrega,
                    id_tarea = e.IdTarea,
                    id_estudiante = e.IdEstudiante,
                    archivo_entregado = e.ArchivoEntregado,
                    fecha_entrega = e.FechaEntrega,
                    estado = e.Estado
                }).ToListAsync();
            return entregas;
        }
    }
}
