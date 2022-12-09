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
    public class LogDAO
    {
        private string UrlBase = ConfigurationManager.AppSettings["urlBase"].ToString();

        public async Task<LogResponse> GuardarLogTipoSolicitud(LogTipoSolicitud obj, string token)
        {
            LogResponse response = new LogResponse();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(UrlBase);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer",
                    parameter: token);
                HttpResponseMessage respuesta = client.PostAsJsonAsync("api/Log/GuardarLogTipoSolicitud", obj).Result;

                if (respuesta.StatusCode == HttpStatusCode.OK || respuesta.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var res = await respuesta.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<LogResponse>(res);
                }
                else
                {
                    response.estado = false;
                    response.mensaje = "No ha sido posible conectarse al servidor";
                }
            }
            return response;
        }

        public async Task<LogResponse> GuardarLogTipoContenedor(LogTipoContenedor obj, string token)
        {
            LogResponse response = new LogResponse();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(UrlBase);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer",
                    parameter: token);
                HttpResponseMessage respuesta = client.PostAsJsonAsync("api/Log/GuardarLogTipoContenedor", obj).Result;

                if (respuesta.StatusCode == HttpStatusCode.OK || respuesta.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var res = await respuesta.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<LogResponse>(res);
                }
                else
                {
                    response.estado = false;
                    response.mensaje = "No ha sido posible conectarse al servidor";
                }
            }
            return response;
        }

        public async Task<LogResponse> GuardarLogTipoIdentificacion(LogTipoIdentificacion obj, string token)
        {
            LogResponse response = new LogResponse();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(UrlBase);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer",
                    parameter: token);
                HttpResponseMessage respuesta = client.PostAsJsonAsync("api/Log/GuardarLogTipoIdentificacion", obj).Result;

                if (respuesta.StatusCode == HttpStatusCode.OK || respuesta.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var res = await respuesta.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<LogResponse>(res);
                }
                else
                {
                    response.estado = false;
                    response.mensaje = "No ha sido posible conectarse al servidor";
                }
            }
            return response;
        }

        public async Task<LogResponse> GuardarLogTipoElector(LogTipoElector obj, string token)
        {
            LogResponse response = new LogResponse();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(UrlBase);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer",
                    parameter: token);
                HttpResponseMessage respuesta = client.PostAsJsonAsync("api/Log/GuardarLogTipoElector", obj).Result;

                if (respuesta.StatusCode == HttpStatusCode.OK || respuesta.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var res = await respuesta.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<LogResponse>(res);
                }
                else
                {
                    response.estado = false;
                    response.mensaje = "No ha sido posible conectarse al servidor";
                }
            }
            return response;
        }

        public async Task<LogResponse> GuardarLogPagoCheckOut(LogPagoCheckOut obj, string token)
        {
            LogResponse response = new LogResponse();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(UrlBase);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer",
                    parameter: token);
                HttpResponseMessage respuesta = client.PostAsJsonAsync("api/Log/GuardarLogpagoCheckout", obj).Result;

                if (respuesta.StatusCode == HttpStatusCode.OK || respuesta.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var res = await respuesta.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<LogResponse>(res);
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