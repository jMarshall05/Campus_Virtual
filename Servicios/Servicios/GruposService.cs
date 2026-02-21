using Abstracciones.Modelos.ModelosDto;
using Abstracciones.Servicios;
using DA.Entidades;
using DA.Interfaces;
using Mapster;
using MapsterMapper;
using Reglas;
using static Abstracciones.Modelos.Requests.GruposRequests;

namespace Servicios.Servicios
{
    public class GruposService : IGruposService
    {
        private readonly IGruposDA _grupos;
        private readonly IUsuariosService _usuarios;
        private readonly IMapper _mapper;
        private readonly IExportService _exportar;
        public GruposService(IExportService exportar,IGruposDA grupos, IUsuariosService usuarios, IMapper mapper)
        {
            _grupos = grupos;
            _usuarios = usuarios;
            _mapper = mapper;
            _exportar = exportar;
        }

        public async Task<int> AgregarGrupo(string idUsuario, AgregarGrupoRequest request)
        {
            var usuario = await _usuarios.ObtenerUsuarioPorId(idUsuario);
            UsuarioReglas.ValidarUsuario(usuario != null);
            var grupo = request.Adapt<GruposDto>();
            grupo.creado_por = $"{usuario.Nombre} {usuario.Apellido}";
            grupo.FechaDeCreacion = DateTime.Now;
            var resultado = await _grupos.AgregarGrupo(grupo.Adapt<GruposAD>());
            return resultado;

        }

        public Task<GruposDto> BuscarGruposPorId(int idGrupo)
        {
            var grupo = _grupos.BuscarGruposPorId(idGrupo);
            return grupo;
        }

        public async Task EditarGrupo(string idUsuario, EditarGrupoRequest request)
        {
            var grupoExiste = await BuscarGruposPorId(request.idGrupo) != null;
            GruposReglas.ExisteGrupo(grupoExiste);
            var usuario = await _usuarios.ObtenerUsuarioPorId(idUsuario);
            UsuarioReglas.ValidarUsuario(usuario != null);
            var grupo = request.Adapt<GruposDto>();
            grupo.modificado_por = $"{usuario.Nombre} {usuario.Apellido}";
            await _grupos.EditarGrupo(grupo.Adapt<GruposAD>());

        }

        public async Task<IEnumerable<GruposDto>> ListarGrupos()
        {
            var grupos =await _grupos.ListarGrupos();
            return grupos;
        }
        public byte[] QrExportar(string url)
        {
            var qr = _exportar.ExportarReporteQr(url);
            return qr;
        }
    }
}
