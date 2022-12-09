using SodigCiudadano.Models.Dao;
using SodigCiudadano.Models.Entidades;
using SodigCiudadano.Util;
using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SodigCiudadano.Controllers
{
    [SecurityFilter]
    public class IdentificacionSolicitanteController : Controller
    {
        private TipoContenedorDAO tipoContenedorDao = new TipoContenedorDAO();
        private TipoSolicitudDAO tipoSolicitudDao = new TipoSolicitudDAO();
        private BCEDAO objBceDao = new BCEDAO();
        private AuthDAO authDao = new AuthDAO();
       

        // GET: IdentificacionSolicitante
        public async Task<ActionResult> Index()
        {
            IdentificacionSolicitanteEnvio obj = new IdentificacionSolicitanteEnvio();
            ViewBag.TipoSolicitud = "";
            ViewBag.TipoContenedor = "";
            ViewBag.Identificacion = Session["numeroIdentificacion"].ToString();
            ViewBag.mostrarCampoNumeroSerie = false;
            try
            {
                int idTipoContenedor = Convert.ToInt32(Session["idTipoContenedor"].ToString());
                int idTipoSolicitud = Convert.ToInt32(Session["idTipoSolicitud"].ToString());
                if (idTipoSolicitud == Convert.ToInt32(ConfigurationManager.AppSettings["codigoRenovacionOlvidoClave"].ToString()))
                    idTipoSolicitud = Convert.ToInt32(ConfigurationManager.AppSettings["codigoRevocacion"].ToString());
                string token = Session["access_token"].ToString();
                int idUsuario = Convert.ToInt32(Session["idUsuario"].ToString());
                string identificacion = Session["Identificacion"].ToString();
                NumeroIntentosResponse intentos = await authDao.ObtenerNumeroIntentosPorIdUsuario(idUsuario, identificacion, token);
                if (intentos.estado)
                {
                    if (intentos.obj.numeroIntentos >= Convert.ToInt32(ConfigurationManager.AppSettings["numeroIntentosPermitidos"].ToString()))
                    {
                        Request.Flash("danger", "Ha superado el número máximo de intentos. Por favor, agende una cita");
                        return RedirectToAction("Index", "Ticket");
                    }
                }
                else
                    Request.Flash("danger", "Ha ocurrido un error al momento de recuperar el número de intentos " + intentos.mensajes);

                NombreContenedorResponse respuesta = await tipoContenedorDao.ObtenerTipoContenedor(idTipoContenedor, token);
                if (respuesta.estado)
                {
                    NombreSolicitudResponse respuestaSolicitud = await tipoSolicitudDao.ObtenerTipoSolicitudPorId(idTipoSolicitud, token);
                    if (respuestaSolicitud.estado)
                    {
                        int codigoRenovacion = Convert.ToInt32(ConfigurationManager.AppSettings["codigoRenovacion"].ToString());
                        int codigoRevocacion = Convert.ToInt32(ConfigurationManager.AppSettings["codigoRevocacion"].ToString());
                        int codigoRenovacionOlvidoClave = Convert.ToInt32(ConfigurationManager.AppSettings["codigoRenovacionOlvidoClave"].ToString());
                        if (respuestaSolicitud.obj.idTipoSolicitud == codigoRenovacion || respuestaSolicitud.obj.idTipoSolicitud == codigoRevocacion || respuestaSolicitud.obj.idTipoSolicitud == codigoRenovacionOlvidoClave)
                            ViewBag.mostrarCampoNumeroSerie = true;
                        ViewBag.TipoSolicitud = respuestaSolicitud.obj.tipoSolicitud;
                        ViewBag.TipoContenedor = respuesta.obj.tipoContenedor;
                        obj.tipoContenedor = respuesta.obj.idTipoContenedor;
                        obj.motivoSolicitud = respuestaSolicitud.obj.idTipoSolicitud;
                        obj.cedula = Session["numeroIdentificacion"].ToString();
                    }
                    else
                        Request.Flash("danger", "Ha ocurrido un error al momento de recuperar el tipo de servicio");
                }
                else
                    Request.Flash("danger", "Ha ocurrido un error al momento de recuperar el tipo de contenedor");
            }
            catch (Exception ex)
            {
                Request.Flash("danger", ex.Message);
            }
            return View(obj);
        }

        [HttpPost]
        public async Task<ActionResult> Index(IdentificacionSolicitanteEnvio obj)
        {
            try
            {
                string token = Session["access_token"].ToString();
                obj.ipRegistro = Request.UserHostAddress;
                obj.idUsuario = Convert.ToInt32(Session["idUsuario"].ToString());
                IdentificacionSolicitanteResponse objResponse = await objBceDao.IdentificacionSolicitante(obj, token);
                if (objResponse.estado)
                {
                    if (objResponse.obj.banderaIdentificacion)
                    {
                        Session["nombreComparacion"] = objResponse.obj.nombre;
                        Session["banderaNombre"] = objResponse.obj.banderaNombre;
                        int idTipoSolicitud = Convert.ToInt32(Session["idTipoSolicitud"].ToString());
                        if (idTipoSolicitud != Convert.ToInt32(ConfigurationManager.AppSettings["codigoEmision"].ToString()))
                        {
                            if (obj.numeroSerieCertificado == "010203040506")
                                Session["correoBCE"] = Session["correoCliente"].ToString();
                            else
                                Session["correoBCE"] = objResponse.obj.mailSolicitante;
                            Session["codigoSolicitanteBCE"] = objResponse.obj.codigoSolicitante;
                            Session["codigoSolicitudBCE"] = objResponse.obj.codigoSolicitud;
                            Session["numeroSerieSolicitud"] = obj.numeroSerieCertificado;
                            return RedirectToAction("VerificacionCodigo", "OCR");
                        }
                        else
                            return RedirectToAction("Index", "PruebaVida");
                    }
                    else
                    {
                        Usuario objUsr = new Usuario { idUsuario = Convert.ToInt32(Session["idUsuario"].ToString()), Identificacion = Session["Identificacion"].ToString() };                        
                        ApiResponse actualizarIntentos = await authDao.ActualizarNumeroIntentos(objUsr, token);
                        if (objResponse.obj.codigoRespuesta == 5050)
                            Request.Flash("danger", "Estimado Usuario, actualmente el número de identificación ya posee un certificado registrado.");
                        else
                            Request.Flash("danger", objResponse.obj.mensajeIdentificacion + " " + objResponse.obj.mensajeError);
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                    Request.Flash("danger", objResponse.mensaje);
            }
            catch (Exception ex)
            {
                Request.Flash("danger", ex.Message);
            }
            return RedirectToAction("Index", "IdentificacionSolicitante");
        }
    }
}

