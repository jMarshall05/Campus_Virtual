using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.DA;
using Abstracciones.Modelos.ModelosDA;
using Abstracciones.Modelos.ModelosDto;
using Microsoft.EntityFrameworkCore;

namespace DA
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
            int resultado = await _elContexto.SaveChangesAsync();
            if (resultado > 0)
                return entidad.Entity.id_grupo;
            return 0;
        }

        public async Task<GruposDto> BuscarGruposPorId(int idGrupo)
        {
            var grupo = await _elContexto.Grupos.FindAsync(idGrupo);
            if (grupo == null)
                return null;
            var grupoDto = new GruposDto
            {
                id_grupo = grupo.id_grupo,
                nombre_grupo = grupo.nombre_grupo,
                descripcion = grupo.descripcion,
                creado_por = grupo.creado_por,
                estado = grupo.estado,
                FechaDeCreacion = grupo.FechaDeCreacion,
                FechaDeModificacion = grupo.FechaDeModificacion,
                modificado_por = grupo.modificado_por
            };
            return grupoDto;
        }

        public async Task<bool> EditarGrupo(int id, GruposAD grupo)
        {
            var grupoExistente = await _elContexto.Grupos.FindAsync(id);
            if (grupoExistente == null)
                return false;
            grupoExistente.nombre_grupo = grupo.nombre_grupo;
            grupoExistente.descripcion = grupo.descripcion;
            grupoExistente.modificado_por = grupo.modificado_por;
            grupoExistente.FechaDeModificacion = DateTime.Now;
            grupoExistente.estado = grupo.estado;
            int resultado = await _elContexto.SaveChangesAsync();
            return resultado > 0;
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
