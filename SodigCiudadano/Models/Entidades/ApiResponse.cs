using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SodigCiudadano.Models.Entidades
{
    public class ApiResponse
    {
        public bool estado { get; set; }
        public dynamic obj { get; set; }
        public string mensaje { get; set; }
    }
}