using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.DA;
using Abstracciones.Excepciones;
using Abstracciones.Modelos.ModelosDA;
using Abstracciones.Modelos.ModelosDto;
using Abstracciones.Modelos.Requests;
using Abstracciones.Servicios;
using DA;
using Microsoft.AspNetCore.Identity;
using Microsoft.Win32;
using Reglas;

namespace Servicios
{

    public class UsuariosService : IUsuariosService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUsuariosDA _usuariosDA;
        private readonly ITelefonosDA _telefonosDA;
        private readonly IEstudianteGrupoDA _estudianteGrupo;
        private readonly IGruposDA _grupos;

        public UsuariosService(UserManager<ApplicationUser> userManager, IUsuariosDA usuariosDA, ITelefonosDA telefonosDA, IEstudianteGrupoDA estudianteGrupo, IGruposDA grupos)
        {
            _userManager = userManager;
            _usuariosDA = usuariosDA;
            _telefonosDA = telefonosDA;
            _estudianteGrupo = estudianteGrupo;
            _grupos = grupos;
        }
        public async Task<string> AgregarUsuario(RegisterRequest request)
        {

            var usuarioExiste = await _userManager.FindByEmailAsync(request.Email);
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

            ApplicationUser user = CrearUsuario(request);
            var resultado = await _userManager.CreateAsync(user, request.Password);

            if (!resultado.Succeeded)
                throw new BusinessException(resultado.Errors.First().Description);

            await _userManager.AddToRoleAsync(user, request.Rol);

            var usuarioAD = await CrearUsuarioAD(request, user);
            await _usuariosDA.AgregarUsuario(usuarioAD);

            request.Telefonos?.ForEach(t => t.IdUsuario = usuarioAD.IdUsuario);
            var telefonosAD = ConvertirTelefonosAD(request.Telefonos ?? new List<TelefonoDto>());
            if (request.Telefonos != null)
                await _telefonosDA.AgregarTelefono(telefonosAD);
            return user.Id;

        }
        private static ApplicationUser CrearUsuario(Abstracciones.Modelos.Requests.RegisterRequest register)
        {
            string numeroRamdon = Random.Shared.Next(0, 100).ToString("D2");

            return new ApplicationUser
            {
                UserName = (register.Nombre.ToUpper().First() + register.Apellido.Trim() + numeroRamdon).Normalize(NormalizationForm.FormD)
                .Where(c => Char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                .Aggregate("", (s, c) => s + c),
                Email = register.Email,
                FechaDeRegistro = DateTime.UtcNow
            };
        }

        public async Task<bool> EditarUsuario(string id, EditarUsuarioRequest request)
        {
            try
            {
                var usuarioExiste = await _userManager.FindByIdAsync(id);
                if (usuarioExiste == null)
                {
                    throw new BusinessException("El usuario no existe");
                }
                var usuarioAD = new UsuariosAD
                {
                    Nombre = request.Nombre,
                    Apellido = request.Apellido,
                };
                var result = await _usuariosDA.EditarUsuario(id, usuarioAD);
                if (request.Telefonos != null)
                {
                    var telefonosValidos = request.Telefonos
                                   .Where(t => t.Telefono > 0 && !string.IsNullOrWhiteSpace(t.Tipo))
                                   .ToList();

                    var telefonosExistentes = ConvertirTelefonosAD(telefonosValidos.Where(t => t.Id > 0).ToList());

                    if (telefonosExistentes.Any())
                    {
                        var resultado = await _telefonosDA.EditarTelefono(telefonosExistentes);

                    }
                }
                return true;
            }
            catch (Exception)
            {

                return false;
            }



        }

        public async Task<bool> EditarUsuarioAdmin(string id, UsuariosDto usuario, int? Idgrupo)
        {
            var user = await _userManager.FindByIdAsync(id) ?? throw new BusinessException("El usuario no existe");
            var Rol = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

            if (Idgrupo != null)
            {
                var existe = await _grupos.BuscarGruposPorId((int)Idgrupo) != null;
                EstudianteGrupoReglas.ExisteGrupo(existe);
            }

            if (user.Email != usuario.Email)
            {
                var userEmail = (await _userManager.FindByEmailAsync(usuario.Email));
                if (userEmail != null && userEmail.Id != id)
                {
                    throw new BusinessException("El email ya está en uso por otro usuario");
                }
                await _userManager.SetEmailAsync(user, usuario.Email);
            }
            if (usuario.Rol != Rol)
            {
                if (Rol != null)
                    await _userManager.RemoveFromRoleAsync(user, Rol);
                await _userManager.AddToRoleAsync(user, usuario.Rol);
            }
            var telefonosValidos = usuario.Telefonos
                        .Where(t => !string.IsNullOrWhiteSpace(t.Telefono.ToString()) && !string.IsNullOrWhiteSpace(t.Tipo))
                        .ToList();
            var telefonosExistentes = telefonosValidos.Where(t => t.Id > 0).ToList();
            if (telefonosExistentes.Any())
            {
                await _telefonosDA.EditarTelefono(ConvertirTelefonosAD(telefonosExistentes));
            }
            var telefonosNuevos = telefonosValidos.Where(t => t.Id == 0).ToList();
            if (telefonosNuevos.Any())
            {
                telefonosNuevos.ForEach(t => t.IdUsuario = id);
                await _telefonosDA.AgregarTelefono(ConvertirTelefonosAD(telefonosNuevos));
            }
            if (Idgrupo != null)
            {
                var estudianteGrupo = await _estudianteGrupo.BuscarEstudianteGrupoPorEstudianteId(id);
                var estudiante = new EstudianteGrupoAD { EstudianteId = id, GrupoId = Idgrupo.Value };

                if (estudianteGrupo == null)
                {
                    await _estudianteGrupo.AgregarEstudianteGrupo(estudiante);
                }
                else
                {
                    await _estudianteGrupo.ActualizarEstudianteGrupo(estudiante);
                }
            }
            var usuarioAD = ConvertirUsuarioAD(usuario);
            var resultado = await _usuariosDA.EditarUsuarioAdmin(id, usuarioAD);
            if (resultado > 0)
                return true;
            return false;
        }

        public async Task<List<UsuariosDto>> ListarUsuarios()
        {
            var usuarios = await _usuariosDA.ListarUsuarios();
            return usuarios ?? new List <UsuariosDto>();
        }
        public async Task<UsuariosDto> ObtenerUsuarioPorId(string idUsuario)
        {
            var usuario =await _usuariosDA.ObtenerUsuarioPorId(idUsuario);
            return usuario;
        }
        private static List<TelefonoAD> ConvertirTelefonosAD(List<TelefonoDto> telefonos)
        {
            var telefonosAD = new List<TelefonoAD>();
            foreach (var telefono in telefonos)
            {
                var telefonoAD = new TelefonoAD
                {
                    Id = telefono.Id,
                    IdUsuario = telefono.IdUsuario,
                    Codigo = telefono.Codigo,
                    Telefono = telefono.Telefono,
                    Tipo = telefono.Tipo,
                    Estado = telefono.Estado
                };
                telefonosAD.Add(telefonoAD);
            }
            return telefonosAD;
        }
        private static List<TelefonoDto> ConvertirTelefonosDto(List<TelefonoAD> telefonosAD)
        {
            var telefonosDto = new List<TelefonoDto>();
            foreach (var telefonoAD in telefonosAD)
            {
                var telefonoDto = new TelefonoDto
                {
                    Id = telefonoAD.Id,
                    IdUsuario = telefonoAD.IdUsuario,
                    Codigo = telefonoAD.Codigo,
                    Telefono = telefonoAD.Telefono,
                    Tipo = telefonoAD.Tipo,
                    Estado = telefonoAD.Estado
                };
                telefonosDto.Add(telefonoDto);
            }
            return telefonosDto;
        }
        private async Task<UsuariosAD> CrearUsuarioAD(Abstracciones.Modelos.Requests.RegisterRequest register, ApplicationUser user)
        {
            string rol = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
            var usuario = new UsuariosAD
            {
                IdUsuario = user.Id,
                Nombre = register.Nombre,
                Apellido = register.Apellido,
                Email = register.Email,
                FechaDeNacimiento = register.FechaDeNacimiento,
                Identificacion = register.Identificacion,
                TipoIdentificacion = register.TipoIdentificacion,
                FechaDeRegistro = DateTime.Now,
                Rol = rol,
                Estado = true,

            };
            return usuario;
        }

        private static UsuariosAD ConvertirUsuarioAD(UsuariosDto usuario)
        {
            return new UsuariosAD
            {
                IdUsuario = usuario.IdUsuario,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Email = usuario.Email,
                FechaDeNacimiento = usuario.FechaDeNacimiento,
                Identificacion = usuario.Identificacion,
                TipoIdentificacion = usuario.TipoIdentificacion,
                FechaDeRegistro = usuario.FechaDeRegistro,
                FechaDeModificacion = DateTime.Now,
                Rol = usuario.Rol,
                Estado = usuario.Estado

            };
        }

    }
}
