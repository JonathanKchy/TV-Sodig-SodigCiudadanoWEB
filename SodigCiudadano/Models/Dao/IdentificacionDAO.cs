using Newtonsoft.Json;
using SodigCiudadano.Models.Entidades;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SodigCiudadano.Models.Dao
{
    public class IdentificacionDAO
    {
        private string UrlBase = ConfigurationManager.AppSettings["urlBase"].ToString();

        public async Task<IdentificacionResponse> guardarLogCedula(LogCedulaFrontal obj, string token, byte[] imagenCedula)
        {
            IdentificacionResponse response = new IdentificacionResponse();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(UrlBase);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer",
                    parameter: token);

                var content = new MultipartFormDataContent();
                var imagen = new ByteArrayContent(imagenCedula);
                var stringPayload = await Task.Run(() => JsonConvert.SerializeObject(obj));
                var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
                imagen.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                content.Add(imagen, "imagen");
                content.Add(httpContent, "obj");
                HttpResponseMessage respuesta = await client.PostAsync("api/Cedula/guardarLogCedula", content);
                if (respuesta.StatusCode == HttpStatusCode.OK || respuesta.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var res = await respuesta.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<IdentificacionResponse>(res);
                }
                else
                {
                    response.estado = false;
                    response.mensajes = "No ha sido posible conectarse al servidor";
                }
            }
            return response;
        }

        public async Task<IdentificacionResponse> guardarLogPasaporte(LogPasaporte obj, string token, byte[] imagenCedula)
        {
            IdentificacionResponse response = new IdentificacionResponse();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(UrlBase);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer",
                    parameter: token);

                var content = new MultipartFormDataContent();
                var imagen = new ByteArrayContent(imagenCedula);
                var stringPayload = await Task.Run(() => JsonConvert.SerializeObject(obj));
                var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
                imagen.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                content.Add(imagen, "imagen");
                content.Add(httpContent, "obj");
                HttpResponseMessage respuesta = await client.PostAsync("api/Pasaporte/guardarLogPasaporte", content);
                if (respuesta.StatusCode == HttpStatusCode.OK || respuesta.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var res = await respuesta.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<IdentificacionResponse>(res);
                }
                else
                {
                    response.estado = false;
                    response.mensajes = "No ha sido posible conectarse al servidor";
                }
            }
            return response;
        }

        public async Task<IdentificacionResponse> guardarLogCertificado(LogCertificado obj, string token, byte[] imagenCedula)
        {
            IdentificacionResponse response = new IdentificacionResponse();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(UrlBase);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer",
                    parameter: token);

                var content = new MultipartFormDataContent();
                var imagen = new ByteArrayContent(imagenCedula);
                var stringPayload = await Task.Run(() => JsonConvert.SerializeObject(obj));
                var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
                imagen.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                content.Add(imagen, "imagen");
                content.Add(httpContent, "obj");
                HttpResponseMessage respuesta = await client.PostAsync("api/Certificado/guardarLogCertificado", content);
                if (respuesta.StatusCode == HttpStatusCode.OK || respuesta.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var res = await respuesta.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<IdentificacionResponse>(res);
                }
                else
                {
                    response.estado = false;
                    response.mensajes = "No ha sido posible conectarse al servidor";
                }
            }
            return response;
        }

        public async Task<IdentificacionResponse> guardarLogPapeleta(LogPapeleta obj, string token, byte[] imagenCedula)
        {
            IdentificacionResponse response = new IdentificacionResponse();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(UrlBase);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer",
                    parameter: token);

                var content = new MultipartFormDataContent();
                var imagen = new ByteArrayContent(imagenCedula);
                var stringPayload = await Task.Run(() => JsonConvert.SerializeObject(obj));
                var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
                imagen.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                content.Add(imagen, "imagen");
                content.Add(httpContent, "obj");
                HttpResponseMessage respuesta = await client.PostAsync("api/Papeleta/guardarLogPapeleta", content);
                if (respuesta.StatusCode == HttpStatusCode.OK || respuesta.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var res = await respuesta.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<IdentificacionResponse>(res);
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