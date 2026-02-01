using System.Data;
using System.Globalization;
using System.Text;
using Abstracciones.Excepciones;
using Abstracciones.Modelos.ModelosDto;
using Abstracciones.Modelos.Requests;
using Abstracciones.Modelos.Responses;
using Abstracciones.Servicios;
using DA;
using DA.Entidades;
using DA.Interfaces;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Reglas;
using static Abstracciones.Modelos.Requests.UsuariosRequests;
using static Abstracciones.Modelos.Responses.AuthResponses;

namespace Servicios.Servicios
{

    public class UsuariosService : IUsuariosService
    {
        private readonly IUsuariosDA _usuariosDA;
        private readonly ITelefonosDA _telefonosDA;//Cambiar por service
        private readonly IEstudianteGrupoDA _estudianteGrupo;//Cambiar por service
        private readonly IGruposDA _grupos;//Cambiar por service
        private readonly ITokenService _TokenService;
        private readonly IMapper _mapper;

        public UsuariosService(ITokenService tokenService, IMapper mapper, IUsuariosDA usuariosDA, ITelefonosDA telefonosDA, IEstudianteGrupoDA estudianteGrupo, IGruposDA grupos)
        {
            _usuariosDA = usuariosDA;
            _telefonosDA = telefonosDA;
            _estudianteGrupo = estudianteGrupo;
            _grupos = grupos;
            _mapper = mapper;
            _TokenService = tokenService;

        }
        public async Task<string> AgregarUsuario(RegisterRequest request)
        {

            var usuarioExiste = await _usuariosDA.ObtenerUsuarioIdentityPorEmail(request.Email);
            if (usuarioExiste != null)
            {
                throw new BusinessException("El usuario ya existe");
            }
            var identificacionExiste = await _usuariosDA.ExisteIdentificacion(request.Identificacion);
            UsuarioReglas.ValidarIdentificacionUnica(identificacionExiste);

            foreach (var telefono in request.Telefonos ?? new List<TelefonoDto>())
            {
                var telefonoExiste = await _telefonosDA.ExisteTelefono(telefono.Codigo, telefono.Telefono);
                UsuarioReglas.ValidarTelefonoUnico(telefonoExiste, telefono);

            }
            var usuarioAD = CrearUsuario(request);
            var user = await _usuariosDA.AgregarUsuario(usuarioAD, request.Password);

            request.Telefonos?.ForEach(t => t.IdUsuario = user);
            var telefonosAD = _mapper.Map<IEnumerable<TelefonoAD>>(request.Telefonos);
            if (request.Telefonos != null)
                await _telefonosDA.AgregarTelefono(telefonosAD);
            return user;

        }

        public async Task EditarUsuario(string id, EditarUsuarioRequest request)
        {
            var usuarioExiste = await _usuariosDA.ObtenerUsuarioIdentityPorId(id) ?? throw new BusinessException("El usuario no existe");
            var usuarioAD = _mapper.Map<UsuariosAD>(request);
            await _usuariosDA.EditarUsuario(id, usuarioAD);
            if (request.Telefonos != null)
            {
                var telefonosValidos = request.Telefonos
                               .Where(t => t.Telefono > 0 && !string.IsNullOrWhiteSpace(t.Tipo))
                               .ToList();

                var telefonosExistentes = _mapper.Map<IEnumerable<TelefonoAD>>(telefonosValidos.Where(t => t.Id > 0).ToList());

                if (telefonosExistentes.Any())
                {
                    await _telefonosDA.EditarTelefono(telefonosExistentes);

                }
            }
        }

        public async Task EditarUsuarioAdmin(string id, EditarUsuarioAdminRequest usuario, int? Idgrupo)
        {
            var user = await _usuariosDA.ObtenerUsuarioIdentityPorId(id);
            UsuarioReglas.ValidarUsuario(user != null);
            if (Idgrupo != null)
            {
                var existe = await _grupos.BuscarGruposPorId((int)Idgrupo) != null;
                EstudianteGrupoReglas.ExisteGrupo(existe);
            }

            if (user.Email != usuario.Email)
            {
                var userEmail = await _usuariosDA.ObtenerUsuarioIdentityPorEmail(usuario.Email);
                if (userEmail != null && userEmail.Id != id)
                {
                    throw new BusinessException("El email ya está en uso por otro usuario");
                }
            }
            var telefonosValidos = usuario.Telefonos
                        .Where(t => !string.IsNullOrWhiteSpace(t.Telefono.ToString()) && !string.IsNullOrWhiteSpace(t.Tipo))
                        .ToList();
            var telefonosExistentes = telefonosValidos.Where(t => t.Id > 0).ToList();
            if (telefonosExistentes.Count != 0)
            {
                await _telefonosDA.EditarTelefono(_mapper.Map<IEnumerable<TelefonoAD>>(telefonosExistentes));
            }
            var telefonosNuevos = telefonosValidos.Where(t => t.Id == 0).ToList();
            if (telefonosNuevos.Count > 0)
            {
                telefonosNuevos.ForEach(t => t.IdUsuario = id);
                await _telefonosDA.AgregarTelefono(_mapper.Map<IEnumerable<TelefonoAD>>(telefonosNuevos));
            }
            if (Idgrupo != null)
            {
                var estudianteGrupo = await _estudianteGrupo.BuscarEstudianteGrupoPorEstudianteId(id);
                var estudiante = estudianteGrupo.Adapt<EstudianteGrupoAD>();

                if (estudianteGrupo == null)
                {
                    await _estudianteGrupo.AgregarEstudianteGrupo(estudiante);
                }
                else
                {
                    await _estudianteGrupo.ActualizarEstudianteGrupo(estudiante);
                }
            }
            var usuarioAD = _mapper.Map<UsuariosAD>(usuario);
            await _usuariosDA.EditarUsuarioAdmin(id, usuarioAD);

        }

        public async Task<IEnumerable<UsuariosDto>> ListarUsuarios()
        {
            var usuarios = await _usuariosDA.ListarUsuarios();
            return usuarios ?? [];
        }

        public async Task<string> Login(LoginRequest login)
        {
            var usuario =await _usuariosDA.Login(login);
            var Token = _TokenService.CrearToken(usuario);
            return Token;
        }

        public async Task<UsuariosDto> ObtenerUsuarioPorId(string idUsuario)
        {
            var usuario = await _usuariosDA.ObtenerUsuarioPorId(idUsuario);
            return usuario;
        }

        private UsuariosAD CrearUsuario(RegisterRequest register)
        {
            var usuario = _mapper.Map<UsuariosAD>(register);
            usuario.FechaDeRegistro = DateTime.UtcNow;
            usuario.Estado = true;
            return usuario;
        }

    }

}

