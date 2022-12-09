using Newtonsoft.Json;
using SodigCiudadano.Models.Dao;
using SodigCiudadano.Models.Entidades;
using SodigCiudadano.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

namespace SodigCiudadano.Controllers
{
    [SecurityFilter]
    public class HomeController : Controller
    {
        private TipoSolicitudDAO tipoSolicitudDao = new TipoSolicitudDAO();
        private TipoPersonaDAO tipoPersonaDao = new TipoPersonaDAO();
        private TipoContenedorDAO tipoContenedorDao = new TipoContenedorDAO();
        private TipoIdentificacionDAO tipoidentificacionDao = new TipoIdentificacionDAO();
        private TipoElectorDAO objDAO = new TipoElectorDAO();
        private LogDAO logDao = new LogDAO();
        private BCEDAO bceDao = new BCEDAO();

        // GET: Home
        public async Task<ActionResult> Index( int ts = 0, int tp = 0, bool msg=false , string msg1 = "" )
        {
            List<TipoElector> lista = new List<TipoElector>();
            try
            {
                if(Convert.ToBoolean(Session["tienePagoPendiente"].ToString()))
                {
                    Request.Flash("Warning", "Para continuar con los procesos es necesario realizar el pago pendiente");
                    return RedirectToAction("Index", "Pago");
                }
                ViewBag.mensaje = msg;
                ViewBag.codigoPasaporte = ConfigurationManager.AppSettings["codigoPasaporte"];
                if (!string.IsNullOrEmpty(msg1))
                    Request.Flash("danger", msg1);
                string token = Session["access_token"].ToString();


                ViewBag.idTipoSolicitud = new SelectList(new List<TipoSolicitud>()); 
                ViewBag.idTipoPersona = new SelectList(new List<TipoPersona>());
                ViewBag.idTipoContenedor = new SelectList(new List<TipoContenedor>(), "idTipoContenedor", "TipoContenedor");
                ViewBag.idTipoIdentificacion = new SelectList(new List<TipoIdentificacion>());
             
        
                    TipoElectorResponse objResponse = await objDAO.ObtenerListaTipoElector(token);
                    if (objResponse.estado)
                        lista = objResponse.obj;
                    else
                       Request.Flash("danger", objResponse.mensaje);
              

                TipoSolicitudResponse respuesta = await tipoSolicitudDao.ObtenerListaTipoSolicitud(token);
                if (respuesta.estado)
                {
                    ViewBag.idTipoSolicitud = new SelectList(respuesta.obj, "idTipoSolicitud", "TipoSolicitud", ts);
                    TipoPersonaResponse response = await tipoPersonaDao.ObtenerListaTipoPersona(token);
                    if(response.estado)
                        ViewBag.idTipoPersona = new SelectList(response.obj, "idTipoPersona", "TipoPersona", tp);
                    else
                        Request.Flash("danger", "Ha ocurrido un error al momento de recuperar los tipo de persona");

                }
                else
                {
                    Request.Flash("danger", "Ha ocurrido un error al momento de recuperar los tipo de solicitudes");
                }
                
                TipoContenedorResponse respuestaConte = await tipoContenedorDao.ObtenerListaTipoContenedor(token);
                if (respuestaConte.estado)
                    ViewBag.idTipoContenedor = new SelectList(respuestaConte.obj, "idTipoContenedor", "TipoContenedor");
                else
                    Request.Flash("danger", "Ha ocurrido un error al momento de recuperar los tipo de contenedores");


            }
            catch (Exception ex) 
            {
                ViewBag.idTipoSolicitud = new SelectList(new List<TipoSolicitud>());
                ViewBag.idTipoPersona = new SelectList(new List<TipoPersona>());
                ViewBag.idTipoContenedor = new SelectList(new List<TipoContenedor>(), "idTipoContenedor", "TipoContenedor");
                ViewBag.idTipoIdentificacion = new SelectList(new List<TipoIdentificacion>());
                Request.Flash("danger", ex.Message);
            }
                            
            return View(lista);
        }

        [HttpPost]
        public async Task<ActionResult> Index(FormCollection formulario)
        {
            int idTipoElector = 0;  
            int idTipoSolicitud = 0;
            int idTipoPersona = 0;
            bool msg = false;
            try
            {                
                string token = Session["access_token"].ToString();
                int idUsuario = Convert.ToInt32(Session["idUsuario"].ToString());
                idTipoSolicitud = Convert.ToInt32(formulario["idTipoSolicitud"].ToString());
                idTipoPersona = Convert.ToInt32(formulario["idTipoPersona"].ToString());
                int  idTipoContenedor = Convert.ToInt32(formulario["idTipoContenedor"].ToString());
                int idTipoIdentificacion = Convert.ToInt32(formulario["idTipoIdentificacion"].ToString());
                idTipoElector = Convert.ToInt32(formulario["idTipoElector"].ToString());

                string ti = idTipoIdentificacion.ToString();
                Session["idTipoIdentificacion"] = idTipoIdentificacion.ToString();

                if(idTipoSolicitud == Convert.ToInt32(ConfigurationManager.AppSettings["codigoRenovacionOlvidoClave"].ToString()))
                {
                    ApiResponse verificarEmision = await bceDao.VerificarEmisiones(token, Session["Identificacion"].ToString());
                    if(!verificarEmision.estado)
                    {
                        Request.Flash("danger", verificarEmision.mensaje);
                        return RedirectToAction("Index", "Home");
                    }
                        
                }
      
                if (idTipoPersona != Convert.ToInt32(ConfigurationManager.AppSettings["codigoPersonaJuridica"].ToString() ))
                {
                    Session["idTipoPersona"] = idTipoPersona.ToString();
                    Session["idTipoSolicitud"] = idTipoSolicitud.ToString();
                    Session["idTipoContenedor"] = idTipoContenedor.ToString();
    
                    string ip = Request.UserHostAddress;
                    LogTipoSolicitud obj = new LogTipoSolicitud { idTipoPersona = idTipoPersona, idTipoSolicitud = idTipoSolicitud ,  idUsuario = idUsuario, ip = ip };
                    LogResponse logRespopnse = await logDao.GuardarLogTipoSolicitud(obj, token);
                    if (logRespopnse.estado)
                    {
                        LogTipoContenedor objTipoContenedor = new LogTipoContenedor { idTipoContenedor = idTipoContenedor, idUsuario = idUsuario, ip = ip };
                        LogResponse logTipo = await logDao.GuardarLogTipoContenedor(objTipoContenedor, token);
                        if(logTipo.estado)
                        {
                            if (idTipoIdentificacion.ToString() == ConfigurationManager.AppSettings["codigoCertificado"].ToString())
                                Session["idTipoIdentificacion"] = ConfigurationManager.AppSettings["codigoCedula"].ToString();
                            else
                                Session["idTipoIdentificacion"] = idTipoIdentificacion.ToString();
                            LogTipoIdentificacion objIdentificacion = new LogTipoIdentificacion { idTipoIdentificacion = idTipoIdentificacion, idUsuario = idUsuario, ip = ip };
                            LogResponse logIdentificacion = await logDao.GuardarLogTipoIdentificacion(objIdentificacion, token);
                            if(logIdentificacion.estado)
                            {
                                LogTipoElector objElector = new LogTipoElector { idTipoElector = idTipoElector, idUsuario = idUsuario, ip = ip };
                                LogResponse logElector = await logDao.GuardarLogTipoElector(objElector, token);
                                if (logElector.estado)
                                {
                                    Session["idTipoElector"] = idTipoElector;
                                    return RedirectToAction("Index", "CapturaDocumento", new { ti = ti });
                                }
                                else Request.Flash("danger", logElector.mensaje);
                            }
                            else Request.Flash("danger", logIdentificacion.mensaje);

                        }                            
                        else Request.Flash("danger", logTipo.mensaje);
                    }                        
                    else
                        Request.Flash("danger", logRespopnse.mensaje);
                }
                else
                {
                    string ip = Request.UserHostAddress;
                    LogTipoSolicitud obj = new LogTipoSolicitud { idTipoPersona = idTipoPersona, idTipoSolicitud = idTipoSolicitud, idUsuario = idUsuario };
                    LogResponse logRespopnse = await logDao.GuardarLogTipoSolicitud(obj, token);                    
                    msg = true;
                    //Request.Flash("danger", "Para persona jurídica es necesario acercarse al registro civil");
                }
                   
            }
            catch (Exception ex)
            {
                Request.Flash("danger", ex.Message);
            }
            return RedirectToAction("Index", "Home", new { ts = idTipoSolicitud, tp = idTipoPersona, msg = msg });
        }

        public async Task<JsonResult> ObtenerTipoIdentificacionPorIdePersona(int idPersona)
        {
            bool estado = false;
            List<TipoIdentificacion> obj = new List<TipoIdentificacion>();
            try
            {
                string token = Session["access_token"].ToString();
                TipoIdentificacionResponse respuestai = await tipoidentificacionDao.ObtenerListaTipoidentificacionPorIdPersona(idPersona, token);
                if(respuestai.estado)
                {
                    obj = respuestai.obj;
                }
            }
            catch (Exception ex)
            {

            }
            return Json(new { estado = estado , data = obj}, JsonRequestBehavior.AllowGet);
        }
       
    }

   
    

}