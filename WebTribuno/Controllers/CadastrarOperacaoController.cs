using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Service.Operacao;
using System.Security.Claims;
using WebTribuno.Models;
using WebTribuno.Service;

namespace WebTribuno.Controllers
{
    [Authorize]
    public class CadastrarOperacaoController : Controller
    {
        private const string SessaoParcelas = "SessionParcelas";

        private readonly IOperacao operacao;

        public CadastrarOperacaoController(IOperacao operacao)
        {
            this.operacao = operacao;
        }

        private List<ParcelaModel>? parcelaModels
        {
            get
            {
                var value = HttpContext.Session.GetString(SessaoParcelas);
                return value == null ? null : JsonConvert.DeserializeObject<List<ParcelaModel>>(value);
            }
            set
            {
                HttpContext.Session.SetString(SessaoParcelas, JsonConvert.SerializeObject(value));
            }
        }

        // GET: CadastrarOperacaoController
        public ActionResult Index()
        {          
            return View();
        }

        // POST: CadastrarOperacaoController/Create
        [HttpPost]   
        public ActionResult Create(OperacaoModel operacaoModel)
        {
            try
            {
                operacao.SaveAsync( 
                    new OperacaoDML()
                    {
                        NomeOperacao = operacaoModel.NomeOperacao,
                        Descricao = operacaoModel.Descricao,
                        DataCadastro = DateTime.Now,
                        IdUsuario = 1,
                        Parcelas = ConverterParcelaModel(parcelaModels)
                    });            
                var retorno = new { Success = true, Message = "Operação salva" };
                return Json(retorno);
            }
            catch(Exception ex)
            {
                var retorno = new { Success = false, Message = ex.Message };
                return Json(retorno);
            }
        }

        private List<OperacaoParcela> ConverterParcelaModel(List<ParcelaModel>? listParcelaModel)
        {
            var parcelasDML = new List<OperacaoParcela>();

            if (listParcelaModel != null && listParcelaModel.Count > 0)
            {
                foreach (var parcela in listParcelaModel)
                {
                    parcelasDML.Add(new OperacaoParcela()
                    {
                        NumeroParcela = parcela.NumeroParcela,
                        ValorParcela = parcela.ValorParcela,
                        DataVencimento = parcela.DataVencimento,
                        DataInclusao = parcela.DataInclusao,
                    });
                }
            }

            return parcelasDML;
        }

        [HttpPost]
        public ActionResult CalcularParcela(decimal valorParcela, int quantidadeParcela, DateTime dataVencimento)
        {
            var parcelas = new List<ParcelaModel>();

            for (int i = 1; i <= quantidadeParcela; i++)
            {
                var parcela = new ParcelaModel()
                {
                    NumeroParcela = i,
                    ValorParcela = valorParcela,
                    DataInclusao = DateTime.Now,
                    DataVencimento = dataVencimento.AddMonths(i),
                };
                parcelas.Add(parcela);
            }
            parcelaModels = parcelas;

            return Json(parcelas);
        }


        // POST: CadastrarOperacaoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
