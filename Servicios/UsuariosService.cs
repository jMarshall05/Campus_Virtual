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

        public UsuariosService(UserManager<ApplicationUser> userManager, IUsuariosDA usuariosDA, ITelefonosDA telefonosDA)
        {
            _userManager = userManager;
            _usuariosDA = usuariosDA;
            _telefonosDA = telefonosDA;
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

        public Task<int> EditarUsuario(string id, UsuariosDto usuario)
        {
            throw new NotImplementedException();
        }

        public Task<int> EditarUsuarioAdmin(string id, UsuariosDto usuario)
        {
            throw new NotImplementedException();
        }

        public Task<List<UsuariosDto>> ListarUsuarios()
        {
            throw new NotImplementedException();
        }

        public Task<UsuariosDto> ObtenerUsuarioPorId(string idUsuario)
        {
            throw new NotImplementedException();
        }
        private static List<TelefonoAD> ConvertirTelefonosAD(List<TelefonoDto> telefonos)
        {
            var telefonosAD = new List<TelefonoAD>();
            foreach (var telefono in telefonos)
            {
                var telefonoAD = new TelefonoAD
                {
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

    }
}
