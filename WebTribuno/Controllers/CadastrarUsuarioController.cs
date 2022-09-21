using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Usuario;
using WebTribuno.Models;

namespace WebTribuno.Controllers
{
    public class CadastrarUsuarioController : Controller
    {
        private readonly IUsuario usuario;

        public CadastrarUsuarioController(IUsuario usuario) 
        {
              this.usuario = usuario;   
        }

        public ActionResult Index()
        {        
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UsuarioViewModel usuarioModel)
        {
            try
            {
                var response = await usuario.SaveAsync(new UsuarioDML() 
                {
                    Nome = usuarioModel.Nome,
                    LoginUsuario = usuarioModel.LoginUsuario,
                    Senha = usuarioModel.Senha,
                    Email = usuarioModel.Email, 
                } );

                if(response.StatusCode == System.Net.HttpStatusCode.OK)
                    return RedirectToAction("Logon", "Login", usuarioModel);

                return View("Index");
            }
            catch(Exception ex)
            {
                return View("Index");
            }
        }
    }
}
