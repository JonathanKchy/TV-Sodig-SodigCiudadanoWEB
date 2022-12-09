using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SodigCiudadano.Models.Entidades
{
    public class MotivoRevocacion
    {
        public int idMotivoSolicitud { get; set; }
        public string motivoSolicitud { get; set; }
        public Nullable<System.DateTime> fechaRegistro { get; set; }
    }

    public class MotivoRevocacionResponse
    {
        public bool estado { get; set; }
        public List<MotivoRevocacion> obj { get; set; }
        public string mensaje { get; set; }
    }
}