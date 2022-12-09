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

    public class BCEDAO
    {
        private string UrlBase = ConfigurationManager.AppSettings["urlBase"].ToString();

        public async Task<IdentificacionSolicitanteResponse> IdentificacionSolicitante(IdentificacionSolicitanteEnvio obj, string token)
        {
            IdentificacionSolicitanteResponse response = new IdentificacionSolicitanteResponse();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(UrlBase);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer", parameter: token);

                HttpResponseMessage respuesta = await client.PostAsJsonAsync("api/BCE/IdentificacionSolicitante", obj);

                if (respuesta.StatusCode == HttpStatusCode.OK || respuesta.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var res = await respuesta.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<IdentificacionSolicitanteResponse>(res);
                    response.estado = true;
                }
                else
                {
                    response.estado = false;
                    response.mensaje = "Ha existido un error. Por favor, inténtelo más tarde. conectarse al servidor";
                }
            }
            return response;
        }

        public async Task<RegistroSolicitanteResponse> RegistroSolicitante(RegistroSolicitanteEnvio obj, string token)
        {
            RegistroSolicitanteResponse response = new RegistroSolicitanteResponse();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(UrlBase);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer", parameter: token);

                HttpResponseMessage respuesta = await client.PostAsJsonAsync("api/BCE/RegistroSolicitante", obj);

                if (respuesta.StatusCode == HttpStatusCode.OK || respuesta.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var res = await respuesta.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<RegistroSolicitanteResponse>(res);
                }
                else
                {
                    response.estado = false;
                    response.mensaje = "Ha existido un error. Por favor, inténtelo más tarde. conectarse al servidor";
                }
            }
            return response;
        }

        public async Task<RegistroSolicitudResponse> RegistroSolicitud(RegistroSolicitudEnvio obj, string token)
        {
            RegistroSolicitudResponse response = new RegistroSolicitudResponse();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(UrlBase);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer", parameter: token);

                HttpResponseMessage respuesta = await client.PostAsJsonAsync("api/BCE/RegistroSolicitud", obj);

                if (respuesta.StatusCode == HttpStatusCode.OK || respuesta.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var res = await respuesta.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<RegistroSolicitudResponse>(res);
                }
                else
                {
                    response.estado = false;
                    response.mensaje = "Ha existido un error. Por favor, inténtelo más tarde. conectarse al servidor";
                }
            }
            return response;
        }

        public async Task<RegistroSolicitudRenovacionResponse> RegistroSolicitudRenovacion(RegistroSolicitudEnvio obj, string token)
        {
            RegistroSolicitudRenovacionResponse response = new RegistroSolicitudRenovacionResponse();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(UrlBase);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer", parameter: token);

                HttpResponseMessage respuesta = await client.PostAsJsonAsync("api/BCE/RegistroSolicitudRenovacion", obj);

                if (respuesta.StatusCode == HttpStatusCode.OK || respuesta.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var res = await respuesta.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<RegistroSolicitudRenovacionResponse>(res);
                }
                else
                {
                    response.estado = false;
                    response.mensaje = "Ha existido un error. Por favor, inténtelo más tarde. conectarse al servidor";
                }
            }
            return response;
        }

        public async Task<RegistroPagoResponse> RegistroPago(RegistroPagoEnvio obj, string token)
        {
            RegistroPagoResponse response = new RegistroPagoResponse();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(UrlBase);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer", parameter: token);

                HttpResponseMessage respuesta = await client.PostAsJsonAsync("api/BCE/RegistroPago", obj);

                if (respuesta.StatusCode == HttpStatusCode.OK || respuesta.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var res = await respuesta.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<RegistroPagoResponse>(res);
                }
                else
                {
                    response.estado = false;
                    response.mensaje = "Ha existido un error. Por favor, inténtelo más tarde. conectarse al servidor";
                }
            }
            return response;
        }

        public async Task<RegistroSolicitudRevocacionResponse> RegistroSolicitudRevocacion(RegistroSolicitudRevocacionEnvio obj, string token)
        {
            RegistroSolicitudRevocacionResponse response = new RegistroSolicitudRevocacionResponse();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(UrlBase);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer", parameter: token);

                HttpResponseMessage respuesta = await client.PostAsJsonAsync("api/BCE/RegistroSolicitudRevocacion", obj);

                if (respuesta.StatusCode == HttpStatusCode.OK || respuesta.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var res = await respuesta.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<RegistroSolicitudRevocacionResponse>(res);
                }
                else
                {
                    response.estado = false;
                    response.mensaje = "Ha existido un error. Por favor, inténtelo más tarde. conectarse al servidor";
                }
            }
            return response;
        }

        public async Task<RegistroOlvidoClaveResponse> RegistroSolicitudOlvidoClave(RegistroOlvidoClaveEnvio obj, string token)
        {
            RegistroOlvidoClaveResponse response = new RegistroOlvidoClaveResponse();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(UrlBase);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer", parameter: token);

                HttpResponseMessage respuesta = await client.PostAsJsonAsync("api/BCE/RegistroSolicitudOlvidoClave", obj);

                if (respuesta.StatusCode == HttpStatusCode.OK || respuesta.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var res = await respuesta.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<RegistroOlvidoClaveResponse>(res);
                }
                else
                {
                    response.estado = false;
                    response.mensaje = "Ha existido un error. Por favor, inténtelo más tarde. conectarse al servidor";
                }
            }
            return response;
        }

        public async Task<ApiResponse> VerificarEmisiones(string token, string identificacion)
        {
            ApiResponse response = new ApiResponse();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(UrlBase);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer",
                    parameter: token);
                HttpResponseMessage respuesta = client.GetAsync("api/BCE/VerificarEmisiones?numeroIdentificacion=" + identificacion).Result;

                if (respuesta.StatusCode == HttpStatusCode.OK || respuesta.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var res = await respuesta.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<ApiResponse>(res);
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