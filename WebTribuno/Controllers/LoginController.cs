using Microsoft.AspNetCore.Mvc;
using Service.Usuario;
using WebTribuno.Models;
using Microsoft.AspNetCore.Http;
using WebTribuno.Service;
using System.Text.Json;
using Newtonsoft.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace WebTribuno.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUsuario usuario;

        public LoginController(IUsuario usuario)
        {
            this.usuario = usuario;
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
                    var usuarioSessao = JsonConvert.DeserializeObject<UsuarioToken>(retorno);
                    
                    if(usuarioSessao != null)
                        SalvarUsuarioSessao(usuarioSessao);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View("index");
                }

            }
            catch (Exception)
            {
                return View("index");
            }
        }

        private async void SalvarUsuarioSessao(UsuarioToken usuarioToken) 
        {
            HttpContext.Session.SetString(Util.sessionToken, usuarioToken.Token);
            HttpContext.Session.SetString(Util.sessionUserName, usuarioToken.Username);
           
            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.NameIdentifier, usuarioToken.Username));
            claims.Add(new Claim(ClaimTypes.Name, usuarioToken.Username));
            claims.Add(new Claim(ClaimTypes.Authentication, usuarioToken.Token));

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync(claimsPrincipal);
        }
    }
}
