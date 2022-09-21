using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Service.Operacao;
using Service.UsuarioToken;
using System.Diagnostics;
using System.Security.Claims;
using WebTribuno.Models;
using WebTribuno.Service;

namespace WebTribuno.Controllers
{
    public class HomeController : Controller
    {
        private readonly IOperacao operacao; 
    
        public HomeController(IOperacao operacao)
        {
            this.operacao = operacao;         
        }

        public async Task<IActionResult> Index()
        {         
            var operacoes = await operacao.GetAll();

            var listaOperacaoModel = new List<OperacaoModel>();
            foreach (var operacao in operacoes)
            {
                var model = new OperacaoModel()
                {
                    NomeOperacao = operacao.NomeOperacao,
                    Descricao = operacao.Descricao,
                    IdOperacao = operacao.IdOperacao,
                };
                listaOperacaoModel.Add(model);
            }

            return View(listaOperacaoModel);
        }

        [HttpPost]
        public async Task<IActionResult> DeletarOperacao(int idOperacao)
        {
            try
            {
                var retorno = await operacao.Delete(idOperacao);

                return Json(new { Sucess = true, Message = "Operação excluido !" });
            }
            catch (Exception ex)
            {
                return Json(new { Sucess = false, Message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AtualizarGridOperacao() 
        {
            var operacoes = await operacao.GetAll();

            var listaOperacaoModel = new List<OperacaoModel>();
            foreach (var operacao in operacoes)
            {
                var model = new OperacaoModel()
                {
                    NomeOperacao = operacao.NomeOperacao,
                    Descricao = operacao.Descricao,
                    IdOperacao = operacao.IdOperacao,
                };
                listaOperacaoModel.Add(model);
            }

            return View("Index",listaOperacaoModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}