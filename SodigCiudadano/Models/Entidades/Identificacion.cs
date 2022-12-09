using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SodigCiudadano.Models.Entidades
{
    public class Identificacion
    {
    }

    public class LogCedulaFrontal
    {
        public int idLogCedulaFrontal { get; set; }
        public int idUsuario { get; set; }
        public string textOcrCedula { get; set; }
        public string pathImagenCedula { get; set; }
        public string cedulaOcr { get; set; }
        public string ipRegistro { get; set; }
        public DateTime? fechaRegistro { get; set; }
        public string contenType { get; set; }
        public string extension { get; set; }
        public bool esFrontal { get; set; }
    }

    public class LogPasaporte
    {
        public int idLogPasaporte { get; set; }
        public int idUsuario { get; set; }
        public string textOcrPasaporte { get; set; }
        public string pathImagenPasaporte { get; set; }
        public string cedulaOcr { get; set; }
        public string ipRegistro { get; set; }
        public DateTime? fechaRegistro { get; set; }
        public HttpPostedFileBase imagePasaporte { get; set; }
        public string contenType { get; set; }
        public string extension { get; set; }
    }

    public class LogCertificado
    {
        public int idLogCertificado { get; set; }
        public int idUsuario { get; set; }
        public string textOcrCertificado { get; set; }
        public string pathImagenCertificado { get; set; }
        public string cedulaOcr { get; set; }
        public string ipRegistro { get; set; }
        public DateTime? fechaRegistro { get; set; }
        public HttpPostedFileBase imageCertificado { get; set; }
        public string contenType { get; set; }
        public string extension { get; set; }
    }

    public class LogPapeleta
    {
        public int idLogPapeleta { get; set; }
        public int idUsuario { get; set; }
        public string pathPapeleta { get; set; }
        public string textOcr { get; set; }
        public string cedulaOcr { get; set; }
        public string ipRegistro { get; set; }
        public DateTime? fechaRegistro { get; set; }
        public HttpPostedFileBase imagePasaporte { get; set; }
        public string contenType { get; set; }
        public string extension { get; set; }
    }

    public class IdentificacionResponse
    {
        public bool estado { get; set; }
        public string mensajes { get; set; }
    }

}