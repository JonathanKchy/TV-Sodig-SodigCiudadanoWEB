using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SodigCiudadano.Models.Entidades
{
    public class Pais
    {
        public string idPais { get; set; }
        public string pais { get; set; }
        public string codigoPais { get; set; }
        public DateTime? fechaRegistro { get; set; }
    }

    public class PaisResponse
    {
        public bool estado { get; set; }
        public List<Pais> lista { get; set; }
        public Pais obj { get; set; }
        public string mensajes { get; set; }
    }
}