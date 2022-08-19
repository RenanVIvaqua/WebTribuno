using System;
using System.Net.Http.Headers;

namespace Service.Usuario
{
    public class Usuario: IUsuario
    {
        private HttpClient client;
        private const string UrlWebApi = "https://localhost:5001/";

        private const string ActionGet    = "api/Usuario/Get";
        private const string ActionSave   = "api/Usuario/Save";
        private const string ActionUpdate = "api/Usuario/Update";
        private const string ActionLogon  = "api/CreateToken";

        public Usuario()
        {          
            client = new HttpClient()
            {
                BaseAddress = new Uri(UrlWebApi),
                Timeout = TimeSpan.FromSeconds(10000)
            };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
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

        public async Task<Uri> SaveAsync(UsuarioDML usuarioDML)
        {
            try
            {
                HttpResponseMessage response = await client.PostAsJsonAsync(ActionSave, usuarioDML);
                response.EnsureSuccessStatusCode();
               
                return response.Headers.Location;
            }
            catch (Exception exception) 
            {
                throw (exception);
            }
        }

        public async Task<HttpResponseMessage> LogonAsync(string login, string password)
        {
            try
            {
                HttpResponseMessage response = await client.PostAsJsonAsync(ActionLogon, new { Username = login, Password = password });
                response.EnsureSuccessStatusCode();                

                return response;
            }
            catch (Exception) 
            {
                throw;
            }            
        }

        public async Task<Uri> Update(UsuarioDML usuario)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(ActionUpdate, usuario);
            response.EnsureSuccessStatusCode();

            return response.Headers.Location;
        }
    }  

}