﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Service.Operacao;
using Service.UsuarioToken;
using System.ComponentModel.DataAnnotations;
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
 
        [HttpPost]   
        public ActionResult Create(OperacaoModel operacaoModel)
        {
            try
            {
                var operacaoDML = ConverterModelemDML(operacaoModel);

                Task<HttpResponseMessage> retorno;

                if (operacaoModel.IdOperacao > 0)                 
                    retorno = operacao.Update(operacaoDML);                
                else                 
                    retorno = operacao.SaveAsync(operacaoDML);
                          
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

        [HttpPost]
        public PartialViewResult CalcularParcela(OperacaoModel pOperacaoModel)
        {
            GerarParcelas(ref pOperacaoModel);
            return PartialView("Index", pOperacaoModel);
        }
              
        private void GerarParcelas(ref OperacaoModel pOperacaoModel) 
        {
            pOperacaoModel.SimulacaoParcela.Parcelas = new List<ParcelaModel>();

            if (pOperacaoModel.SimulacaoParcela.DataPrimeiroVencimento == DateTime.MinValue)
                pOperacaoModel.SimulacaoParcela.DataPrimeiroVencimento = DateTime.Now;

            for (int i = 1; i <= pOperacaoModel.SimulacaoParcela.QuantidadeParcela; i++)
            {
                var parcela = new ParcelaModel()
                {
                    NumeroParcela = i,
                    ValorParcela = pOperacaoModel.SimulacaoParcela.ValorParcela,
                    DataInclusao = DateTime.Now,
                    DataVencimento = pOperacaoModel.SimulacaoParcela.DataPrimeiroVencimento.AddMonths(i - 1),
                };
                pOperacaoModel.SimulacaoParcela.Parcelas.Add(parcela);
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

        private OperacaoModel ConverterDMLparaModel(OperacaoDML pOperacaoDML)
        {
            var operacaoModel = new OperacaoModel()
            {
                NomeOperacao = pOperacaoDML.NomeOperacao,
                Descricao = pOperacaoDML.Descricao,
                IdOperacao = pOperacaoDML.IdOperacao,
                SimulacaoParcela = new SimulacaoParcela 
                {
                    Parcelas = ConverterParcelaParaModel(pOperacaoDML.Parcelas),
                    QuantidadeParcela = pOperacaoDML.Parcelas.Count,
                    ValorParcela = pOperacaoDML.Parcelas[0].ValorParcela,
                    DataPrimeiroVencimento = pOperacaoDML.Parcelas[0].DataVencimento,
                    TipoOperacao = pOperacaoDML.TipoOperacao,
                    TipoCalculo = pOperacaoDML.TipoCalculo
                }
            };          

            return operacaoModel;
        }

        private List<ParcelaModel> ConverterParcelaParaModel(List<OperacaoParcela> pOperacaoParcela)
        {
            var parcelasModel = new List<ParcelaModel>();

            foreach (var parcela in pOperacaoParcela)
            {
                var parcelaModel = new ParcelaModel()
                {
                    DataAlteracao = parcela.DataAlteracao,
                    DataInclusao = parcela.DataInclusao,
                    NumeroParcela = parcela.NumeroParcela,
                    ValorParcela = parcela.ValorParcela,
                    DataVencimento = parcela.DataVencimento,
                };
                parcelasModel.Add(parcelaModel);

            }
            return parcelasModel;
        }

        private OperacaoDML ConverterModelemDML(OperacaoModel operacaoModel)
        {
            var operacaoDML = new OperacaoDML()
            {
                IdOperacao = operacaoModel.IdOperacao,
                NomeOperacao = operacaoModel.NomeOperacao,
                Descricao = operacaoModel.Descricao,
                DataCadastro = DateTime.Now,
                IdUsuario = usuarioToken.RetornarUsuarioSessao().Id,
                TipoCalculo = operacaoModel.SimulacaoParcela.TipoCalculo,
                TipoOperacao= operacaoModel.SimulacaoParcela.TipoOperacao,
                Parcelas = ConverterParcelaModel(operacaoModel.SimulacaoParcela.Parcelas)
            };

            return operacaoDML;
        }        
    }
}
