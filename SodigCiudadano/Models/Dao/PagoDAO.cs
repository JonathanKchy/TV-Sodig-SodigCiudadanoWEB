using Newtonsoft.Json;
using RestSharp;
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
    public class PagoDAO
    {
        private string UrlBase = ConfigurationManager.AppSettings["urlBase"].ToString();
        private LogDAO objLogDao = new LogDAO();
        public async Task<PagoResponse> insertarPago(Pago obj, string token)
        {
            PagoResponse response = new PagoResponse();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(UrlBase);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer",
                    parameter: token);
                HttpResponseMessage respuesta = client.PostAsJsonAsync("api/Pago/insertarPago", obj).Result;

                if (respuesta.StatusCode == HttpStatusCode.OK || respuesta.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var res = await respuesta.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<PagoResponse>(res);
                }
                else
                {
                    response.estado = false;
                    response.mensajes = "Ha existido un error. Por favor, inténtelo más tarde. conectarse al servidor";
                }
            }
            return response;
        }

        public async Task<CheckOutResponse> generarCheckOutId()
        {
            CheckOutResponse response = new CheckOutResponse();

            using (HttpClient client = new HttpClient())
            {
                object obj = new object();
                string token = ConfigurationManager.AppSettings["Authorization"].ToString();
                string urlPago = ConfigurationManager.AppSettings["urlPago"].ToString();
                string entityId = ConfigurationManager.AppSettings["entityId"].ToString();
                string currency = ConfigurationManager.AppSettings["currency"].ToString();
                string paymentType = ConfigurationManager.AppSettings["paymentType"].ToString();
                client.BaseAddress = new Uri(urlPago);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer",
                    parameter: token);
                string url = "v1/checkouts?entityId=" + entityId + "&amount=20.00&currency=" + currency + "&paymentType=" + paymentType;
                HttpResponseMessage respuesta = client.PostAsJsonAsync(url, obj).Result;

                if (respuesta.StatusCode == HttpStatusCode.OK || respuesta.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var res = await respuesta.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<CheckOutResponse>(res);
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

        public async Task<ProcesamientoPagoResponse> ProcesarPago(string resourcePath, string identificacion, int idUsuario, string tokenApi, string correo)
        {
            ProcesamientoPagoResponse response = new ProcesamientoPagoResponse();
            response.estado = true;
            response.result = new result();
            PagoLog log = new PagoLog();
            log.identificacion = identificacion;
            log.idUsuario = idUsuario;
            log.jsonEnvio = resourcePath;

            using (HttpClient client = new HttpClient())
            {
                string token = ConfigurationManager.AppSettings["Authorization"].ToString();
                string urlPago = ConfigurationManager.AppSettings["urlPago"].ToString();
                string entityId = ConfigurationManager.AppSettings["entityId"].ToString();
                client.BaseAddress = new Uri(urlPago);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer",
                    parameter: token);
                string url = resourcePath + "?entityId=" + entityId;
                HttpResponseMessage respuesta = client.GetAsync(url).Result;

                if (respuesta.StatusCode == HttpStatusCode.OK || respuesta.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var res = await respuesta.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<ProcesamientoPagoResponse>(res);
                    response.estado = true;
                    log.jsonRespuesta = res;
                    log.idRespuestaPago = response.id;
                    PagoResponse objPagoResponse = await insertarLogPago(log, tokenApi);
                }
                else
                {
                    var res = await respuesta.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<ProcesamientoPagoResponse>(res);
                    response.estado = false;
                    response.result = new result();
                    response.result.description = "Ha existido un error. Por favor, inténtelo más tarde. conectarse al servidor";
                    log.jsonRespuesta = res;
                    log.idRespuestaPago = response.id;
                    PagoResponse objPagoResponse = await insertarLogPago(log, tokenApi);

                }
            }
            return response;
        }

        public async Task<CheckOutResponse> generarCheckOutIdFase2(CheckOutEnvio objCheck, string identificacion, string tokenApi)
        {
            CheckOutResponse response = new CheckOutResponse();            
            string token = ConfigurationManager.AppSettings["Authorization"].ToString();
            string urlPago = ConfigurationManager.AppSettings["urlPago"].ToString();
            string entityId = ConfigurationManager.AppSettings["entityId"].ToString();
            string currency = ConfigurationManager.AppSettings["currency"].ToString();
            string paymentType = ConfigurationManager.AppSettings["paymentType"].ToString();
            string IDCARD = ConfigurationManager.AppSettings["IDCARD"].ToString();
            string MID = ConfigurationManager.AppSettings["MID"].ToString();
            string TID = ConfigurationManager.AppSettings["TID"].ToString();
            string SERV = ConfigurationManager.AppSettings["SERV"].ToString();
            string ECI = ConfigurationManager.AppSettings["ECI"].ToString();
            string Shoper_version = ConfigurationManager.AppSettings["Shoper_version"].ToString();
            var client = new RestClient(urlPago + "/v1/checkouts");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddHeader("Authorization", "Bearer " + token);
            request.AddParameter("entityId", entityId);
            request.AddParameter("amount", objCheck.amount);
            request.AddParameter("currency", currency);
            request.AddParameter("paymentType", paymentType);
            request.AddParameter("customer.givenName", objCheck.givenName);
            request.AddParameter("customer.middleName", objCheck.middleName);
            request.AddParameter("customer.surname", objCheck.surname);
            request.AddParameter("customer.ip", objCheck.ip);
            request.AddParameter("customer.merchantCustomerId", objCheck.merchantCustomerId);
            request.AddParameter("merchantTransactionId", objCheck.merchantTransactionId);
            request.AddParameter("customer.email", objCheck.email);
            request.AddParameter("customer.identificationDocType", IDCARD);
            request.AddParameter("customer.identificationDocId", objCheck.identificationDocId);
            request.AddParameter("customer.phone", objCheck.phone);
            request.AddParameter("billing.street1", objCheck.street1);
            request.AddParameter("billing.country", objCheck.country);
            request.AddParameter("billing.postcode", objCheck.postcode);
            request.AddParameter("customParameters[SHOPPER_MID]", MID);
            request.AddParameter("customParameters[SHOPPER_TID]", TID);
            request.AddParameter("customParameters[SHOPPER_ECI]", ECI);
            request.AddParameter("customParameters[SHOPPER_PSERV]", SERV);
            request.AddParameter("customParameters[SHOPPER_VAL_BASE0]", objCheck.SHOPPER_VAL_BASE0);
            request.AddParameter("customParameters[SHOPPER_VAL_BASEIMP]", objCheck.SHOPPER_VAL_BASEIMP);
            request.AddParameter("customParameters[SHOPPER_VAL_IVA]", objCheck.SHOPPER_VAL_IVA);
            request.AddParameter("cart.items[\"0\"].name", objCheck.name);
            request.AddParameter("cart.items[\"0\"].description", objCheck.description);
            request.AddParameter("cart.items[\"0\"].price", objCheck.price);
            request.AddParameter("cart.items[\"0\"].quantity", objCheck.quantity);
            request.AddParameter("customParameters[SHOPPER_VERSIONDF]", Shoper_version); //2
            if(Convert.ToBoolean(ConfigurationManager.AppSettings["testBotonPagos"].ToString()))
              request.AddParameter("testMode", "EXTERNAL");
            request.AddParameter("risk.parameters[USER_DATA2]", "SODIG");

            IRestResponse respuesta = await client.ExecuteAsync(request);
            if (respuesta.StatusCode == HttpStatusCode.OK || respuesta.StatusCode == HttpStatusCode.InternalServerError)
            {
                var res = respuesta.Content;
                response = JsonConvert.DeserializeObject<CheckOutResponse>(res);
                LogPagoCheckOut objPagoCheck = new LogPagoCheckOut();
                objPagoCheck.identificacion = identificacion;
                objPagoCheck.response = res;
                var logsCheckout = objLogDao.GuardarLogPagoCheckOut(objPagoCheck, tokenApi);
                response.estado = true;
            }
            else
            {
                var res = respuesta.Content;
                response = JsonConvert.DeserializeObject<CheckOutResponse>(res);
                LogPagoCheckOut objPagoCheck = new LogPagoCheckOut();
                objPagoCheck.identificacion = identificacion;
                objPagoCheck.response = res;
                var logsCheckout = objLogDao.GuardarLogPagoCheckOut(objPagoCheck, tokenApi);
                response.estado = false;
                response.mensaje = response.result.description;
            }

            return response;
        }

        public async Task<FacturaResponse> generarFactura(Factura obj, string token)
        {
            FacturaResponse response = new FacturaResponse();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(UrlBase);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer",
                    parameter: token);
                HttpResponseMessage respuesta = client.PostAsJsonAsync("api/Factura/facturar", obj).Result;

                if (respuesta.StatusCode == HttpStatusCode.OK || respuesta.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var res = await respuesta.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<FacturaResponse>(res);
                }
                else
                {
                    response.estado = false;
                    response.mensajes = "Ha existido un error. Por favor, inténtelo más tarde. conectarse al servidor";
                }
            }
            return response;
        }

        public async Task<bool> AnularPago(string Id, string amount, string identificacion, int idUsuario, string tokenApi)
        {
            bool respuestaPago = false;
            AnulacionPagoResponse response = new AnulacionPagoResponse();
            response.estado = false;
            response.result = new result();
            PagoLog log = new PagoLog();
            log.identificacion = identificacion;
            log.idUsuario = idUsuario;
            log.jsonEnvio = amount;
            log.idRespuestaPago = Id;
            using (HttpClient client = new HttpClient())
            {
                object obj = new object();
                string token = ConfigurationManager.AppSettings["Authorization"].ToString();
                string urlPago = ConfigurationManager.AppSettings["urlPago"].ToString();
                string entityId = ConfigurationManager.AppSettings["entityId"].ToString();
                string codigoPagoExitoso = ConfigurationManager.AppSettings["codigoPagoExitoso"].ToString();
                client.BaseAddress = new Uri(urlPago);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer",
                    parameter: token);
                string url = "/v1/payments/" + Id + "?authentication.entityId=" + entityId + "&paymentType=RF&amount=" + amount + "&currency=USD";
                HttpResponseMessage respuesta = client.PostAsJsonAsync(url, obj).Result;

                if (respuesta.StatusCode == HttpStatusCode.OK || respuesta.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var res = await respuesta.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<AnulacionPagoResponse>(res);

                    log.jsonRespuesta = res;
                    if (response.result.code == codigoPagoExitoso)
                        response.estado = true;

                    PagoResponse objPagoResponse = await insertarLogPago(log, tokenApi);
                }
                else
                {
                    var res = await respuesta.Content.ReadAsStringAsync();
                    log.jsonRespuesta = res;
                    response.estado = false;
                    response.result = new result();
                    response.result.description = "Ha existido un error. Por favor, inténtelo más tarde. conectarse al servidor";
                    PagoResponse objPagoResponse = await insertarLogPago(log, tokenApi);
                }
            }
            return respuestaPago;
        }

        public async Task<PagoResponse> insertarLogPago(PagoLog obj, string token)
        {
            PagoResponse response = new PagoResponse();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(UrlBase);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer",
                    parameter: token);
                HttpResponseMessage respuesta = client.PostAsJsonAsync("api/Pago/insertarLogPago", obj).Result;

                if (respuesta.StatusCode == HttpStatusCode.OK || respuesta.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var res = await respuesta.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<PagoResponse>(res);
                }
                else
                {
                    response.estado = false;
                    response.mensajes = "Ha existido un error. Por favor, inténtelo más tarde. conectarse al servidor";
                }
            }
            return response;
        }

        public async Task<PagoResponse> EnviarCorreoPago(PagoCorreo obj, string token)
        {
            PagoResponse response = new PagoResponse();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(UrlBase);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer",
                    parameter: token);
                HttpResponseMessage respuesta = client.PostAsJsonAsync("api/Pago/enviarCorreo", obj).Result;

                if (respuesta.StatusCode == HttpStatusCode.OK || respuesta.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var res = await respuesta.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<PagoResponse>(res);
                }
                else
                {
                    response.estado = false;
                    response.mensajes = "Ha existido un error. Por favor, inténtelo más tarde. conectarse al servidor";
                }
            }
            return response;
        }

        public async Task<PagoResponse> enviarCorreProcesoCulminadoo(PagoCorreo obj, string token)
        {
            PagoResponse response = new PagoResponse();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(UrlBase);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer",
                    parameter: token);
                HttpResponseMessage respuesta = client.PostAsJsonAsync("api/Pago/enviarCorreProcesoCulminadoo", obj).Result;

                if (respuesta.StatusCode == HttpStatusCode.OK || respuesta.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var res = await respuesta.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<PagoResponse>(res);
                }
                else
                {
                    response.estado = false;
                    response.mensajes = "Ha existido un error. Por favor, inténtelo más tarde. conectarse al servidor";
                }
            }
            return response;
        }

        public async Task<PagoResponse> EnviarCorreoProcesoCompleto(PagoCorreo obj, string token)
        {
            PagoResponse response = new PagoResponse();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(UrlBase);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer",
                    parameter: token);
                HttpResponseMessage respuesta = client.PostAsJsonAsync("api/Pago/enviarCorreo", obj).Result;

                if (respuesta.StatusCode == HttpStatusCode.OK || respuesta.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var res = await respuesta.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<PagoResponse>(res);
                }
                else
                {
                    response.estado = false;
                    response.mensajes = "Ha existido un error. Por favor, inténtelo más tarde. conectarse al servidor";
                }
            }
            return response;
        }


        public async Task<PagoCodigoResponse> ObtenerRespuestaPorCodigo(string codigo, string token)
        {
            PagoCodigoResponse response = new PagoCodigoResponse();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(UrlBase);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer", parameter: token);
                string url = "api/Pago/ObtenerRespuestaPorCodigo?codigo=" + codigo;
                HttpResponseMessage respuesta = client.GetAsync(url).Result;

                if (respuesta.StatusCode == HttpStatusCode.OK || respuesta.StatusCode == HttpStatusCode.InternalServerError || respuesta.StatusCode == HttpStatusCode.NotFound)
                {
                    var res = await respuesta.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<PagoCodigoResponse>(res);
                }
                else
                {
                    response.estado = false;
                    response.mensaje = "Ha existido un error. Por favor, inténtelo más tarde. conectarse al servidor";
                }
            }
            return response;
        }
    }
}