using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SodigCiudadano.Models.Entidades
{
    public class PruebaVida
    {
        public int idLogPruebaVida { get; set; }
        public int idUsuario { get; set; }
        public string correo { get; set; }
        public string pathPruebaVida { get; set; }
        public bool esVideo { get; set; }
        public bool esImage { get; set; }
        public string ipRegistro { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public string contenType { get; set; }
        public string extension { get; set; }
        public string cedulaOcr { get; set; }
        public HttpPostedFileBase video { get; set; }
        public byte[] imagen { get; set; }
    }

    public class PruebaVidaResponse
    {
        public bool estado { get; set; }
        public string mensajes { get; set; }
    }
}