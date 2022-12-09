using Newtonsoft.Json;
using System;

namespace SodigCiudadano.Models.Entidades
{
    public class Pago
    {
        public int idPago { get; set; }
        public int idSolicitud { get; set; }
        public DateTime fecha { get; set; }
        public DateTime fechaAutorizacion { get; set; }
        public string sha { get; set; }
        public string status { get; set; }
        public string trankey { get; set; }
        public string ip { get; set; }
        public string numeroFactura { get; set; }
        public string valor { get; set; }
    }

    public class PagoResponse
    {
        public bool estado { get; set; }
        public Pago obj { get; set; }
        public string mensajes { get; set; }
    }

    public class CheckOutResponse
    {
        public bool estado { get; set; }
        public string mensaje { get; set; }
        public result result { get; set; }
        public string buildNumber { get; set; }
        public string timestamp { get; set; }
        public string ndc { get; set; }
        public string id { get; set; }
    }

    public class result
    {
        public string code { get; set; }
        public string description { get; set; }
    }

    public class ProcesamientoPagoResponse
    {
        public bool estado { get; set; }
        public string id { get; set; }
        public string paymentType { get; set; }
        public string paymentBrand { get; set; }
        public string amount { get; set; }
        public string currency { get; set; }
        public string descriptor { get; set; }
        public string buildNumber { get; set; }
        public string timestamp { get; set; }
        public string ndc { get; set; }
        public result result { get; set; }
        public threeDSecure threeDSecure { get; set; }
        public customParameter customParameter { get; set; }
        public Risk risk { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

    }

    public class Card
    {
        public string bin { get; set; }
        public string last4Digits { get; set; }
        public string holder { get; set; }
        public string expiryMonth { get; set; }
        public string expiryYear { get; set; }

    }

    public class threeDSecure
    {
        public string eci { get; set; }
    }

    public class customParameter
    {
        public string CTPE_DESCRIPTOR_TEMPLATE { get; set; }

    }

    public class Risk
    {
        public string score { get; set; }
    }

    public class CheckOutEnvio
    {
        public string amount { get; set; }
        public string givenName { get; set; }
        public string middleName { get; set; }
        public string surname { get; set; }
        public string ip { get; set; }
        public string merchantCustomerId { get; set; }
        public string merchantTransactionId { get; set; }
        public string email { get; set; }
        public string identificationDocId { get; set; }
        public string phone { get; set; }
        public string street1 { get; set; }
        public string country { get; set; }
        public string postcode { get; set; }
        public string SHOPPER_VAL_BASE0 { get; set; }
        public string SHOPPER_VAL_BASEIMP { get; set; }
        public string SHOPPER_VAL_IVA { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string price { get; set; }
        public string quantity { get; set; }
        public string SHOPPER_VERSIONDF { get; set; }
    }

    public class AnulacionPagoResponse
    {
        public bool estado { get; set; }
        public string id { get; set; }
        public string referenceId { get; set; }
        public string paymentType { get; set; }
        public string amount { get; set; }
        public string currency { get; set; }
        public string descriptor { get; set; }
        public string merchantTransactionId { get; set; }
        public result result { get; set; }
    }

    public class PagoLog
    {
        public int idLog { get; set; }
        public string identificacion { get; set; }
        public string jsonEnvio { get; set; }
        public string jsonRespuesta { get; set; }
        public string idRespuestaPago { get; set; }
        public int idInicioSesion { get; set; }
        public int idUsuario { get; set; }
    }

    public class PagoCorreo
    {
        public string identificacion { get; set; }
        public string motivo { get; set; }
        public string correoEnvio { get; set; }
        public string nombreProceso { get; set; }
        public string nombreUsuario { get; set; }
    }

    public class PagoCodigo
    {
        public string codigoCatalogo { get; set; }
        public string descripcion { get; set; }
        public Nullable<System.DateTime> fechaRegistro { get; set; }
    }

    public class PagoCodigoResponse
    {
        public bool estado { get; set; }
        public PagoCodigo obj { get; set; }
        public string mensaje { get; set; }
    }





}