using Service.Usuario;

namespace WebTribuno.Service
{
    public class Util
    {
        public const string sessionUserName = "username";
        public const string sessionToken = "token";

        public UsuarioToken RetornaUsuarioLogado(HttpContext httpContext)
        {
            var username = httpContext.Session.GetString(sessionUserName);
            var token = httpContext.Session.GetString(sessionToken);

            return new UsuarioToken()
            {
                Username = string.IsNullOrEmpty(username) ? string.Empty : username,
                Token = string.IsNullOrEmpty(token) ? string.Empty : token,
            };
        }
    }
}

