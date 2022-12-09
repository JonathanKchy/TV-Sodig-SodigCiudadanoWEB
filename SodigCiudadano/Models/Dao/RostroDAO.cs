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
    public class RostroDAO
    {
        private string UrlBase = ConfigurationManager.AppSettings["urlBase"].ToString();

        public async Task<RostroResponse> obtenerRostros(byte[] imagenByte, string token)
        {
            RostroResponse response = new RostroResponse();  
            
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
                HttpResponseMessage respuesta = await client.PostAsync("api/Rostro/obtenerRostros", content);
                if (respuesta.StatusCode == HttpStatusCode.OK || respuesta.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var res = await respuesta.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<RostroResponse>(res);
                }
                else
                {
                    response.estado = false;
                    response.mensajes = "No ha sido posible conectarse al servidor";
                }

            }
            return response;
        }

        public async Task<RostroResponse> obtenerRostrosWebcam(byte[] imagenByte, string token)
        {
            RostroResponse response = new RostroResponse();

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
                HttpResponseMessage respuesta = await client.PostAsync("api/Rostro/obtenerRostrosWebCam", content);
                if (respuesta.StatusCode == HttpStatusCode.OK || respuesta.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var res = await respuesta.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<RostroResponse>(res);
                }
                else
                {
                    response.estado = false;
                    response.mensajes = "No ha sido posible conectarse al servidor";
                }

            }
            return response;
        }

        public async Task<RostroResponse> obtenerRostrosPapeletaVotacion(byte[] imagenByte, string token)
        {
            RostroResponse response = new RostroResponse();

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
                HttpResponseMessage respuesta = await client.PostAsync("api/Rostro/obtenerRostrosPapeletaVotacion", content);
                if (respuesta.StatusCode == HttpStatusCode.OK || respuesta.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var res = await respuesta.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<RostroResponse>(res);
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