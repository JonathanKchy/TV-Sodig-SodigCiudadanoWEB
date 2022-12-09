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
    public class TicketDAO
    {
        private string UrlBase = ConfigurationManager.AppSettings["urlBase"].ToString();

        public async Task<TicketResponse> InsertarTicket(Ticket obj, string token, byte[] imagenByte = null)
        {
            TicketResponse response = new TicketResponse();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(UrlBase);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer", parameter: token);

                var content = new MultipartFormDataContent();
                ByteArrayContent imagen = null;
                if (imagenByte != null)
                {
                    imagen = new ByteArrayContent(imagenByte);
                    imagen.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                }
                var stringPayload = await Task.Run(() => JsonConvert.SerializeObject(obj));
                var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
                if (imagen != null)
                    content.Add(imagen, "imagen");
                content.Add(httpContent, "objTicket");
                HttpResponseMessage respuesta = await client.PostAsync("api/Ticket/InsertarTicket", content);
                if (respuesta.StatusCode == HttpStatusCode.OK || respuesta.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var res = await respuesta.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<TicketResponse>(res);
                }
                else
                {
                    response.estado = false;
                    response.mensaje = "No ha sido posible conectarse al servidor";
                }
            }
            return response;
        }

        public async Task<ListaComentarioTicketResponse> ObtenerListaComentarioPorIdTicketEIdentificacion(int idTicket, string identificacion, string token)
        {
            ListaComentarioTicketResponse response = new ListaComentarioTicketResponse();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(UrlBase);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer", parameter: token);
                string url = "api/Ticket/ObtenerListaComentarioPorIdTicketEIdentificacion?idTicket=" + idTicket + "&identificacion="+identificacion;
                HttpResponseMessage respuesta = client.GetAsync(url).Result;

                if (respuesta.StatusCode == HttpStatusCode.OK || respuesta.StatusCode == HttpStatusCode.InternalServerError || respuesta.StatusCode == HttpStatusCode.NotFound)
                {
                    var res = await respuesta.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<ListaComentarioTicketResponse>(res);
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