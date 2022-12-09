using System;
using System.Collections.Generic;

namespace SodigCiudadano.Models.Entidades
{
    public class Provincia
    {
        public string idProvincia { get; set; }
        public string idPais { get; set; }
        public string provincia { get; set; }
        public string pais { get; set; }
        public string codigoProvincia { get; set; }
        public DateTime? fechaRegistro { get; set; }
    }

    public class ProvinciaResponse
    {
        public bool estado { get; set; }
        public List<Provincia> lista { get; set; }
        public Provincia obj { get; set; }
        public string mensajes { get; set; }
    }
}