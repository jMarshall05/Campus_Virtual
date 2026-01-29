using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.Modelos.ModelosDto;
using DA.Entidades;

namespace DA.Interfaces
{
    public interface IUsuariosDA
    {
        Task<string> AgregarUsuario(UsuariosAD usuario);
        Task EditarUsuarioAdmin(string id, UsuariosAD usuario);
        Task EditarUsuario(string id, UsuariosAD usuario);
        Task<IEnumerable<UsuariosDto>> ListarUsuarios();
        Task<UsuariosDto> ObtenerUsuarioPorId(string idUsuario);
        Task<bool> ExisteIdentificacion(string identificacion);

    }
}
