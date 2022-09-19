using Microsoft.Extensions.Configuration;
using Service.UsuarioToken;
using Service.WebApiConfig;
using System;
using System.Net.Http.Headers;

namespace Service.Usuario
{
    public class Usuario: WebApiParametrizacao,IUsuario
    {
        private const string ActionGet    = "api/Usuario/Get";
        private const string ActionSave   = "api/Usuario/Save";
        private const string ActionUpdate = "api/Usuario/Update";
        private const string ActionLogon  = "api/CreateToken";

        public Usuario(IUsuarioToken usuarioToken, IConfiguration configuration) : base(configuration, usuarioToken)
        {

        }                   

        public  async Task<UsuarioDML> Get(string caminho, int id)
        {
            var usuario = new UsuarioDML();

            HttpResponseMessage response = await client.GetAsync(caminho);
            if (response.IsSuccessStatusCode) 
            {
                usuario = await response.Content.ReadAsAsync<UsuarioDML>(); 
            }           
            
            return usuario;            
        }

        public async Task<HttpResponseMessage> SaveAsync(UsuarioDML usuarioDML)
        {
            var uriRelative = new Uri(new Uri(UrlWebApi), relativeUri: ActionSave);
            return await PostAsync(uriRelative, usuarioDML);
        }

        public async Task<HttpResponseMessage> LogonAsync(string login, string password)
        {
            var uriRelative = new Uri(new Uri(UrlWebApi), relativeUri: ActionLogon);
            return await PostAsync(uriRelative, new { Username = login, Password = password });                      
        }

        public async Task<HttpResponseMessage> Update(UsuarioDML usuario)
        {
           var uriRelative = new Uri(new Uri(UrlWebApi), relativeUri: ActionUpdate);
           return await PostAsync(uriRelative, usuario);
            
        }
    }  

}