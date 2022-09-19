using Service.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Service.Operacao
{
    public interface IOperacao
    {
        Task<HttpResponseMessage> SaveAsync(OperacaoDML operacao);

        Task<HttpResponseMessage> Update(OperacaoDML operacao);

        Task<HttpResponseMessage> Delete(int idOperacao);

        Task<OperacaoDML> Get(int idOperacao);

        Task<List<OperacaoDML>> GetAll(int idUsuario);
    }
}
