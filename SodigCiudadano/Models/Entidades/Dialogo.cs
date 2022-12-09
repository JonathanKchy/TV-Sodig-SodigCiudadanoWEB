using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SodigCiudadano.Models.Entidades
{
    public class Dialogo
    {
        public int idDialogo { get; set; }
        public string dialogo { get; set; }
        public DateTime? fechaRegistro { get; set; }
    }

    public class DialogoResponse
    {
        public bool estado { get; set; }
        public Dialogo obj { get; set; }
        public string mensaje { get; set; }
    }
}