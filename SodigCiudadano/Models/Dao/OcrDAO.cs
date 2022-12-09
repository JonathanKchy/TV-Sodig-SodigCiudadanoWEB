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
    public class OcrDAO
    {
        private string UrlBase = ConfigurationManager.AppSettings["urlBase"].ToString();

        public async Task<OcrResponse> obtenerOcr(byte[] imagenByte, string token)
        {
            OcrResponse response = new OcrResponse();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(UrlBase);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer",
                    parameter: token);

                var content = new MultipartFormDataContent();
                var imagen = new ByteArrayContent(imagenByte);
                imagen.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                content.Add(imagen, "image");
                HttpResponseMessage respuesta = await client.PostAsync("api/Ocr/obtenerOcr", content);
                if (respuesta.StatusCode == HttpStatusCode.OK || respuesta.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var res = await respuesta.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<OcrResponse>(res);
                }
                else
                {
                    response.estado = false;
                    response.mensajes = "No ha sido posible conectarse al servidor";
                }
            }
            return response;
        }

        public async Task<NumeroIdentifiacionResponse> obtenerNumeroIdentificacion(Ocr obj, string token)
        {
            NumeroIdentifiacionResponse response = new NumeroIdentifiacionResponse();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(UrlBase);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer",
                    parameter: token);
                
                HttpResponseMessage respuesta = await client.PostAsJsonAsync("api/Identificacion/obtenerNumeroIdentificacion", obj);
                if (respuesta.StatusCode == HttpStatusCode.OK || respuesta.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var res = await respuesta.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<NumeroIdentifiacionResponse>(res);
                }
                else
                {
                    response.estado = false;
                    response.mensajes = "No ha sido posible conectarse al servidor";
                }
            }
            return response;
        }

        public async Task<NumeroIdentifiacionResponse> obtenerNumeroIdentificacionCertificadoIdentidad(Ocr obj, string token)
        {
            NumeroIdentifiacionResponse response = new NumeroIdentifiacionResponse();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(UrlBase);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer",
                    parameter: token);

                HttpResponseMessage respuesta = await client.PostAsJsonAsync("api/Identificacion/obtenerNumeroIdentificacionCertificadoIdentidad", obj);
                if (respuesta.StatusCode == HttpStatusCode.OK || respuesta.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var res = await respuesta.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<NumeroIdentifiacionResponse>(res);
                }
                else
                {
                    response.estado = false;
                    response.mensajes = "No ha sido posible conectarse al servidor";
                }
            }
            return response;
        }

        public async Task<NumeroIdentifiacionResponse> obtenerNumeroIdentificacionPasaporte(Ocr obj, string token)
        {
            NumeroIdentifiacionResponse response = new NumeroIdentifiacionResponse();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(UrlBase);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer",
                    parameter: token);

                HttpResponseMessage respuesta = await client.PostAsJsonAsync("api/Identificacion/obtenerNumeroIdentificacionPasaporte", obj);
                if (respuesta.StatusCode == HttpStatusCode.OK || respuesta.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var res = await respuesta.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<NumeroIdentifiacionResponse>(res);
                }
                else
                {
                    response.estado = false;
                    response.mensajes = "No ha sido posible conectarse al servidor";
                }
            }
            return response;
        }

        public async Task<NumeroIdentifiacionResponse> verificarCedulaPosterior(Ocr obj, string token)
        {
            NumeroIdentifiacionResponse response = new NumeroIdentifiacionResponse();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(UrlBase);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer",
                    parameter: token);

                HttpResponseMessage respuesta = await client.PostAsJsonAsync("api/Identificacion/verificarCedulaPosterior", obj);
                if (respuesta.StatusCode == HttpStatusCode.OK || respuesta.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var res = await respuesta.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<NumeroIdentifiacionResponse>(res);
                }
                else
                {
                    response.estado = false;
                    response.mensajes = "No ha sido posible conectarse al servidor";
                }
            }
            return response;
        }

        public async Task<NumeroIdentifiacionResponse> obtenerNumeroIdentificacionPapeleta(Ocr obj, string token)
        {
            NumeroIdentifiacionResponse response = new NumeroIdentifiacionResponse();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(UrlBase);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer",
                    parameter: token);

                HttpResponseMessage respuesta = await client.PostAsJsonAsync("api/Identificacion/obtenerNumeroIdentificacionPapeleta", obj);
                if (respuesta.StatusCode == HttpStatusCode.OK || respuesta.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var res = await respuesta.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<NumeroIdentifiacionResponse>(res);
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