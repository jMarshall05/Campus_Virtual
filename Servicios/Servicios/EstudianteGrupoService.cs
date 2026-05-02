using Abstracciones.Modelos.ModelosDto;
using Abstracciones.Servicios;
using DA.Entidades;
using DA.Interfaces;
using Mapster;
using Reglas;

namespace Servicios.Servicios
{
    public class EstudianteGrupoService : IEstudianteGrupoService
    {
        private readonly IEstudianteGrupoDA _estudianteGrupo;
        private readonly IUsuariosService _usuarios;
        private readonly IGruposDA _grupos;

        public EstudianteGrupoService(IEstudianteGrupoDA estudianteGrupoService, IGruposDA grupos, IUsuariosService usuarios)
        {
            _estudianteGrupo = estudianteGrupoService;
            _usuarios = usuarios;
            _grupos = grupos;
        }

        public async Task ActualizarEstudianteGrupo(EstudianteGrupoDto estudiante)
        {
            var existeEstudiante = await _usuarios.ObtenerUsuarioPorId(estudiante.EstudianteId) != null;
            UsuarioReglas.ValidarUsuario(existeEstudiante);

            var existeGrupo = await _grupos.BuscarGruposPorId(estudiante.GrupoId) != null;
            GruposReglas.ExisteGrupo(existeGrupo);

            await _estudianteGrupo.ActualizarEstudianteGrupo(estudiante.Adapt<EstudianteGrupoAD>());

        }

        public async Task<int> AgregarEstudianteGrupo(EstudianteGrupoDto estudiante)
        {
            var existeEstudiante = await _usuarios.ObtenerUsuarioPorId(estudiante.EstudianteId) != null;
            UsuarioReglas.ValidarUsuario(existeEstudiante);

            var existeGrupo = await _grupos.BuscarGruposPorId(estudiante.GrupoId) != null;
            GruposReglas.ExisteGrupo(existeGrupo);

            var resultado = await _estudianteGrupo.AgregarEstudianteGrupo(estudiante.Adapt<EstudianteGrupoAD>());
            return resultado;
        }

        public async Task<EstudianteGrupoDto> BuscarEstudianteGrupoPorEstudianteId(string idEstudiante)
        {
            var existeEstudiante = await _usuarios.ObtenerUsuarioPorId(idEstudiante) != null;
            UsuarioReglas.ValidarUsuario(existeEstudiante); ;

            var estudianteGrupo = await _estudianteGrupo.BuscarEstudianteGrupoPorEstudianteId(idEstudiante);
            return estudianteGrupo;
        }

        public async Task<IEnumerable<EstudianteGrupoDto>> BuscarEstudianteGrupoPorGrupoId(int idGrupo)
        {
            var existeGrupo = await _grupos.BuscarGruposPorId(idGrupo) != null;
            GruposReglas.ExisteGrupo(existeGrupo); ;

            var estudianteGrupo = await _estudianteGrupo.BuscarEstudianteGrupoPorGrupoId(idGrupo);
            return estudianteGrupo;
        }

        public async Task<IEnumerable<EstudianteGrupoDto>> ListarEstudiantesGrupos()
        {
            var lista = await _estudianteGrupo.ListarEstudiantesGrupos();
            return lista;
        }

        public async Task<IEnumerable<EstudianteGrupoDto>> ListarEstudiantesPorIdGrupo(int idGrupo)
        {
            var lista = await _estudianteGrupo.ListarEstudiantesPorIdGrupo(idGrupo);
            return lista;
        }

        public async Task<IEnumerable<EstudianteGrupoDto>> ListarGruposPorIdEstudiante(string idUsuario)
        {
            var lista = await _estudianteGrupo.ListarGruposPorIdEstudiante(idUsuario);
            return lista;
        }
    }
}
