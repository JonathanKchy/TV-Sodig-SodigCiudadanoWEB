using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SodigCiudadano.Models.Entidades
{
    public class BCE
    {
    }

    public class IdentificacionSolicitanteEnvio
    {
        public string cedula { get; set; }
        public int tipoContenedor { get; set; }
        public int motivoSolicitud { get; set; }
        public string numeroSerieCertificado { get; set; }
        public string ipRegistro { get; set; }
        public int idUsuario { get; set; }
    }

    public class IdentificacionSolicitante
    {
        public int codigoRespuesta { get; set; }
        public string mensajeError { get; set; }
        public bool banderaIdentificacion { get; set; }
        public string mensajeIdentificacion { get; set; }
        public long codigoSolicitud { get; set; }
        public long codigoSolicitante { get; set; }
        public int numeroRenovaciones { get; set; }
        public string mailSolicitante { get; set; }
        public string celularSolicitante { get; set; }
        public bool estado { get; set; }
        public string nombre { get; set; }
        public bool banderaNombre { get; set; }
    }

    public class IdentificacionSolicitanteResponse
    {
        public bool estado { get; set; }
        public IdentificacionSolicitante obj { get; set; }
        public string mensaje { get; set; }
    }

    public class RegistroSolicitanteEnvio
    {
        public string apellidoPaterno { get; set; }
        public string apellidoMaterno { get; set; }
        public string cedula { get; set; }
        public string nombres { get; set; }
        public int tipoIdentificacion { get; set; }
        public string ipRegistro { get; set; }
        public int idUsuario { get; set; }
        public string nombre { get; set; }
        public bool banderaNombre { get; set; }
    }

    public class RegistroSolicitante
    {
        public bool estado { get; set; }
        public int codigoRespuesta { get; set; }
        public string mensajeError { get; set; }
        public int codigoSolicitante { get; set; }
        public bool banderaResultadoSolicitante { get; set; }
        public string mensajeResultadoSolicitante { get; set; }
    }

    public class RegistroSolicitanteResponse
    {
        public bool estado { get; set; }
        public RegistroSolicitante obj { get; set; }
        public string mensaje { get; set; }
    }

    public class RegistroSolicitudEnvio
    {
        public int codSolicitud { get; set; }
        public int tipoSolicitud { get; set; }
        public int estadoSolicitud { get; set; }
        public int motivoSolicitud { get; set; }
        public int tipoContenedorCertificado { get; set; }
        public List<int> usoCertificadoDigital { get; set; }
        public string txtOtroUso { get; set; }
        public string numeroSerieCertificado { get; set; }
        public string rucSolicitante { get; set; }
        public string rupSolicitante { get; set; }
        public int oficinaTerceroVinculado { get; set; }
        public int codigoSolicitante { get; set; }
        public string celularSolicitante { get; set; }
        public string ciudadSolicitante { get; set; }
        public string parroquiaSolicitante { get; set; }
        public string telefonoDomicilioSolicitante { get; set; }
        public string mailSolicitante { get; set; }
        public string mailAlternoSolicitante { get; set; }
        public string paisDomicilioSolicitante { get; set; }
        public string provinciaSolicitante { get; set; }
        public string estadoDomicilioSolicitante { get; set; }
        public string estadoOficinaSolicitante { get; set; }
        public string ciudadDomicilioSolicitante { get; set; }
        public string ciudadOficinaSolicitante { get; set; }
        public string direccionSolicitante { get; set; }
        public string actividadEconomica { get; set; }
        public string idActividadEconomica { get; set; }
        public string cargoSolicitante { get; set; }
        public int paisOficinaSolicitante { get; set; }
        public int provinciaOficinaSolicitante { get; set; }
        public string direccionOficinaSolitante { get; set; }
        public string telefonoOficinaSolicitante { get; set; }
        public string extensionOficinaSolicitante { get; set; }
        public bool estadoRecuperacion { get; set; }
        public int estadoPapeletaVotacion { get; set; }
        public int exepcionesPapeletaVotacion { get; set; }
        public double porcentajeCoincidencia { get; set; }
        public List<byte> archivoFoto { get; set; }
        public byte archivoFoto1 { get; set; }
        public string ipRegistro { get; set; }
        public string identificacion { get; set; }
        public int idUsuario { get; set; }
        public string archivoFotoBase64 { get; set; }
    }

    public class RegistroSolicitud
    {
        public bool estado { get; set; }
        public int codigoRespuesta { get; set; }
        public string mensajeError { get; set; }
        public int codigoSolicitud { get; set; }
        public bool banderaResultadoSolicitud { get; set; }
        public string mensajeResultadoSolicitante { get; set; }

    }

    public class RegistroSolicitudResponse
    {
        public bool estado { get; set; }
        public RegistroSolicitud obj { get; set; }
        public string mensaje { get; set; }
    }

    public class RegistroPagoEnvio
    {
        public int codigoSolicitud { get; set; }
        public string numeroFactura { get; set; }
        public DateTime fechaFactura { get; set; }
        public string numeroAsientoContable { get; set; }
        public decimal valorFactura { get; set; }
        public string observacion { get; set; }
        public string ipRegistro { get; set; }
        public string identificacion { get; set; }
        public int idUsuario { get; set; }
    }

    public class RegistroPago
    {
        public bool estado { get; set; }
        public int codigoRespuesta { get; set; }
        public string mensajeError { get; set; }
        public bool banderaPago { get; set; }
        public string mensajePago { get; set; }
        public int Serial { get; set; }
    }

    public class RegistroPagoResponse
    {
        public bool estado { get; set; }
        public RegistroPago obj { get; set; }
        public string mensaje { get; set; }
    }

    public class RegistroSolicitudRenovacion
    {
        public bool estado { get; set; }
        public int codigoRespuesta { get; set; }
        public string mensajeError { get; set; }
        public int codigoSolicitudRenovacion { get; set; }
        public bool banderaSolicitudRenovacion { get; set; }
        public string mensajeSolicitudRenovacion { get; set; }
    }

    public class RegistroSolicitudRenovacionResponse
    {
        public bool estado { get; set; }
        public RegistroSolicitudRenovacion obj { get; set; }
        public string mensaje { get; set; }
    }

    public class RegistroSolicitudRevocacionEnvio
    {
        public int codSolicitud { get; set; }
        public int motivoSolicitud { get; set; }
        public int motivoRevocacion { get; set; }
        public int idUsuario { get; set; }
        public string ipRegistro { get; set; }
        public string identificacion { get; set; }
    }

    public class RegistroSolicitudRevocacion
    {
        public bool estado { get; set; }
        public int codigoRespuesta { get; set; }
        public string mensajeError { get; set; }
        public bool banderaSolicitudRevocacion { get; set; }
        public string mensajeSolicitudRevocacion { get; set; }
        public int codigoSolicitudRevocacion { get; set; }
    }

    public class RegistroSolicitudRevocacionResponse
    {
        public bool estado { get; set; }
        public RegistroSolicitudRevocacion obj { get; set; }
        public string mensaje { get; set; }
    }

    public class RegistroOlvidoClaveEnvio
    {
        public int codigoSolicitud { get; set; }
        public string numeroSerieCertificado { get; set; }
        public string ipRegistro { get; set; }
        public string identificacion { get; set; }
        public int idUsuario { get; set; }
    }

    public class RegistroOlvidoClave
    {
        public bool estado { get; set; }
        public int codigoRespuesta { get; set; }
        public int numeroIntentosOlvidoClave { get; set; }
        public string mensajeError { get; set; }
        public int codigoSolicitudOlvidoClave { get; set; }
        public bool banderaSolicitudOlvidoClave { get; set; }
        public string mensajeSolicitudOlvidoClave { get; set; }
    }

    public class RegistroOlvidoClaveResponse
    {
        public bool estado { get; set; }
        public RegistroOlvidoClave obj { get; set; }
        public string mensaje { get; set; }
    }

}