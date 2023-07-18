using Highsoft.Web.Mvc.Charts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Service.Operacao;
using Service.UsuarioToken;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.Serialization;
using System.Security.Claims;
using WebTribuno.Models;
using WebTribuno.Service;

namespace WebTribuno.Controllers
{
    public class HomeController : Controller
    {
        private readonly IOperacao operacao;
        private readonly IUsuarioToken usuarioToken;

        public HomeController(IOperacao operacao, IUsuarioToken usuarioToken)
        {
            this.operacao = operacao;
            this.usuarioToken = usuarioToken;
        }

        public async Task<IActionResult> Index()
        {           
            var operacoes = await operacao.GetAll();

            ViewData["nomeUsuario"] = usuarioToken.RetornarUsuarioSessao().Username;

            var homeModel = CarregarModel(operacoes);
            MontarGraficoRendimento(operacoes);                 
          
            return View(homeModel);
        }

        private HomeModel CarregarModel(List<OperacaoDML> pOperacoes) 
        {
            var homeModel = new HomeModel();

            foreach (var operacao in pOperacoes)
            {
                var model = new OperacaoModel()
                {
                    NomeOperacao = operacao.NomeOperacao,
                    Descricao = operacao.Descricao,
                    IdOperacao = operacao.IdOperacao,
                };

                if (operacao.TipoOperacao == TipoOperacao.Passivo)
                    homeModel.ListaPassivo.Add(operacao);
                else
                    homeModel.ListaRendimento.Add(operacao);
            }

            return homeModel;
        }

        private void MontarGraficoRendimento(List<OperacaoDML> pOperacoes) 
        {
            var pDicionarioDespesa = new Dictionary<int, decimal>();
            var pDicionarioRendimento = new Dictionary<int, decimal>();

            GerarDadosGraficoRedimento(pOperacoes, out pDicionarioDespesa, out pDicionarioRendimento);
            GerarSessaoDados(pDicionarioDespesa, TipoOperacao.Passivo);
            GerarSessaoDados(pDicionarioRendimento, TipoOperacao.Rendimento);
            GerarSessaoDadosMeses(pDicionarioDespesa.Keys.ToList());
        }

        private void GerarSessaoDadosMeses(List<int>pMeses) 
        {
            var listaMeses = new List<string>();
            
            foreach (int mes in pMeses)
                listaMeses.Add(CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(mes));

            ViewData[$"MesesData"] = listaMeses;
        }

        private DateTime ObterDataRefrencia() 
        {
            return DateTime.Now.AddMonths(6);       
        }

        private void GerarSessaoDados(Dictionary<int, decimal> pValores, TipoOperacao pTipoOperacao) 
        {
            List<LineSeriesData> valoresGrafico = new List<LineSeriesData>();

            foreach (var valor in pValores)             
                valoresGrafico.Add(new LineSeriesData() { Y = (double?)valor.Value });
                       
            ViewData[$"{pTipoOperacao}Data"] = valoresGrafico;       
        }

        private void GerarDadosGraficoRedimento(List<OperacaoDML> pOperacoes, out Dictionary<int, decimal>pDicionarioDespesa, out Dictionary<int, decimal> pDicionarioRendimento) 
        {
            DateTime dataReferencia = ObterDataRefrencia();
            pDicionarioDespesa = new Dictionary<int, decimal>();
            pDicionarioRendimento = new Dictionary<int, decimal>();

            for (int i = 0; i <= 10; i++) 
            {                
                decimal somaDespesa = 0;
                decimal somaRendimento = 0;
                foreach (var operacao in pOperacoes)
                {

                    var teste = (from x in operacao.Parcelas where x.DataVencimento.Month == dataReferencia.Month && x.DataVencimento.Year == dataReferencia.Year select x.ValorParcela);

                    if (operacao.TipoOperacao == TipoOperacao.Passivo)                    
                        somaDespesa += (from x in operacao.Parcelas where x.DataVencimento.Month == dataReferencia.Month && x.DataVencimento.Year == dataReferencia.Year select x.ValorParcela).Sum();     

                    else if(operacao.TipoOperacao == TipoOperacao.Rendimento)
                        somaRendimento += (from x in operacao.Parcelas where x.DataVencimento.Month == dataReferencia.Month && x.DataVencimento.Year == dataReferencia.Year select x.ValorParcela).Sum();
                }

                pDicionarioDespesa.Add(dataReferencia.Month, somaDespesa);
                pDicionarioRendimento.Add(dataReferencia.Month, somaRendimento);
                dataReferencia = dataReferencia.AddMonths(1);
            }   
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