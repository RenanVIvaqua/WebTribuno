using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Usuario
{
    public interface IUsuario
    {
        Task<UsuarioDML> Get(string caminho, int id);

        Task<Uri> SaveAsync(UsuarioDML usuarioDML);

        Task<HttpResponseMessage> LogonAsync(string login, string password);

        Task<Uri> Update(UsuarioDML product);
    }
}
