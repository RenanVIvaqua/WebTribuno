using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Service.Usuario;
using Service.UsuarioToken;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Service.WebApiConfig;

namespace Service.Operacao
{
    public class Operacao : WebApiParametrizacao, IOperacao
    {
        private const string OperacaoActionGet = "api/Operacao/Get";
        private const string OperacaoActionGetAll = "api/Operacao/GetAll";
        private const string OperacaoActionSave = "api/Operacao/Save";
        private const string OperacaoActionUpdate = "api/Operacao/Update";
        private const string OperacaoActionDelete = "api/Operacao/Delete";

        public Operacao(IUsuarioToken usuarioToken, IConfiguration configuration) : base(configuration, usuarioToken)
        {

        }

        public async Task<HttpResponseMessage> Delete(int idOperacao)
        {
            var uriRelative = new Uri(new Uri(UrlWebApi), relativeUri: OperacaoActionDelete + "/?id=" + idOperacao);
            return await DeleteAsync(uriRelative);
        }

        public async Task<OperacaoDML> Get(int id)
        {
            var uriRelative = new Uri(new Uri(UrlWebApi), relativeUri: OperacaoActionGet + "/?id=" + id);
            var response = await GetAsync(uriRelative);

            return await response.Content.ReadAsAsync<OperacaoDML>();
        }

        public async Task<List<OperacaoDML>> GetAll()
        {
            var uriRelative = new Uri(new Uri(UrlWebApi), relativeUri: OperacaoActionGetAll + "/?idUsuario=" + IdUsuarioSessao);
            HttpResponseMessage response = await GetAsync(uriRelative);

            return await response.Content.ReadAsAsync<List<OperacaoDML>>();
        }

        public async Task<HttpResponseMessage> SaveAsync(OperacaoDML operacao)
        {           
            var uriRelative = new Uri(new Uri(UrlWebApi), relativeUri: OperacaoActionSave);
            return await PostAsync(uriRelative, operacao);
        }

        public Task<HttpResponseMessage> Update(OperacaoDML operacao)
        {
            throw new NotImplementedException();
        }
    }
}
