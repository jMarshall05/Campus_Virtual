using Abstracciones.Modelos.ModelosDto;
using DA.Entidades;
using DA.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DA.Implementaciones
{
    public class CalificacionesDA : ICalificacionesAD
    {
        private readonly ApplicationDbContext _elContexto;
        public CalificacionesDA(ApplicationDbContext contexto)
        {
            _elContexto = contexto;
        }
        public async Task<int> AgregarCalificacion(CalificacionesAD calificacion)
        {
            var entidad = await _elContexto.Calificaciones.AddAsync(calificacion);
            await _elContexto.SaveChangesAsync();
            return entidad.Entity.IdCalificacion;
        }

        public async Task EditarCalificacion(CalificacionesAD calificacion)
        {
            var calificacionExistente = await _elContexto.Calificaciones.FindAsync(calificacion.IdCalificacion);
            if (calificacionExistente != null)
            {
                calificacionExistente.IdEntrega = calificacion.IdEntrega;
                calificacionExistente.Calificacion = calificacion.Calificacion;
                calificacionExistente.Comentario = calificacion.Comentario;
                calificacionExistente.FechaCalificacion = calificacion.FechaCalificacion;
                calificacionExistente.Estado = calificacion.Estado;

                await _elContexto.SaveChangesAsync();
            }
        }

        public async Task EliminarCalificacion(int id_calificacion)
        {
            var calificacionExistente = await _elContexto.Calificaciones.FindAsync(id_calificacion);
            if (calificacionExistente != null)
                calificacionExistente.Estado = !calificacionExistente.Estado;
            await _elContexto.SaveChangesAsync();
        }

        public async Task<IEnumerable<CalificacionesDto>> ListarCalificaciones()
        {
            var lista = await _elContexto.Calificaciones
                .Where(e => e.Estado == true)
                        .Select(e => new CalificacionesDto
                        {
                            id_entrega = e.IdEntrega,
                            id_calificacion = e.IdCalificacion,
                            calificacion = e.Calificacion,
                            comentario = e.Comentario,
                            fecha_calificacion = e.FechaCalificacion,
                            Estado = e.Estado
                        })
                        .ToListAsync();

            return lista;
        }

        public async Task<IEnumerable<CalificacionesDto>> ListarCalificacionesPorEstudiante(string idEstudiante)
        {
            var entregas = await _elContexto.Calificaciones
                 .Include(e => e.Entrega)
                 .Where(e => e.Entrega.IdEstudiante == idEstudiante && e.Estado == true)
                 .Select(e => new CalificacionesDto
                 {
                     id_calificacion = e.IdCalificacion,
                     id_entrega = e.IdEntrega,
                     calificacion = e.Calificacion,
                     comentario = e.Comentario,
                     fecha_calificacion = e.FechaCalificacion,
                     Estado = e.Estado,
                     Entrega = e.Entrega == null ? null : new EntregasDto
                     {
                         id_entrega = e.Entrega.IdEntrega,
                         id_tarea = e.Entrega.IdTarea,
                         id_estudiante = e.Entrega.IdEstudiante,
                         fecha_entrega = e.Entrega.FechaEntrega,
                         archivo_entregado = e.Entrega.ArchivoEntregado,
                         estado = e.Entrega.Estado
                     }
                 }).ToListAsync();
            return entregas;


        }

        public async Task<IEnumerable<CalificacionesDto>> ListarCalificacionesPorGrupo(int idGrupo)
        {
            var calificacionesGrupo = await _elContexto.Calificaciones
                .Where(c =>
                    _elContexto.Entregas.Any(e =>
                        e.IdEntrega == c.IdEntrega &&
                        _elContexto.Tareas.Any(t =>
                            t.IdTarea == e.IdTarea &&
                            t.IdGrupo == idGrupo
                        )
                    )
                )
                .Select(c => new CalificacionesDto
                {
                    id_calificacion = c.IdCalificacion,
                    id_entrega = c.IdEntrega,
                    calificacion = c.Calificacion,
                    comentario = c.Comentario,
                    Entrega = c.Entrega == null ? null : new EntregasDto
                    {
                        id_entrega = c.Entrega.IdEntrega,
                        id_tarea = c.Entrega.IdTarea,
                        id_estudiante = c.Entrega.IdEstudiante,
                        fecha_entrega = c.Entrega.FechaEntrega,
                        archivo_entregado = c.Entrega.ArchivoEntregado,
                        estado = c.Entrega.Estado
                    },
                    fecha_calificacion = c.FechaCalificacion
                })
                .ToListAsync();
            return calificacionesGrupo;
        }
    }
}
