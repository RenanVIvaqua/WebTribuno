using Service.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Operacao
{
    public interface  IOperacao
    {
        Task<HttpResponseMessage> SaveAsync(string token, OperacaoDML operacao);

        Task<HttpResponseMessage> Update(OperacaoDML operacao);

        Task<HttpResponseMessage> Delete(string token, int idOperacao);

        Task<OperacaoDML> Get(string token, int idOperacao);

        Task<List<OperacaoDML>> GetAll(string token, int idUsuario);

    }
}
