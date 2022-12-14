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
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace SodigCiudadano.Models.Dao
{
    public class InformacionDAO
    {
        private string UrlBase = ConfigurationManager.AppSettings["urlBase"].ToString();

        public async Task<InformacionResponse> ObtenerImagen(string identificacion, string token)
        {
            int contador = 0;
            InformacionResponse response = new InformacionResponse();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(UrlBase);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer", parameter: token);
                string url = "api/Informacion/ObtenerImagen?identificacion="+identificacion;

                HttpResponseMessage respuesta;

                do
                {
                    if (contador != 0)
                    {
                        Thread.Sleep(3000);
                    }
                    respuesta = client.GetAsync(url).Result;
                    var res = await respuesta.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<InformacionResponse>(res);
                    //aumento contador
                    contador += 1;

                } while (respuesta.StatusCode != HttpStatusCode.OK && contador < 13);
                

                //if (respuesta.StatusCode == HttpStatusCode.OK || respuesta.StatusCode == HttpStatusCode.InternalServerError || respuesta.StatusCode == HttpStatusCode.NotFound)
                //{
                //    var res = await respuesta.Content.ReadAsStringAsync();
                //    response = JsonConvert.DeserializeObject<InformacionResponse>(res);
                //}
                //else
                //{
                //    response.estado = false;
                //    response.mensajes = "No ha sido posible conectarse al servidor";
                //}


            }
            return response;
        }

        public async Task<InformacionPruebaVidaResponse> VerificarPruebaVida(byte[] imagenBase64, byte[] imagenDocumento, byte[] imagenVideo, byte[] imagenSelfie, string token, PruebaVida pruebaVida)
        {
            InformacionPruebaVidaResponse response = new InformacionPruebaVidaResponse();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(UrlBase);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer",
                    parameter: token);
                var stringPayload = await Task.Run(() => JsonConvert.SerializeObject(pruebaVida));
                var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

                var content = new MultipartFormDataContent();
                var imagen1 = new ByteArrayContent(imagenBase64);
                imagen1.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                content.Add(imagen1, "imagenBase64");
                var imagen2 = new ByteArrayContent(imagenDocumento);
                imagen2.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                content.Add(imagen2, "imagenDocumento");
                var imagen3 = new ByteArrayContent(imagenVideo);
                imagen3.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                content.Add(imagen3, "imagenVideo");
                var imagen4 = new ByteArrayContent(imagenSelfie);
                imagen4.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                content.Add(imagen4, "imagenSelfie");

                content.Add(httpContent, "pruebaVida");

                HttpResponseMessage respuesta = await client.PostAsync("api/Informacion/VerificarPruebaVida", content);
                if (respuesta.StatusCode == HttpStatusCode.OK || respuesta.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var res = await respuesta.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<InformacionPruebaVidaResponse>(res);
                }
                else
                {
                    response.estado = false;
                    response.mensajes = "No ha sido posible conectarse al servidor";
                }
            }
            return response;
        }

        public async Task<InformacionResponse> ObtenerInformacionCNE(string identificacion, string token)
        {
            InformacionResponse response = new InformacionResponse();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(UrlBase);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer", parameter: token);
                string url = "api/Informacion/ObtenerInformacionCNE?identificacion=" + identificacion;
                HttpResponseMessage respuesta = client.GetAsync(url).Result;

                if (respuesta.StatusCode == HttpStatusCode.OK || respuesta.StatusCode == HttpStatusCode.InternalServerError || respuesta.StatusCode == HttpStatusCode.NotFound)
                {
                    var res = await respuesta.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<InformacionResponse>(res);
                }
                else
                {
                    response.estado = false;
                    response.mensajes = "No ha sido posible conectarse al servidor";
                }
            }
            return response;
        }

        public async Task<InformacionResponse> ObtenerInformacionDatosDemograficos(string identificacion, string token)
        {
            int contador = 0;
            InformacionResponse response = new InformacionResponse();
            using (HttpClient client = new HttpClient())
            {
                //solicitud dinardap
                client.BaseAddress = new Uri(UrlBase);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer", parameter: token);
                string url = "api/Informacion/ObtenerInformacionDatosDemograficos?identificacion=" + identificacion;
                HttpResponseMessage respuesta;
                do
                {
                     respuesta = client.GetAsync(url).Result;

                    //if (respuesta.StatusCode == HttpStatusCode.OK )//|| respuesta.StatusCode == HttpStatusCode.InternalServerError || respuesta.StatusCode == HttpStatusCode.NotFound)
                    //{
                    //    var res = await respuesta.Content.ReadAsStringAsync();
                    //    response = JsonConvert.DeserializeObject<InformacionResponse>(res);
                    //}
                    //else
                    //{
                    //    response.estado = false;
                    //    response.mensajes = "No ha sido posible conectarse al servidor";
                    //}



                    //temporizados de 3 segundos
                    if (contador != 0)
                    {
                        Thread.Sleep(3000);
                    }
                    
                    //responseDatosDemograficos = await objInformacionDAO.ObtenerInformacionDatosDemograficos(identificacion, token);

                        var res = await respuesta.Content.ReadAsStringAsync();
                        response = JsonConvert.DeserializeObject<InformacionResponse>(res);
                    //aumento contador
                    contador += 1;
                } while (respuesta.StatusCode != HttpStatusCode.OK && contador < 13);
            }
            return response;
        }

    }
}