using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SodigCiudadano.Models.Entidades
{
    public class CodigoOTP
    {
        public int idUsuario { get; set; }
        public string correoUsuario { get; set; }
        public string codigoOtpIngresado { get; set; }
        public string identificacion { get; set; }
        public string ip { get; set; }
    }

    public class CodigoOTPResponse
    {
        public bool estado { get; set; }
        public string mensaje { get; set; }
    }
}