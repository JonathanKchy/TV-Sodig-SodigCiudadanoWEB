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
    public class CiudadDAO
    {
        private string UrlBase = ConfigurationManager.AppSettings["urlBase"].ToString();

        public async Task<CiudadResponse> ObtenerListaCiudades(string token)
        {
            CiudadResponse response = new CiudadResponse();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(UrlBase);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer",
                    parameter: token);
                HttpResponseMessage respuesta = client.GetAsync("api/Ciudad/ObtenerListaCiudades").Result;

                if (respuesta.StatusCode == HttpStatusCode.OK || respuesta.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var res = await respuesta.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<CiudadResponse>(res);
                }
                else
                {
                    response.estado = false;
                    response.mensajes = "Ha existido un error. Por favor, inténtelo más tarde. conectarse al servidor";
                }
            }
            return response;
        }

        public async Task<CiudadResponse> ObtenerCiudadPorNombreCiudad(string nombreCiudad, string token)
        {
            CiudadResponse response = new CiudadResponse();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(UrlBase);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer",
                    parameter: token);
                HttpResponseMessage respuesta = client.GetAsync("api/Ciudad/ObtenerCiudadPorNombre?nombreCiudad=" + nombreCiudad).Result;

                if (respuesta.StatusCode == HttpStatusCode.OK || respuesta.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var res = await respuesta.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<CiudadResponse>(res);
                }
                else
                {
                    response.estado = false;
                    response.mensajes = "Ha existido un error. Por favor, inténtelo más tarde. conectarse al servidor";
                }
            }
            return response;
        }

        public async Task<CiudadResponse> ObtenerCiudadPorIdCiudad(string idCiudad, string token)
        {
            CiudadResponse response = new CiudadResponse();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(UrlBase);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer",
                    parameter: token);
                HttpResponseMessage respuesta = client.GetAsync("api/Ciudad/ObtenerCiudadPorIdCiudad?idCiudad=" + idCiudad).Result;

                if (respuesta.StatusCode == HttpStatusCode.OK || respuesta.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var res = await respuesta.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<CiudadResponse>(res);
                }
                else
                {
                    response.estado = false;
                    response.mensajes = "Ha existido un error. Por favor, inténtelo más tarde. conectarse al servidor";
                }
            }
            return response;
        }

        public async Task<CiudadResponse> ObtenerCiudadPorIdProvincia(string idProvincia, string token)
        {
            CiudadResponse response = new CiudadResponse();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(UrlBase);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer",
                    parameter: token);
                HttpResponseMessage respuesta = client.GetAsync("api/Ciudad/ObtenerCiudadPorIdProvincia?idProvincia=" + idProvincia).Result;

                if (respuesta.StatusCode == HttpStatusCode.OK || respuesta.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var res = await respuesta.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<CiudadResponse>(res);
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