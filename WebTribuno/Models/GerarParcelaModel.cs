using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebTribuno.Models
{
    public class GerarParcelaModel
    {
        [Required]       
        [Range(1,99)]
        [DisplayName("Nro Parcela")]
        public int NumeroDeParcelas { get; set; }

        [Required]
        [DisplayName("Valor")]
        public decimal ValorParcela { get; set; }

        [Required]
        [DisplayName("Primeiro vencimento")]
        public DateTime PrimeiroVencimento { get; set; }
              
        public decimal SaldoCalculado { get; set; }
    }
}
