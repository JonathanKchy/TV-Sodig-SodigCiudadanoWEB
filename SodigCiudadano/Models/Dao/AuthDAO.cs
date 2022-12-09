using Newtonsoft.Json;
using SodigCiudadano.Models.Entidades;
using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SodigCiudadano.Models.Dao
{
    public class AuthDAO
    {
        private  string UrlBase = ConfigurationManager.AppSettings["urlBase"].ToString();        

        public Captcha VerifcarCaptcha(string response)
        {
            string secret = ConfigurationManager.AppSettings["captchaKey"].ToString();
            var client = new System.Net.WebClient();
            var GoogleReply = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, response));
            Captcha obj = JsonConvert.DeserializeObject<Captcha>(GoogleReply);
            return obj;
        }

        public async Task<ApiResponse> IniciarSesion(string identificacion, string ip, string correo, bool aceptaTerminos)
        {
            ApiResponse response = new ApiResponse();
            Usuario usuario = new Usuario();
            usuario.Identificacion = identificacion;
            usuario.ip = ip;
            usuario.correo = correo;
            usuario.aceptaTerminos = aceptaTerminos;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(UrlBase);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage respuesta = client.PostAsJsonAsync("api/Usuario/IniciarSesion", usuario).Result;

                if (respuesta.StatusCode == HttpStatusCode.OK || respuesta.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var res = await respuesta.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<ApiResponse>(res);
                }
                else
                {
                    response.estado = false;
                    response.mensaje = "Ha existido un error. Por favor, inténtelo más tarde. conectarse al servidor";
                }
            }
            return response;
        }

        public async Task<CodigoOTPResponse> crearCodigoOtp(CodigoOTP obj, string token)
        {
            CodigoOTPResponse response = new CodigoOTPResponse();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(UrlBase);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer",
                    parameter: token);
                HttpResponseMessage respuesta = client.PostAsJsonAsync("api/Usuario/CrearCodigoOtp", obj).Result;

                if (respuesta.StatusCode == HttpStatusCode.OK || respuesta.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var res = await respuesta.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<CodigoOTPResponse>(res);
                }
                else
                {
                    response.estado = false;
                    response.mensaje = "No ha sido posible conectarse al servidor";
                }
            }
            return response;
        }

        public async Task<CodigoOTPResponse> verificarCodigoOtp(CodigoOTP obj, string token)
        {
            CodigoOTPResponse response = new CodigoOTPResponse();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(UrlBase);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer",
                    parameter: token);
                HttpResponseMessage respuesta = client.PostAsJsonAsync("api/Usuario/VerificarCodigoOtp", obj).Result;

                if (respuesta.StatusCode == HttpStatusCode.OK || respuesta.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var res = await respuesta.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<CodigoOTPResponse>(res);
                }
                else
                {
                    response.estado = false;
                    response.mensaje = "No ha sido posible conectarse al servidor";
                }
            }
            return response;
        }


        public async Task<ApiResponse> ActualizarNumeroIntentos(Usuario obj, string token)
        {
            ApiResponse response = new ApiResponse();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(UrlBase);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer",
                    parameter: token);
                HttpResponseMessage respuesta = client.PostAsJsonAsync("api/Usuario/ActualizarNumeroIntentos", obj).Result;

                if (respuesta.StatusCode == HttpStatusCode.OK || respuesta.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var res = await respuesta.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<ApiResponse>(res);
                }
                else
                {
                    response.estado = false;
                    response.mensaje = "Ha existido un error. Por favor, inténtelo más tarde. conectarse al servidor";
                }
            }
            return response;
        }

        public async Task<NumeroIntentosResponse> ObtenerNumeroIntentosPorIdUsuario(int idUsuario, string identificacion, string token)
        {
            NumeroIntentosResponse response = new NumeroIntentosResponse();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(UrlBase);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer", parameter: token);
                string url = "api/Usuario/ObtenerNumeroIntentosPorIdUsuario?idUsuario=" + idUsuario + "&numeroIdentificacion=" + identificacion;
                HttpResponseMessage respuesta = client.GetAsync(url).Result;

                if (respuesta.StatusCode == HttpStatusCode.OK || respuesta.StatusCode == HttpStatusCode.InternalServerError || respuesta.StatusCode == HttpStatusCode.NotFound)
                {
                    var res = await respuesta.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<NumeroIntentosResponse>(res);
                }
                else
                {
                    response.estado = false;
                    response.mensajes = "Ha existido un error. Por favor, inténtelo más tarde. conectarse al servidor";
                }
            }
            return response;
        }


    }
}