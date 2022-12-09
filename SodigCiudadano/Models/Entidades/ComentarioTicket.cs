using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SodigCiudadano.Models.Entidades
{
    public class ComentarioTicket
    {
        public int idComentarioDetalleTicket { get; set; }
        public int idTicket { get; set; }
        public int idEstado { get; set; }
        public string comentario { get; set; }
        public int idUsuario { get; set; }
        public string ip { get; set; }
        public System.DateTime fechaIngreso { get; set; }
        public string estadoTicket { get; set; }
        public string nombreUsuario { get; set; }
        public int idEstadoTicket { get; set; }
        public string EstadoTicketGeneral { get; set; }
    }

    public class ListaComentarioTicketResponse
    {
        public bool estado { get; set; }
        public List<ComentarioTicket> obj { get; set; }
        public string mensaje { get; set; }
    }

}