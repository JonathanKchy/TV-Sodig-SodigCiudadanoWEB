using System;
using System.Collections.Generic;

namespace SodigCiudadano.Models.Entidades
{
    public class ActividadEconomica
    {
        public string idActividadEconomica { get; set; }
        public string actividadEconomica { get; set; }
        public DateTime? fechaRegistro { get; set; }
    }

    public class ActividadEconomicaResponse
    {
        public bool estado { get; set; }
        public List<ActividadEconomica> lista { get; set; }
        public ActividadEconomica obj { get; set; }
        public string mensajes { get; set; }
    }
}