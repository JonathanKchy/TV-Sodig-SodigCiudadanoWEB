using System;
using System.Collections.Generic;

namespace SodigCiudadano.Models.Entidades
{
    public class Datos
    {
        public Cliente Cliente { get; set; }
        public Solicitud Solicitud { get; set; }
    }

    public class DatosResponse
    {
        public bool estado { get; set; }
        public Datos obj { get; set; }
        public string mensajes { get; set; }
    }

    public class SolicitudAprobadaResponse
    {
        public bool estado { get; set; }
        public List<SolicitudAprobada> lista { get; set; }
        public string mensajes { get; set; }
    }

    public class Cliente
    {
        public string Identificacion { get; set; }
        public int IdTipoIdentificacion { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public string PrimerNombre { get; set; }
        public string SegundoNombre { get; set; }
        public bool Elector { get; set; }
        public int IdTipoElector { get; set; }
        public Actividad Actividad { get; set; }
        public List<Telefono> Telefonos { get; set; }
        public List<Direccion> Direcciones { get; set; }
    }

    public class Solicitud
    {
        public int IdSolicitud { get; set; }
        public int IdTipoSolicitud { get; set; }
        public string Identificacion { get; set; }
        public int IdTipoContenedor { get; set; }
        public int IdMotivoSolicitud { get; set; }
        public int IdEstado { get; set; }
        public string CodigoSolicitante { get; set; }
        public string IdUsoCertificado { get; set; }
        public string otroUso { get; set; }
        public string codigoSolicitud { get; set; }
        public DateTime? Fecha { get; set; }
        public string Ip { get; set; }
        public string identificacionFactura { get; set; }
        public string nombreFactura { get; set; }
    }

    public class Actividad
    {
        public int IdActividad { get; set; }
        public string IdActividadEconomica { get; set; }
        public string Ruc { get; set; }
        public string Rup { get; set; }
    }

    public class Telefono
    {
        public int IdContacto { get; set; }
        public int IdTipoContacto { get; set; }
        public int IdCategoria { get; set; }
        public string Contacto { get; set; }
        public string Extension { get; set; }
        public int IdSolicitud { get; set; }
    }

    public class Direccion
    {
        public int IdDireccion { get; set; }
        public int IdCategoria { get; set; }
        public string IdCiudad { get; set; }
        public string IdPais { get; set; }
        public string IdProvincia { get; set; }
        public int IdSolicitud { get; set; }
        public string direccion { get; set; }
        public string Sector { get; set; }
        public string codigoPostal { get; set; }
        public DateTime? FechaRegistro { get; set; }
    }

    public class SolicitudAprobada
    {
        public int IdSolicitud { get; set; }
        public int IdTipoSolicitud { get; set; }
        public string Identificacion { get; set; }
        public int IdTipoContenedor { get; set; }
        public int IdMotivoSolicitud { get; set; }
        public int IdEstado { get; set; }
        public string CodigoSolicitante { get; set; }
        public string IdUsoCertificado { get; set; }
        public DateTime? Fecha { get; set; }
        public string Ip { get; set; }
        public string TipoSolicitud { get; set; }
        public string TipoContenedor { get; set; }
        public string motivoSolicitud { get; set; }
        public string estado { get; set; }
        public string usoCertificado { get; set; }
        public string correo { get; set; }
    }
}