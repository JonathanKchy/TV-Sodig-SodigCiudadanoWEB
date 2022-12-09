using System;
using System.Collections.Generic;

namespace SodigCiudadano.Models.Entidades
{
    public class UsoCertificado
    {
        public string idUsoCertificado { get; set; }
        public string usoCertificado { get; set; }
        public DateTime? fechaRegistro { get; set; }
    }

    public class UsoCertificadoResponse
    {
        public bool estado { get; set; }
        public List<UsoCertificado> lista { get; set; }
        public UsoCertificado obj { get; set; }
        public string mensajes { get; set; }
    }
}