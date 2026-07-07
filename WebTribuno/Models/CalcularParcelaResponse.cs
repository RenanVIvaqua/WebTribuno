namespace WebTribuno.Models
{
    public class CalcularParcelaResponse
    {
        public decimal ValorTotal { get; set; }

        public List<ParcelaModel> Parcelas { get; set; } = new();
    }
}