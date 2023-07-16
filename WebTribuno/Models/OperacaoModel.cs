using Microsoft.AspNetCore.Mvc;
using Service.Operacao;
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
        [Required]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        public TipoOperacao TipoOperacao { get; set; }

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
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DataPrimeiroVencimento { get; set; }
     
        [Display(Name = "Tipo de operação:")]
        public TipoOperacao TipoOperacao { get; set; }

        [Display(Name = "Tipo de calculo:")]
        public TipodeCalculo TipoCalculo { get; set; }

        public List<ParcelaModel> Parcelas { get; set; }

    }


    public class ParcelaModel
    {        
        public int NumeroParcela { get; set; }       
        public decimal ValorParcela { get; set; }
        public DateTime DataVencimento { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime DataAlteracao { get; set; }
        public StatusParcela StatusParcela { get; set; }
    }
   
}
