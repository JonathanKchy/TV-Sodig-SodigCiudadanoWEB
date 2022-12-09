using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SodigCiudadano.Models.Entidades
{
    public class Ciudad
    {
        public string idCiudad { get; set; }
        public string idProvincia { get; set; }
        public string ciudad { get; set; }
        public string provincia { get; set; }
        public string codigoProvincia { get; set; }
        public DateTime? fechaRegistr { get; set; }
    }

    public class CiudadResponse
    {
        public bool estado { get; set; }
        public List<Ciudad> lista { get; set; }
        public Ciudad obj { get; set; }
        public string mensajes { get; set; }
    }

}