using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SodigCiudadano.Models.Entidades
{
    public class NumeroIdentifiacionResponse
    {
        public bool estado { get; set; }
        public string numeroIdentificacion { get; set; }
        public string mensajes { get; set; }
        public string codigoDatilar { get; set; }
        public bool esEcuatoriano { get; set; }
        public bool esMilitar { get; set; }
        public bool esDiscapacitado { get; set; }
    }
}