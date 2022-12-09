using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SodigCiudadano.Models.Entidades
{
    public class TipoContenedor
    {
        public int idTipoContenedor { get; set; }
        public string tipoContenedor { get; set; }
        public DateTime? fechaRegistro { get; set; }
    }

    public class TipoContenedorResponse
    {
        public bool estado { get; set; }
        public List<TipoContenedor> obj { get; set; }
        public string mensaje { get; set; }
    }

    public class NombreContenedorResponse
    {
        public bool estado { get; set; }
        public TipoContenedor obj { get; set; }
        public string mensaje { get; set; }
    }

}