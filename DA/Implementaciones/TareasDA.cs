using Abstracciones.Modelos.ModelosDto;
using DA.Entidades;
using DA.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DA.Implementaciones
{
    public class TareasDA : ITareasAD
    {
        private readonly ApplicationDbContext _elContexto;
        public TareasDA(ApplicationDbContext elContexto)
        {
            _elContexto = elContexto;
        }
        public async Task<int> AgregarTarea(TareasAD tarea)
        {
            tarea.FechaPublicacion = DateTime.UtcNow;
            tarea.Estado = true;
            var entidad = await _elContexto.Tareas.AddAsync(tarea);
            await _elContexto.SaveChangesAsync();
            return entidad.Entity.IdTarea;
        }

        public async Task EditarTarea(int id, TareasAD tarea)
        {
            var tareaExistente = await _elContexto.Tareas.FindAsync(id);
            tareaExistente.Titulo = tarea.Titulo;
            tareaExistente.Descripcion = tarea.Descripcion;
            tareaExistente.FechaEntrega = tarea.FechaEntrega;
            tareaExistente.ArchivoAdjunto = tarea.ArchivoAdjunto;
            tareaExistente.FechaModificacion = DateTime.UtcNow;
            tareaExistente.IdGrupo = tarea.IdGrupo;
            await _elContexto.SaveChangesAsync();

        }

        public async Task CambiarEstadoTarea(int idTarea)
        {
            var tareaExistente = await _elContexto.Tareas.FindAsync(idTarea);
            tareaExistente.Estado = !tareaExistente.Estado;
            await _elContexto.SaveChangesAsync();

        }

        public async Task<IEnumerable<TareaDto>> ListarTareas()
        {
            var tareas = await _elContexto.Tareas
                 .Include(t => t.Grupo)
                 .Select(t => new TareaDto
                 {
                     IdTarea = t.IdTarea,
                     Titulo = t.Titulo,
                     Descripcion = t.Descripcion,
                     FechaEntrega = t.FechaEntrega,
                     IdMateria = t.IdMateria,
                     ArchivoAdjunto = t.ArchivoAdjunto,
                     FechaModificacion = t.FechaModificacion,
                     FechaPublicacion = t.FechaPublicacion,
                     idGrupo = t.IdGrupo,
                     Grupo = new GruposDto
                     {
                         idGrupo = t.Grupo.idGrupo,
                         Nombre = t.Grupo.Nombre
                     },
                     Estado = t.Estado
                 })
                 .ToListAsync();
            return tareas;
        }

        public async Task<List<TareaDto>> ListarTareasPorGrupo(int IdGrupo)
        {
            var tareas = await _elContexto.Tareas
                   .Where(t => t.IdGrupo == IdGrupo)
                   .Include(t => t.Grupo)
                   .Select(t => new TareaDto
                   {
                       IdTarea = t.IdTarea,
                       Titulo = t.Titulo,
                       Descripcion = t.Descripcion,
                       FechaEntrega = t.FechaEntrega,
                       IdMateria = t.IdMateria,
                       ArchivoAdjunto = t.ArchivoAdjunto,
                       FechaModificacion = t.FechaModificacion,
                       FechaPublicacion = t.FechaPublicacion,
                       idGrupo = t.IdGrupo,
                       Grupo = new GruposDto
                       {
                           idGrupo = t.Grupo.idGrupo,
                           Nombre = t.Grupo.Nombre
                       },
                       Estado = t.Estado
                   })
                   .ToListAsync();
            return tareas;
        }

        public async Task<TareaDto> ObtenerPorId(int idTarea)
        {
            var tarea = await _elContexto.Tareas
                .Where(t => t.IdTarea == idTarea)
                .Include(t => t.Grupo)
                .Select(t => new TareaDto
                {
                    IdTarea = t.IdTarea,
                    Titulo = t.Titulo,
                    Descripcion = t.Descripcion,
                    FechaEntrega = t.FechaEntrega,
                    IdMateria = t.IdMateria,
                    ArchivoAdjunto = t.ArchivoAdjunto,
                    FechaModificacion = t.FechaModificacion,
                    FechaPublicacion = t.FechaPublicacion,
                    idGrupo = t.IdGrupo,
                    Grupo = new GruposDto
                    {
                        idGrupo = t.Grupo.idGrupo,
                        Nombre = t.Grupo.Nombre
                    },
                    Estado = t.Estado
                })
                .FirstOrDefaultAsync();
            return tarea;
        }
    }
}
