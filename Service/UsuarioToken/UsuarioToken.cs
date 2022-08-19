using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.UsuarioToken
{
    public class UsuarioToken : IUsuarioToken
    {
        private UsuarioTokenDML usuarioTokenDML { get; set; }

        public UsuarioTokenDML RetornarUsuarioSessao() 
        {
            return usuarioTokenDML;
        }

        public void SalvarUsuarioToken(UsuarioTokenDML usuarioToken) 
        {
            usuarioTokenDML = usuarioToken;
        }
    }
}
