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

        public Task<int> EditarUsuario(string id, UsuariosAD usuario)
        {
            throw new NotImplementedException();
        }

        public Task<int> EditarUsuarioAdmin(string id, UsuariosAD usuario)
        {
            throw new NotImplementedException();
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
