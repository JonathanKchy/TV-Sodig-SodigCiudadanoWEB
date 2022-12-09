using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SodigCiudadano.Models.Entidades
{
    public class TipoPersona
    {
        public int idTipoPersona { get; set; }
        public string tipoPersona { get; set; }
        public DateTime? fechaRegistro { get; set; }
    }

    public class TipoPersonaResponse
    {
        public bool estado { get; set; }
        public List<TipoPersona> obj { get; set; }
        public string mensaje { get; set; }
    }
}