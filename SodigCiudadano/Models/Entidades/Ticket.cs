using System.Collections.Generic;

namespace SodigCiudadano.Models.Entidades
{
    public class Ticket
    {
        public int idTicket { get; set; }
        public int idTipo { get; set; }
        public int idEstadoTicket { get; set; }
        public System.DateTime fechaIngreso { get; set; }
        public string nombre { get; set; }
        public string identificacion { get; set; }
        public string telefono { get; set; }
        public string mail { get; set; }
        public string detalle { get; set; }
        public string urlArchivo { get; set; }
        public string tipoTicket { get; set; }
        public string EstadoTicket { get; set; }
        public string ipRegistro { get; set; }
        public int idUsuario { get; set; }
    }

    public class TicketResponse
    {
        public bool estado { get; set; }
        public Ticket obj { get; set; }
        public string mensaje { get; set; }
    }

}