using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Service.UsuarioToken;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Service.WebApiConfig
{
    public class WebApiParametrizacao
    {
        private IConfigurationRoot ConfigRoot;

        protected string UrlWebApi = string.Empty;
        protected int TimeOut = 0;
        protected int IdUsuarioSessao = 0;
        protected HttpClient client = new HttpClient();
        

        public WebApiParametrizacao(IConfiguration configuration, IUsuarioToken usuarioToken)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var usuarioSessao = usuarioToken.RetornarUsuarioSessao(); 
            if (usuarioSessao != null)
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + usuarioSessao.Token);
                IdUsuarioSessao = usuarioSessao.Id;
            }

            client.DefaultRequestHeaders.AcceptEncoding.Clear();

            ConfigRoot = (IConfigurationRoot)configuration;

            UrlWebApi = ConfigRoot["WebApi:url"];
            if (int.TryParse(ConfigRoot["WebApi:TimeOut"], out int timeOut))
                TimeOut = timeOut;
        }

        protected async Task<HttpResponseMessage> DeleteAsync(Uri uri)
        {
            HttpResponseMessage response = await client.DeleteAsync(uri);
            return ValidarRetorno(response);
        }

        protected async Task<HttpResponseMessage> GetAsync(Uri uri)
        {
            HttpResponseMessage response = await client.GetAsync(uri);
            return ValidarRetorno(response);
        }

        protected async Task<HttpResponseMessage> PostAsync(Uri uri, object parametro)
        {
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(parametro), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(uri, httpContent);
            return ValidarRetorno(response);
        }

        private HttpResponseMessage ValidarRetorno(HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();

            if (response.IsSuccessStatusCode)
                return response;
            else
                throw new Exception(response.StatusCode.ToString());
        }
    }

}
