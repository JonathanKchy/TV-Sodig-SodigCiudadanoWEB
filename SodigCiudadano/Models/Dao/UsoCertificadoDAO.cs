using Newtonsoft.Json;
using SodigCiudadano.Models.Entidades;
using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SodigCiudadano.Models.Dao
{
    public class UsoCertificadoDAO
    {
        private string UrlBase = ConfigurationManager.AppSettings["urlBase"].ToString();

        public async Task<UsoCertificadoResponse> ObtenerListaUsoCertificado(string token)
        {
            UsoCertificadoResponse response = new UsoCertificadoResponse();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(UrlBase);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer",
                    parameter: token);
                HttpResponseMessage respuesta = client.GetAsync("api/UsoCertificado/ObtenerListaUsoCertificado").Result;

                if (respuesta.StatusCode == HttpStatusCode.OK || respuesta.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var res = await respuesta.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<UsoCertificadoResponse>(res);
                }
                else
                {
                    response.estado = false;
                    response.mensajes = "No ha sido posible conectarse al servidor";
                }
            }
            return response;
        }
    }
}