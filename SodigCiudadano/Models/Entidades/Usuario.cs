using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SodigCiudadano.Models.Entidades
{
    public class Usuario
    {
        public int idUsuario { get; set; }
        public string Identificacion { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public string access_token { get; set; }
        public string ip { get; set; }
        public string correo { get; set; }
        public bool aceptaTerminos { get; set; }
    }

    public class NumeroIntentos
    {
        public int idUsuario { get; set; }
        public string numeroIdentificacion { get; set; }
        public int numeroIntentos { get; set; }
    }

    public class NumeroIntentosResponse
    {
        public bool estado { get; set; }
        public NumeroIntentos obj { get; set; }
        public string mensajes { get; set; }
    }

}