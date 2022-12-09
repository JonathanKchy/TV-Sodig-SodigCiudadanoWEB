using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SodigCiudadano.Models.Entidades
{
    public class TipoSolicitud
    {
        public int idTipoSolicitud { get; set; }
        public string tipoSolicitud { get; set; }
        public DateTime? fechaRegistro { get; set; }
    }

    public class TipoSolicitudResponse
    {
        public bool estado { get; set; }
        public List<TipoSolicitud> obj { get; set; }
        public string mensaje { get; set; }
    }

    public class NombreSolicitudResponse
    {
        public bool estado { get; set; }
        public TipoSolicitud obj { get; set; }
        public string mensaje { get; set; }
    }
}