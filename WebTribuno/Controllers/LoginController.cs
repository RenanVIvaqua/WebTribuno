using Microsoft.AspNetCore.Mvc;
using Service.Usuario;
using WebTribuno.Models;
using Newtonsoft.Json;
using Service.UsuarioToken;

namespace WebTribuno.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUsuario usuario;

        private IUsuarioToken usuarioToken;

        public LoginController(IUsuario usuario, IUsuarioToken usuarioToken)
        {
            this.usuario = usuario;
            this.usuarioToken = usuarioToken;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Logon(UsuarioViewModel usuarioModel)
        {
            try
            {
                var response = await usuario.LogonAsync(usuarioModel.LoginUsuario, usuarioModel.Senha);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var retorno = await response.Content.ReadAsStringAsync();
                    var usuarioSessao = JsonConvert.DeserializeObject<UsuarioTokenDML>(retorno);

                    if (usuarioSessao != null)                    
                        usuarioToken.SalvarUsuarioToken(usuarioSessao);
                   
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View("index");
                }

            }
            catch (Exception ex)
            {
                return View("index");
            }
        }       
    }
}
