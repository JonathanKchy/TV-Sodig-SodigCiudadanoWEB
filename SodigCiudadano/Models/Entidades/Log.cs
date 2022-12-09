namespace SodigCiudadano.Models.Entidades
{   
    public class Log
    {
    }

    public class LogTipoSolicitud
    {
        public int idTipoSolicitud { get; set; }
        public int idTipoPersona { get; set; }
        public int idUsuario { get; set; }
        public string ip { get; set; }
    }

    public class LogTipoContenedor
    {
        public int idTipoContenedor { get; set; }
        public int idUsuario { get; set; }
        public string ip { get; set; }
    }

    public class LogTipoIdentificacion
    {
        public int idTipoIdentificacion { get; set; }
        public int idUsuario { get; set; }
        public string ip { get; set; }
    }

    public class LogTipoElector
    {
        public int idTipoElector { get; set; }
        public int idUsuario { get; set; }
        public string ip { get; set; }
    }

    public class LogResponse
    {
        public bool estado { get; set; }
        public string mensaje { get; set; }
    }

    public class LogPagoCheckOut
    {
        public string identificacion { get; set; }
        public string response { get; set; }
    }
}