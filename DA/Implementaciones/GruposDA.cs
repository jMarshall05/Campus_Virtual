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
    public class GruposDA : IGruposDA
    {
        private readonly ApplicationDbContext _elContexto;
        public GruposDA(ApplicationDbContext elContexto)
        {
            _elContexto = elContexto;
        }
        public async Task<int> AgregarGrupo(GruposAD grupo)
        {
            var entidad = await _elContexto.Grupos.AddAsync(grupo);
            await _elContexto.SaveChangesAsync();
            return entidad.Entity.id_grupo;
        }

        public async Task<GruposDto> BuscarGruposPorId(int idGrupo)
        {
            var grupo = await _elContexto.Grupos.Where(g=>g.id_grupo == idGrupo)
                .Select(grupo => new GruposDto
                {
                    id_grupo = grupo.id_grupo,
                    nombre_grupo = grupo.nombre_grupo,
                    descripcion = grupo.descripcion,
                    creado_por = grupo.creado_por,
                    estado = grupo.estado,
                    FechaDeCreacion = grupo.FechaDeCreacion,
                    FechaDeModificacion = grupo.FechaDeModificacion,
                    modificado_por = grupo.modificado_por
                }).FirstOrDefaultAsync();
            return grupo;
        }

        public async Task EditarGrupo(int id, GruposAD grupo)
        {
            var grupoExistente = await _elContexto.Grupos.FindAsync(id);
            grupoExistente.nombre_grupo = grupo.nombre_grupo;
            grupoExistente.descripcion = grupo.descripcion;
            grupoExistente.modificado_por = grupo.modificado_por;
            grupoExistente.FechaDeModificacion = DateTime.Now;
            grupoExistente.estado = grupo.estado;
            await _elContexto.SaveChangesAsync();
        }

        public async Task<IEnumerable<GruposDto>> ListarGrupos()
        {
            var grupos = await _elContexto.Grupos.Select(grupo => new GruposDto
            {
                id_grupo = grupo.id_grupo,
                nombre_grupo = grupo.nombre_grupo,
                descripcion = grupo.descripcion,
                creado_por = grupo.creado_por,
                estado = grupo.estado,
                FechaDeCreacion = grupo.FechaDeCreacion,
                FechaDeModificacion = grupo.FechaDeModificacion,
                modificado_por = grupo.modificado_por
            }).ToListAsync();
            return grupos;
        }
    }
}
