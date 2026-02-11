using System.Data;
using System.Net;
using Abstracciones.Excepciones;
using Abstracciones.Modelos.ModelosDto;
using Abstracciones.Modelos.Requests;
using Abstracciones.Servicios;
using Abstracciones.Servicios.Helpers;
using DA.Entidades;
using DA.Interfaces;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using OtpNet;
using QRCoder;
using Reglas;
using Servicios.Helpers;
using static Abstracciones.Modelos.Requests.UsuariosRequests;
using static Abstracciones.Modelos.Responses.AuthResponses;

namespace Servicios.Servicios
{

    public class UsuariosService : IUsuariosService
    {
        private readonly IUsuariosDA _usuariosDA;
        private readonly ITelefonosService _telefonos;
        private readonly IEstudianteGrupoHelper _estudianteGrupo;
        private readonly IGruposHelper _grupos;
        private readonly ITokenService _TokenService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuracion;
        private readonly SecretProtectorService _secrets;

        public UsuariosService(IConfiguration configuration, ITokenService tokenService, IMapper mapper, IUsuariosDA usuariosDA, ITelefonosService telefonos, IEstudianteGrupoHelper estudianteGrupo, IGruposHelper grupos, SecretProtectorService secrets)
        {
            _usuariosDA = usuariosDA;
            _telefonos = telefonos;
            _estudianteGrupo = estudianteGrupo;
            _grupos = grupos;
            _mapper = mapper;
            _TokenService = tokenService;
            _secrets = secrets;
            _configuracion = configuration;

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

            foreach (var telefono in request.Telefonos)
            {
                var telefonoExiste = await _telefonos.ExisteTelefono(telefono.Codigo, telefono.Telefono);
                UsuarioReglas.ValidarTelefonoUnico(telefonoExiste, telefono);
            }
            var usuarioAD = CrearUsuario(request);
            var user = await _usuariosDA.AgregarUsuario(usuarioAD, request.Password);

            request.Telefonos?.ForEach(t => t.IdUsuario = user);
            if (request.Telefonos != null)
                await _telefonos.AgregarTelefono(request.Telefonos);
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

                var telefonosExistentes = telefonosValidos.Where(t => t.Id > 0).ToList();

                if (telefonosExistentes.Any())
                {
                    await _telefonos.EditarTelefono(telefonosExistentes);

                }
            }
        }

        public async Task EditarUsuarioAdmin(string id, EditarUsuarioAdminRequest usuario, int? Idgrupo)
        {
            var user = await _usuariosDA.ObtenerUsuarioIdentityPorId(id);
            UsuarioReglas.ValidarUsuario(user != null);
            if (Idgrupo != null)
            {
                var existe = await _grupos.BuscarGrupoPorId((int)Idgrupo) != null;
                GruposReglas.ExisteGrupo(existe);
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
                await _telefonos.EditarTelefono(telefonosExistentes);
            }
            var telefonosNuevos = telefonosValidos.Where(t => t.Id == 0).ToList();
            if (telefonosNuevos.Count > 0)
            {
                telefonosNuevos.ForEach(t => t.IdUsuario = id);
                await _telefonos.AgregarTelefono(telefonosNuevos);
            }
            if (Idgrupo != null)
            {
                await _estudianteGrupo.AddOrEdit(id, Idgrupo);
            }
            var usuarioAD = _mapper.Map<UsuariosAD>(usuario);
            await _usuariosDA.EditarUsuarioAdmin(id, usuarioAD);

        }

        public Task<IEnumerable<UsuariosDto>> ListarPorRol(string rol)
        {
            var usuarios = _usuariosDA.ListarPorRol(rol);
            return usuarios;
        }

        public async Task<IEnumerable<UsuariosDto>> ListarUsuarios()
        {
            var usuarios = await _usuariosDA.ListarUsuarios();
            return usuarios ?? [];
        }

        public async Task<string> Login(LoginRequest login)
        {
            var usuario = await _usuariosDA.Login(login);
            if (usuario != null)
            {
                var Token = _TokenService.CrearToken(usuario);
                return Token;
            }
            return null;
        }

        public async Task<TwofaResponse> EnableAuthenticator(string userId)
        {
            var user = await _usuariosDA.ObtenerUsuarioIdentityPorId(userId);

            if (!string.IsNullOrEmpty(user.GoogleAuthenticatorSecretTemp))
            {
                var existingSecret = _secrets.Unprotect(user.GoogleAuthenticatorSecretTemp);

                return GenerarQr(existingSecret, user.Email);
            }

            var secret = Base32Encoding.ToString(KeyGeneration.GenerateRandomKey(20));
            var googleKey = _secrets.Protect(secret);

            await _usuariosDA.EnableAuthenticator(userId, googleKey);

            return GenerarQr(secret, user.Email);
        }

        private TwofaResponse GenerarQr(string secret, string email)
        {
            var issuerRaw = _configuracion["keys:AppName"];
            var issuer = WebUtility.UrlEncode(issuerRaw);
            var emailEncoded = WebUtility.UrlEncode(email);

            var otpauthUrl = $"otpauth://totp/{issuer}:{emailEncoded}?secret={secret}&issuer={issuer}";

            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(otpauthUrl, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new PngByteQRCode(qrCodeData);
            var qrCodeImage = qrCode.GetGraphic(20);

            return new TwofaResponse
            {
                Qr = Convert.ToBase64String(qrCodeImage),
                SecretKey = secret
            };
        }

        public async Task<string> VerifyTwoFa(string idusuario, string code)
        {
            var user =await _usuariosDA.ObtenerUsuarioIdentityPorId(idusuario);
            var secret = _secrets.Unprotect(user.GoogleAuthenticatorSecretTemp);
            var totp = new Totp(Base32Encoding.ToBytes(secret));
            var serverCode = totp.ComputeTotp();
            code = code?.Trim();
            var isValid = totp.VerifyTotp(code.Trim(), out long _, new VerificationWindow(previous: 2, future: 2));


            if (!isValid)
            {
                throw new BusinessException("Código incorrecto");
            }
            var tokenRequest = await _usuariosDA.VerifyTwoFa(idusuario);
            var token = _TokenService.CrearToken(tokenRequest);
            return token;
        }
        public async Task<string> DisableAuthenticator(string IdUsuario)
        {
            var existe = await _usuariosDA.ObtenerUsuarioIdentityPorId(IdUsuario) != null;
            UsuarioReglas.ValidarUsuario(existe);
            var respuesta = await _usuariosDA.DisableAuthenticator(IdUsuario);
            var token = _TokenService.CrearToken(respuesta);
            return token;
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

