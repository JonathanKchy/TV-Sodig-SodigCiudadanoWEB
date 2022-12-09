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
    public class MotivoRevocacionDAO
    {
        private string UrlBase = ConfigurationManager.AppSettings["urlBase"].ToString();

        public async Task<MotivoRevocacionResponse> ObtenerListaMotivoRenovacion(string token)
        {
            MotivoRevocacionResponse response = new MotivoRevocacionResponse();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(UrlBase);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer",
                    parameter: token);
                var url = "api/Catalogo/ObtenerListaMotivoRenovacion";
                HttpResponseMessage respuesta = client.GetAsync(url).Result;

                if (respuesta.StatusCode == HttpStatusCode.OK || respuesta.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var res = await respuesta.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<MotivoRevocacionResponse>(res);
                }
                else
                {
                    response.estado = false;
                    response.mensaje = "Ha ocurrido un problema interno. Por favor, inténtelo más tarde";
                }
            }
            return response;
        }
    }
}