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
    public class PruebaVidaDAO
    {
        private string UrlBase = ConfigurationManager.AppSettings["urlBase"].ToString();

        public async Task<PruebaVidaResponse> guardarLogPruebaVida(PruebaVida obj, string token, byte[] file)
        {
            PruebaVidaResponse response = new PruebaVidaResponse();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(UrlBase);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer",
                    parameter: token);

                var content = new MultipartFormDataContent();
                var archivo = new ByteArrayContent(file);
                var stringPayload = await Task.Run(() => JsonConvert.SerializeObject(obj));
                var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
                archivo.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                content.Add(archivo, "imagen");
                content.Add(httpContent, "obj");
                HttpResponseMessage respuesta = await client.PostAsync("api/PruebaVida/guardarLogPruebaVida", content);
                if (respuesta.StatusCode == HttpStatusCode.OK || respuesta.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var res = await respuesta.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<PruebaVidaResponse>(res);
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