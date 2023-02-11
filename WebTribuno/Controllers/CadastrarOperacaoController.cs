using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Service.Operacao;
using Service.UsuarioToken;
using System.Security.Claims;
using WebTribuno.Models;
using WebTribuno.Pages;
using WebTribuno.Service;

namespace WebTribuno.Controllers
{
   
    public class CadastrarOperacaoController : Controller
    {
        private readonly IOperacao operacao;
        private readonly IUsuarioToken usuarioToken;

        public CadastrarOperacaoController(IOperacao operacao, IUsuarioToken usuarioToken)
        {
            this.operacao = operacao;
            this.usuarioToken= usuarioToken;
        }               

        public ActionResult Index()
        {
            return View();
        }
              
        public ActionResult Alteracao(int pIdOperacao)
        {
            try
            {
                var retorno = operacao.Get(pIdOperacao);
                var model = ConverterDMLparaModel(retorno.Result);

                return View("Index",model);
            }
            catch (Exception ex) 
            {
                return View("~/Views/Shared/Error.cshtml", new ErrorViewModel() { MensagemErro = ex.Message });
            }
        }

        private OperacaoModel ConverterDMLparaModel(OperacaoDML pOperacaoDML) 
        {           
            var operacaoModel = new OperacaoModel()
            {
                NomeOperacao = pOperacaoDML.NomeOperacao,
                Descricao = pOperacaoDML.Descricao,
                IdOperacao = pOperacaoDML.IdOperacao,               
            };

            operacaoModel.SimulacaoParcela = new SimulacaoParcela();           

            operacaoModel.SimulacaoParcela.Parcelas = ConverterParcelaParaModel(pOperacaoDML.Parcelas);
            operacaoModel.SimulacaoParcela.QuantidadeParcela = operacaoModel.SimulacaoParcela.Parcelas.Count();
            operacaoModel.SimulacaoParcela.ValorParcela = operacaoModel.SimulacaoParcela.Parcelas[0].ValorParcela;
            operacaoModel.SimulacaoParcela.DataPrimeiroVencimento = operacaoModel.SimulacaoParcela.Parcelas[0].DataVencimento;

            return operacaoModel;
        }

        private List<ParcelaModel> ConverterParcelaParaModel(List<OperacaoParcela> pOperacaoParcela) 
        {
            var parcelasModel = new List<ParcelaModel>();
            
            foreach(var parcela in pOperacaoParcela) 
            {
                var parcelaModel = new ParcelaModel()
                {
                    DataAlteracao= parcela.DataAlteracao,
                    DataInclusao=parcela.DataInclusao,
                    NumeroParcela = parcela.NumeroParcela,
                    ValorParcela = parcela.ValorParcela,
                    DataVencimento = parcela.DataVencimento,
                };
                parcelasModel.Add(parcelaModel);

            }
            return parcelasModel;
        }


        [HttpPost]   
        public ActionResult Create(OperacaoModel operacaoModel)
        {
            try
            {
                GerarParcelas(ref operacaoModel);

                var retorno = operacao.SaveAsync( 
                    new OperacaoDML()
                    {
                        NomeOperacao = operacaoModel.NomeOperacao,
                        Descricao = operacaoModel.Descricao,
                        DataCadastro = DateTime.Now,
                        IdUsuario = usuarioToken.RetornarUsuarioSessao().Id,
                        Parcelas = ConverterParcelaModel(operacaoModel.SimulacaoParcela.Parcelas)
                    });

                if (retorno.Result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return View("Index");
                }
                else
                {
                    if (retorno.Result.RequestMessage != null)
                        throw new Exception(retorno.Result.RequestMessage.ToString());
                    else
                        throw new Exception("Erro Desconhecido");
                }                    
            }
            catch(Exception ex)
            {               
                return View("~/Views/Shared/Error.cshtml", new ErrorViewModel() { MensagemErro = ex.Message });
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
        public PartialViewResult CalcularParcela(OperacaoModel pOperacaoModel)
        {
            GerarParcelas(ref pOperacaoModel);                        

            return PartialView("Index", pOperacaoModel);
        }

        private void GerarParcelas(ref OperacaoModel pOperacaoModel) 
        {
            pOperacaoModel.SimulacaoParcela.Parcelas = new List<ParcelaModel>();

            for (int i = 1; i <= pOperacaoModel.SimulacaoParcela.QuantidadeParcela; i++)
            {
                var parcela = new ParcelaModel()
                {
                    NumeroParcela = i,
                    ValorParcela = pOperacaoModel.SimulacaoParcela.ValorParcela,
                    DataInclusao = DateTime.Now,
                    DataVencimento = pOperacaoModel.SimulacaoParcela.DataPrimeiroVencimento.AddMonths(i),
                };
                pOperacaoModel.SimulacaoParcela.Parcelas.Add(parcela);
            }
        }
    }
}
