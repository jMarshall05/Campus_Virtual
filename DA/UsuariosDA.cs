using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.DA;
using Abstracciones.Excepciones;
using Abstracciones.Modelos.ModelosDA;
using Abstracciones.Modelos.ModelosDto;
using Microsoft.EntityFrameworkCore;

namespace DA
{
    public class UsuariosDA : IUsuariosDA
    {
        private readonly ApplicationDbContext _elContexto;
        public UsuariosDA(ApplicationDbContext Contexto)
        {
            _elContexto = Contexto;
        }

        public async Task<string> AgregarUsuario(UsuariosAD usuario)
        {

            await _elContexto.Usuarios.AddAsync(usuario);
            int Resultado = await _elContexto.SaveChangesAsync();
            if (Resultado > 0)
            {
                return usuario.IdUsuario;
            }
            else
            {
                return String.Empty;
            }
        }

        public async Task<int> EditarUsuario(string id, UsuariosAD usuario)
        {
            var usuarioExistente =await _elContexto.Usuarios.FirstOrDefaultAsync(u => u.IdUsuario == id);
            if (usuarioExistente != null)
            {
                usuarioExistente.Nombre = usuario.Nombre;
                usuarioExistente.Apellido = usuario.Apellido;
                usuarioExistente.FechaDeModificacion = DateTime.Now;
                int resultado = await _elContexto.SaveChangesAsync();
                return resultado;
            }
            else
            {
                throw new BusinessException("El usuario no existe o no se pudo encontrar en la base de datos.");
            }
        }

        public async Task<int> EditarUsuarioAdmin(string id, UsuariosAD usuario)
        {

            UsuariosAD usuarioExistente = _elContexto.Usuarios.FirstOrDefault(u => u.IdUsuario == id);
            if (usuarioExistente != null)
            {
                usuarioExistente.Nombre = usuario.Nombre;
                usuarioExistente.Apellido = usuario.Apellido;
                usuarioExistente.Email = usuario.Email;
                usuarioExistente.FechaDeNacimiento = usuario.FechaDeNacimiento;
                usuarioExistente.FechaDeModificacion = DateTime.Now;
                usuarioExistente.Rol = usuario.Rol;
                usuarioExistente.Identificacion = usuario.Identificacion;
                usuarioExistente.Estado = usuario.Estado;
                usuarioExistente.TipoIdentificacion = usuario.TipoIdentificacion;
                int resultado = await _elContexto.SaveChangesAsync();
                return resultado;
            }
            else
            {
                throw new Exception("El usuario no existe o no se pudo encontrar en la base de datos.");
            }
        }

        public async Task<bool> ExisteIdentificacion(string identificacion)
        {
            var existe = await _elContexto.Usuarios.AnyAsync(u => u.Identificacion == identificacion);
            return existe;
        }

        public async Task<List<UsuariosDto>> ListarUsuarios()
        {
            var usuariosDto = await _elContexto.Usuarios
                .Select(u => new UsuariosDto
                {
                    IdUsuario = u.IdUsuario,
                    Nombre = u.Nombre,
                    Apellido = u.Apellido,
                    Email = u.Email,
                    Telefonos = u.Telefonos.Select(t => new TelefonoDto
                    {
                        Id = t.Id,
                        IdUsuario = t.IdUsuario,
                        Codigo = t.Codigo,
                        Telefono = t.Telefono,
                        Tipo = t.Tipo,
                        Estado = t.Estado
                    }).ToList(),
                    FechaDeNacimiento = u.FechaDeNacimiento,
                    Identificacion = u.Identificacion,
                    FechaDeRegistro = u.FechaDeRegistro,
                    FechaDeModificacion = u.FechaDeModificacion,
                    Rol = u.Rol,
                    Estado = u.Estado
                })
                .ToListAsync();
            return usuariosDto;
        }

        public async Task<UsuariosDto> ObtenerUsuarioPorId(string idUsuario)
        {
            var usuario = await _elContexto.Usuarios
                .Include(u => u.Telefonos)
                .FirstOrDefaultAsync(u => u.IdUsuario == idUsuario); 
            if (usuario == null)
                return null;
            var usuarioDto = new UsuariosDto
            {
                IdUsuario = usuario.IdUsuario,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Email = usuario.Email,
                Telefonos = usuario.Telefonos.Select(t => new TelefonoDto
                {
                    Id = t.Id,
                    IdUsuario = t.IdUsuario,
                    Codigo = t.Codigo,
                    Telefono = t.Telefono,
                    Tipo = t.Tipo,
                    Estado = t.Estado
                }).ToList(),
                FechaDeNacimiento = usuario.FechaDeNacimiento,
                Identificacion = usuario.Identificacion,
                FechaDeRegistro = usuario.FechaDeRegistro,
                FechaDeModificacion = usuario.FechaDeModificacion,
                Rol = usuario.Rol,
                Estado = usuario.Estado
            };
            return usuarioDto;
        }
    }
}