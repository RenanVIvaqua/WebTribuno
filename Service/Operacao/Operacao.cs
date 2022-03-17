using Newtonsoft.Json;
using Service.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Service.Operacao
{
    public class Operacao : IOperacao
    {
        private HttpClient client;
        private const string UrlWebApi = "https://localhost:44390/"; // Pegar do appsettings 

        private const string ActionGet = "api/Operacao/Get";
        private const string ActionGetAll = "api/Operacao/GetAll";
        private const string ActionSave = "api/Operacao/Save";
        private const string ActionUpdate = "api/Operacao/Update";
        private const string ActionDelete = "api/Operacao/Delete";

        public Operacao()
        {
            client = new HttpClient()
            {
                BaseAddress = new Uri(UrlWebApi),
                Timeout = TimeSpan.FromSeconds(10000) // Pegar do appsettings
            };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<HttpResponseMessage> Delete(string token, int idOperacao)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var uriRelative = new Uri(new Uri(UrlWebApi), relativeUri: ActionDelete + "/?id=" + idOperacao);

            HttpResponseMessage response = await client.DeleteAsync(uriRelative);

            response.EnsureSuccessStatusCode();

            if (response.IsSuccessStatusCode)
                return response;
            else
                throw (new Exception(response.StatusCode.ToString()));
        }

        public async Task<OperacaoDML> Get(string token, int id)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var uriRelative = new Uri(new Uri(UrlWebApi), relativeUri: ActionGet + "/?id=" + id);

            HttpResponseMessage response = await client.GetAsync(uriRelative);

            response.EnsureSuccessStatusCode();

            if (response.IsSuccessStatusCode)
            {
                var operacao = new OperacaoDML();
                return operacao =  await response.Content.ReadAsAsync<OperacaoDML>();
            }
            else
                throw (new Exception(response.StatusCode.ToString()));
        }

        public  async Task<List<OperacaoDML>> GetAll(string token, int idUser)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var uriRelative = new Uri(new Uri(UrlWebApi), relativeUri: ActionGetAll + "/?idUsuario=" + idUser);

            HttpResponseMessage response = await client.GetAsync(uriRelative);

            response.EnsureSuccessStatusCode();

            if (response.IsSuccessStatusCode)
            {
                var operacao = await response.Content.ReadAsAsync<List<OperacaoDML>>();
                return operacao;
            }
            else
                throw (new Exception(response.StatusCode.ToString()));
        }

        

        public async Task<HttpResponseMessage> SaveAsync(string token, OperacaoDML operacao)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(operacao), Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            try
            {
                HttpResponseMessage response = await client.PostAsync(UrlWebApi + ActionSave, httpContent);
                response.EnsureSuccessStatusCode();

                return response;
            }
            catch (Exception ex) 
            {
                throw new Exception("network error", ex);
            }         
        }

        public Task<HttpResponseMessage> Update(OperacaoDML operacao)
        {
            throw new NotImplementedException();
        }
    }
}
