using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SodigCiudadano.Models.Entidades
{
    public class TipoElector
    {
        public int idTipoElector { get; set; }
        public string tipoElector { get; set; }
        public DateTime? fechaRegistro { get; set; }
    }

    public class TipoElectorResponse
    {
        public bool estado { get; set; }
        public List<TipoElector> obj { get; set; }
        public string mensaje { get; set; }
    }
}