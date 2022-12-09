using Newtonsoft.Json;
using SodigCiudadano.Models.Entidades;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace SodigCiudadano.Models.Dao
{
    public class TipoSolicitudDAO
    {
        private string UrlBase = ConfigurationManager.AppSettings["urlBase"].ToString();

        public async Task<TipoSolicitudResponse> ObtenerListaTipoSolicitud(string token)
        {
            TipoSolicitudResponse response = new TipoSolicitudResponse();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(UrlBase);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(scheme: "Bearer",
                    parameter: token);
                HttpResponseMessage respuesta = client.GetAsync("api/TipoSolicitud/ObtenerListaTipoSolicitud").Result;

                if (respuesta.StatusCode == HttpStatusCode.OK || respuesta.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var res = await respuesta.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<TipoSolicitudResponse>(res);
                }
                else
                {
                    response.estado = false;
                    response.mensaje = "No ha sido posible conectarse al servidor";
                }
            }
            return response;
        }

        public async Task<NombreSolicitudResponse> ObtenerTipoSolicitudPorId(int idTipoSolicitud, string token)
        {
            NombreSolicitudResponse response = new NombreSolicitudResponse();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(UrlBase);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(scheme: "Bearer",
                    parameter: token);
                string url = "api/TipoSolicitud/ObtenerTipoSolicitudPorId?idTipoSolicitud=" + idTipoSolicitud;
                HttpResponseMessage respuesta = client.GetAsync(url).Result;

                if (respuesta.StatusCode == HttpStatusCode.OK || respuesta.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var res = await respuesta.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<NombreSolicitudResponse>(res);
                }
                else
                {
                    response.estado = false;
                    response.mensaje = "No ha sido posible conectarse al servidor";
                }
            }
            return response;
        }
    }
}