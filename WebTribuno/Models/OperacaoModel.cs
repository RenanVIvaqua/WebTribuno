using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebTribuno.Models
{
    public class OperacaoModel
    {
        [Key]
        public int IdOperacao { get; set; }

        [Required]
        [MaxLength(30)]

        [Display(Name = "Nome operação")]
        public string NomeOperacao { get; set; }

        [MaxLength(100)]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }      

        public SimulacaoParcela SimulacaoParcela { get; set; }

    }

    public class SimulacaoParcela 
    {
        [Required]
        [Display(Name = "Quantidade de parcela")]
        public int QuantidadeParcela { get; set; }

        [Required]
        [Display(Name = "Valor da parcela")]
        public decimal ValorParcela { get; set; }

        [Required]
        [Display(Name = "Data do primeiro vencimento")]
        public DateTime DataPrimeiroVencimento { get; set; }

        [Required]
        [Display(Name = "Tipo de operação:")]
        public TipoOperacao TipoOperacao { get; set; }

        [Display(Name = "Tipo de calculo:")]
        public TipodeCalculo TipoCalculo { get; set; }

        public List<ParcelaModel>Parcelas { get; set; }

    }


    public class ParcelaModel
    {
        [Required]
        public int NumeroParcela { get; set; }
        [Required]
        public decimal ValorParcela { get; set; }
        public DateTime DataVencimento { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime DataAlteracao { get; set; }
        public StatusParcela StatusParcela { get; set; }

    }

    public enum TipoOperacao
    {       
        [Description("Rendimento")]
        Rendimento = 2,

        [Description("Passivo")]
        Passivo = 3
    }

    public enum TipodeCalculo
    {

        [Description("Calculado por Parcela")]
        Parcela = 2,

        [Description("Calculado por Valor Total da Operacao")]
        Operacao = 3
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
