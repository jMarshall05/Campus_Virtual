using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.Modelos.ModelosDA;
using Abstracciones.Modelos.ModelosDto;

namespace Abstracciones.DA
{
    public interface IUsuariosDA
    {
        Task<string> AgregarUsuario(UsuariosAD usuario);
        Task<int> EditarUsuarioAdmin(string id, UsuariosAD usuario);
        Task<int> EditarUsuario(string id, UsuariosAD usuario);
        Task<List<UsuariosAD>> ListarUsuarios();
        Task<UsuariosAD> ObtenerUsuarioPorId(string idUsuario);
        Task<bool> ExisteIdentificacion(string identificacion);

    }
}
