using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Operacao
{
    public class OperacaoDML
    {
        public OperacaoDML()
        {
            Parcelas = new List<OperacaoParcela>();
        }

        public int IdUsuario { get; set; }
        public int IdOperacao { get; set; }
        public string NomeOperacao { get; set; }
        public string Descricao { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataAlteracao { get; set; }
        public TipoOperacao TipoOperacao { get; set; }
        public TipodeCalculo TipoCalculo { get; set; }
        public List<OperacaoParcela> Parcelas { get; set; }

    }

    public enum TipoOperacao
    {
        [Description("Tipo de Operação Não Definido")]
        NaoDefinido = 1,

        [Description("Tipo de Operação Ativo")]
        Rendimento = 2,

        [Description("Tipo de Operação Passivo")]
        Passivo = 3
    }

    public enum TipodeCalculo
    {
        [Description("Calculado por Parcela")]
        NãoDefinido = 1,

        [Description("Calculado por Parcela")]
        Parcela = 2,

        [Description("Calculado por Valor Total da Operacao")]
        Operacao = 3
    }

    public class OperacaoParcela
    {
        public int IdParcela { get; set; }
        public int IdOperacao { get; set; }
        public int NumeroParcela { get; set; }
        public decimal ValorParcela { get; set; }
        public DateTime DataVencimento { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime DataAlteracao { get; set; }
        public StatusParcela StatusParcela { get; set; }
    }

    public enum StatusParcela
    {
        [Description("Tipo de Parcela Não definido")]
        NaoDefinido = 1,

        [Description("Parcela Em Aberto")]
        EmAberto = 2,

        [Description("Parcela em atraso")]
        Vencido = 3,

        [Description("Parcela quitada")]
        Pago = 4

    }
}
