using System;
using System.Collections.Generic;

namespace SodigCiudadano.Models.Entidades
{
    public class CatalogoTicket
    {
        public string idTipoTicket { get; set; }
        public string tipoTicket { get; set; }
        public Nullable<System.DateTime> fechaRegistro { get; set; }
    }

    public class CatalogoTicketResponse
    {
        public bool estado { get; set; }
        public List<CatalogoTicket> obj { get; set; }
        public string mensaje { get; set; }
    }

    public class NumeroTransaccionResponse
    {
        public bool estado { get; set; }
        public int numeroTransaccion { get; set; }
        public string mensaje { get; set; }
    }
}