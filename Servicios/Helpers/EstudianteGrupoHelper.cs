using DA.Entidades;
using DA.Interfaces;

namespace Servicios.Helpers
{
    public class EstudianteGrupoHelper : IEstudianteGrupoHelper
    {
        private readonly IEstudianteGrupoDA _estudianteGrupo;
        public EstudianteGrupoHelper(IEstudianteGrupoDA estudianteGrupoService)
        {
            _estudianteGrupo = estudianteGrupoService;
        }

        public async Task AddOrEdit(string id, int? Idgrupo)
        {
            var estudianteGrupo = await _estudianteGrupo.BuscarEstudianteGrupoPorEstudianteId(id);
            var estudiante = new EstudianteGrupoAD { EstudianteId = id, GrupoId = (int)Idgrupo };

            if (estudianteGrupo == null)
            {
                await _estudianteGrupo.AgregarEstudianteGrupo(estudiante);
            }
            else
            {
                await _estudianteGrupo.ActualizarEstudianteGrupo(estudiante);
            }
        }
    }
}
