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
    public class TipoIdentificacionDAO
    {
        private string UrlBase = ConfigurationManager.AppSettings["urlBase"].ToString();

        public async Task<TipoIdentificacionResponse> ObtenerListaTipoidentificacionPorIdPersona(int idPersona, string token)
        {
            TipoIdentificacionResponse response = new TipoIdentificacionResponse();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(UrlBase);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer",
                    parameter: token);
                var url = "api/TipoIdentificacion/ObtenerListaTipoIdentificacionPorIdTipoPersona?idTipoPersona=" + idPersona;
                HttpResponseMessage respuesta = client.GetAsync(url).Result;

                if (respuesta.StatusCode == HttpStatusCode.OK || respuesta.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var res = await respuesta.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<TipoIdentificacionResponse>(res);
                }
                else
                {
                    response.estado = false;
                    response.mensaje = "No ha sido posible conectarse al servidor";
                }
            }
            return response;
        }

        internal Task<TipoIdentificacionResponse> ObtenerListaTipoidentificacionPorIdPersona(string token)
        {
            throw new NotImplementedException();
        }
    }
}