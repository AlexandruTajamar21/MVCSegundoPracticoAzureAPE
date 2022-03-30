using APISEgundoPracticoAzureAPE.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MVCSegundoPracticoAzureAPE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MVCSegundoPracticoAzureAPE.Services
{
    public class ServiceCliente
    {
        private string UrlApi;
        private MediaTypeWithQualityHeaderValue Header;

        public ServiceCliente(string urlapi)
        {
            this.UrlApi = urlapi;
            this.Header = new MediaTypeWithQualityHeaderValue("application/json");
        }

        public async Task<string> GetTokenAsync(string userName, string password)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                LoginModel model = new LoginModel
                {
                    UserName = userName,
                    Password = password
                };
                string json = JsonConvert.SerializeObject(model);
                StringContent content =
                    new StringContent(json, Encoding.UTF8, "application/json");
                string request = "/api/authorization/ValidarUsuario";
                HttpResponseMessage response =
                    await client.PostAsync(request, content);
                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    JObject jObject = JObject.Parse(data);
                    string token = jObject.GetValue("response").ToString();
                    return token;
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<Ticket> GetTicketId(int id, string token)
        {
            string request = "/api/Empresa/FindTicket/" + id;
            Ticket tickets = await this.CallApiAsync<Ticket>(request, token);
            return tickets;
        }

        public async Task CreateTicket(DateTime fecha, string importe, string producto, string fileName, string storagePath, string token)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                client.DefaultRequestHeaders.Add("Authorization", "bearer " + token);

                string request = "/api/empresa/createticket";

                Ticket ticket = new Ticket { Fecha = fecha, Importe = importe, Producto = producto, FileName = fileName, StoragePath = storagePath };

                string json = JsonConvert.SerializeObject(ticket);

                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(request, content);
            }
        }

        public async Task<List<Ticket>> GetTicketsUser(string token)
        {
            string request = "/api/Empresa/TicketsUsuario";
            List<Ticket> tickets = await this.CallApiAsync<List<Ticket>>(request,token);
            return tickets;
        }

        private async Task<T> CallApiAsync<T>(string request)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                HttpResponseMessage response =
                    await client.GetAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    T data = await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    return default(T);
                }
            }
        }

        public async Task<Boolean> CreateUser(string nombre, string apellidos, string email, string userName, string password)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                //client.DefaultRequestHeaders.Add("Authorization", "bearer " + token);

                string request = "/api/authorization/InsertUsuario";

                Usuario user = new Usuario { Nombre = nombre, Apellidos = apellidos, Email = email, UserName = userName, Password = password };

                string json = JsonConvert.SerializeObject(user);

                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(request, content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private async Task<T> CallApiAsync<T>(string request, string token)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                client.DefaultRequestHeaders.Add("Authorization", "bearer " + token);
                HttpResponseMessage response =
                    await client.GetAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    T data = await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    return default(T);
                }

            }
        }

    }
}
