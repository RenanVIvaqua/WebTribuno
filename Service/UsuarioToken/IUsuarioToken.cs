using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.UsuarioToken
{
    public interface IUsuarioToken
    {        
        UsuarioTokenDML RetornarUsuarioSessao();
        void SalvarUsuarioToken(UsuarioTokenDML usuarioToken);

    }
}

    
