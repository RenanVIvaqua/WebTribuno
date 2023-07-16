using Microsoft.AspNetCore.Mvc;
using Service.Operacao;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebTribuno.Models
{
    public class HomeModel
    {
        public List<OperacaoDML> ListaRendimento = new List<OperacaoDML>();
        public List<OperacaoDML> ListaPassivo = new List<OperacaoDML>();
    }
}
