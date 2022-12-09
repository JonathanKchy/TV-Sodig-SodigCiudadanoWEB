using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SodigCiudadano.Models.Entidades
{
    public class Informacion
    {
        public string Nombre { get; set; }
        public string Valor { get; set; }
    }

    public class InformacionResponse
    {
        public bool estado { get; set; }
        public List<Informacion> obj { get; set; }
        public string mensajes { get; set; }
        public bool verificacionVotante { get; set; }
    }

    public class InformacionPruebaVidaResponse
    {
        public bool estado { get; set; }
        public InformacionVidaResponse obj { get; set; }
        public string mensajes { get; set; }
    }

    public class InformacionVidaResponse
    {
        public List<InformacionResponseApiVerify> verificacion { get; set; }
        public bool resultadoVerificacion { get; set; }
        public string mensaje { get; set; }
    }

    public class InformacionResponseApiVerify
    {
        public double porcentaje { get; set; }
        public bool isIdentical { get; set; }
        public decimal confidence { get; set; }
        public ErrorApiVerifi error { get; set; }
    }

    public class ErrorApiVerifi
    {
        public string code { get; set; }
        public string message { get; set; }
    }

}