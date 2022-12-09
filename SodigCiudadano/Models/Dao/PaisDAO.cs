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
    public class PaisDAO
    {
        private string UrlBase = ConfigurationManager.AppSettings["urlBase"].ToString();

        public async Task<PaisResponse> ObtenerListaPaises(string token)
        {
            PaisResponse response = new PaisResponse();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(UrlBase);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer",
                    parameter: token);
                HttpResponseMessage respuesta = client.GetAsync("api/Pais/ObtenerListaPaises").Result;

                if (respuesta.StatusCode == HttpStatusCode.OK || respuesta.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var res = await respuesta.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<PaisResponse>(res);
                }
                else
                {
                    response.estado = false;
                    response.mensajes = "Ha existido un error. Por favor, inténtelo más tarde. conectarse al servidor";
                }
            }
            return response;
        }

        public async Task<PaisResponse> ObtenerPaisPorId(string idPais, string token)
        {
            PaisResponse response = new PaisResponse();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(UrlBase);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer",
                    parameter: token);
                HttpResponseMessage respuesta = client.GetAsync("api/Pais/ObtenerPaisPorIdPais?idPais=" + idPais).Result;

                if (respuesta.StatusCode == HttpStatusCode.OK || respuesta.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var res = await respuesta.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<PaisResponse>(res);
                }
                else
                {
                    response.estado = false;
                    response.mensajes = "Ha existido un error. Por favor, inténtelo más tarde. conectarse al servidor";
                }
            }
            return response;
        }

    }
}