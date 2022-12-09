using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SodigCiudadano.Models.Entidades
{
    public class TipoIdentificacion
    {
        public int idTipoIdentificacion { get; set; }
        public int idTipoPersona { get; set; }
        public string tipoIdentificacion { get; set; }
        public DateTime? fechaRegistro { get; set; }
    }

    public class TipoIdentificacionResponse
    {
        public bool estado { get; set; }
        public List<TipoIdentificacion> obj { get; set; }
        public string mensaje { get; set; }
    }
}