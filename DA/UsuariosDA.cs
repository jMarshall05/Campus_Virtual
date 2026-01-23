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

            _elContexto.Usuarios.Add(usuario);
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
            UsuariosAD usuarioExistente = _elContexto.Usuarios.FirstOrDefault(u => u.IdUsuario == id);
            if (usuarioExistente != null)
            {
                usuarioExistente.Nombre = usuario.Nombre;
                usuarioExistente.Apellido = usuario.Apellido;
                usuarioExistente.FechaDeModificacion = DateTime.Now;

                EntityState estado = _elContexto.Entry(usuarioExistente).State = EntityState.Modified;
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

                EntityState estado = _elContexto.Entry(usuarioExistente).State = EntityState.Modified;
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

        public Task<List<UsuariosAD>> ListarUsuarios()
        {
            throw new NotImplementedException();
        }

        public Task<UsuariosAD> ObtenerUsuarioPorId(string idUsuario)
        {
            throw new NotImplementedException();
        }


    }

}
