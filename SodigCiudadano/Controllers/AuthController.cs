using SodigCiudadano.Models.Dao;
using SodigCiudadano.Models.Entidades;
using SodigCiudadano.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SodigCiudadano.Controllers
{
    public class AuthController : Controller
    {
        private AuthDAO objDao = new AuthDAO();
        private DatosDAO objDatosDAO = new DatosDAO();
        private CatalogoDAO objCatalogoDao = new CatalogoDAO();
        private TicketDAO objTickeDao = new TicketDAO();
        private AuthDAO authDao = new AuthDAO();
        // Auth
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(FormCollection formulario)
        {
            Captcha objCaptcha;
            try
            {
                Session["tienePagoPendiente"] = false;
                string identificacion = formulario["identificacion"].ToString();
                string correo = formulario["correo"].ToString();
                bool aceptaTerminos = (formulario["checkTerminos"] ?? "").Equals("on", StringComparison.CurrentCultureIgnoreCase);
                //    var responseCaptcha = formulario["g-recaptcha-response"].ToString();    
                bool esNumero = true;

                try
                {
                    int numero = Convert.ToInt32(identificacion);
                }
                catch (Exception)
                {
                    esNumero = false;
                }

                if (esNumero && identificacion.Length == 10)
                {
                    if (!Validacion.verificarIdentificacion(identificacion))
                    {
                        Request.Flash("warning", "Ingrese una identificación válida");
                        return View();
                    }
                }

                //if (Validacion.verificarIdentificacion(identificacion))
                //{

                /* objCaptcha = objDao.VerifcarCaptcha(responseCaptcha);
                     if (objCaptcha.Success)
                     {*/
                string ip = Request.UserHostAddress;
                ApiResponse respuesta = await objDao.IniciarSesion(identificacion, ip, correo, aceptaTerminos);
                if (respuesta.estado)
                {

                    Session["idUsuario"] = (string)respuesta.obj.idUsuario;
                    Session["Identificacion"] = (string)respuesta.obj.Identificacion;
                    Session["correoCliente"] = (string)respuesta.obj.correo;
                    Session["access_token"] = (string)respuesta.obj.access_token;


                    NumeroIntentosResponse intentos = await authDao.ObtenerNumeroIntentosPorIdUsuario(Convert.ToInt32(Session["idUsuario"].ToString()), identificacion, Session["access_token"].ToString());

                    if (intentos.obj.numeroIntentos >= Convert.ToInt32(ConfigurationManager.AppSettings["numeroIntentosPermitidos"].ToString()))
                    {
                        Request.Flash("danger", "Ha superado el número máximo de intentos permitidos. Por favor, realice el proceso nuevamente en unos momentos");
                        return RedirectToAction("Login", "Auth");
                    }

                    DatosResponse objResponse = await objDatosDAO.buscarSolicitudPorIdentificacion(identificacion, Session["access_token"].ToString());
                    if (objResponse.estado)
                    {
                        if (objResponse.obj.Cliente == null || objResponse.obj.Solicitud == null || string.IsNullOrEmpty(objResponse.obj.Cliente.Telefonos[2].Contacto))
                        {
                            SolicitudAprobadaResponse solicitudAprobadaResponse = await objDatosDAO.buscarSolicitudesAprobadasPorIdentificacion(identificacion, Session["access_token"].ToString());
                            if (solicitudAprobadaResponse.estado)
                            {
                                if (solicitudAprobadaResponse.lista.Count > 0)
                                {
                                    CodigoOTP obj = new CodigoOTP();
                                    obj.idUsuario = Convert.ToInt32((string)respuesta.obj.idUsuario);
                                    obj.correoUsuario = solicitudAprobadaResponse.lista[0].correo;
                                    obj.identificacion = (string)respuesta.obj.Identificacion;
                                    Session["correoUsuarioTemp"] = obj.correoUsuario;
                                    Session["IdentificacionTemp"] = (string)respuesta.obj.Identificacion;
                                    Session.Contents.Remove("Identificacion");
                                    CodigoOTPResponse objCodigoResponse = await objDao.crearCodigoOtp(obj, (string)respuesta.obj.access_token);
                                    if (objCodigoResponse.estado)
                                        return RedirectToAction("verificacionCodigo", "Auth", new { identificador = "1" });
                                    else
                                        Request.Flash("danger", objCodigoResponse.mensaje);
                                }
                                else
                                {
                                    CodigoOTP obj = new CodigoOTP();
                                    obj.idUsuario = Convert.ToInt32((string)respuesta.obj.idUsuario);
                                    obj.correoUsuario = correo;
                                    obj.identificacion = (string)respuesta.obj.Identificacion;
                                    Session["correoUsuarioTemp"] = correo;
                                    Session["IdentificacionTemp"] = (string)respuesta.obj.Identificacion;
                                    Session.Contents.Remove("Identificacion");
                                    CodigoOTPResponse objCodigoResponse = await objDao.crearCodigoOtp(obj, (string)respuesta.obj.access_token);
                                    if (objCodigoResponse.estado)
                                        return RedirectToAction("verificacionCodigo", "Auth", new { identificador = "3" });
                                    else
                                        Request.Flash("danger", objCodigoResponse.mensaje);
                                }
                            }
                            else
                                Request.Flash("danger", solicitudAprobadaResponse.mensajes);
                        }
                        else
                        {
                            CodigoOTP obj = new CodigoOTP();
                            obj.idUsuario = Convert.ToInt32((string)respuesta.obj.idUsuario);
                            obj.correoUsuario = objResponse.obj.Cliente.Telefonos[2].Contacto.ToString();
                            obj.identificacion = (string)respuesta.obj.Identificacion;
                            Session.Contents.Remove("Identificacion");
                            CodigoOTPResponse objCodigoResponse = await objDao.crearCodigoOtp(obj, (string)respuesta.obj.access_token);
                            if (objCodigoResponse.estado)
                            {
                                Session["idTipoSolicitud"] = objResponse.obj.Solicitud.IdTipoSolicitud;
                                Session["idTipoContenedor"] = objResponse.obj.Solicitud.IdTipoContenedor;
                                Session["correoUsuarioTemp"] = objResponse.obj.Cliente.Telefonos[2].Contacto;
                                Session["IdentificacionTemp"] = (string)respuesta.obj.Identificacion;
                                Session["numeroIdentificacion"] = (string)respuesta.obj.Identificacion;
                                Session["tienePagoPendiente"] = true;
                                return RedirectToAction("verificacionCodigo", "Auth", new { identificador = "2" });
                            }
                            else
                                Request.Flash("danger", objCodigoResponse.mensaje);
                        }
                    }
                    else Request.Flash("danger", objResponse.mensajes);
                }
                else
                    Request.Flash("danger", respuesta.mensaje);
                //  }
                //  else
                // Request.Flash("danger", "Ha ocurrido un error en el captcha. Por Favor, intente de nuevo");
                /* }
                  else
                      Request.Flash("warning", "Por favor, ingrese una identificación válida");*/

            }
            catch (Exception ex)
            {
                Request.Flash("danger", ex.Message);
            }

            return View();
        }

        public ActionResult verificacionCodigo(string identificador)
        {
            ViewBag.identificador = identificador;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> verificacionCodigo(string codigoOtp, string identificador)
        {
           
            try
            {
                if (string.IsNullOrEmpty(Session["correoUsuarioTemp"].ToString()))
                    return RedirectToAction("Login", "Auth");
                   CodigoOTP obj = new CodigoOTP();
                obj.correoUsuario = Session["correoUsuarioTemp"].ToString();
                obj.codigoOtpIngresado = codigoOtp;
                obj.identificacion = Session["IdentificacionTemp"].ToString();
                obj.idUsuario = Convert.ToInt32(Session["idUsuario"].ToString());
                obj.ip = Request.UserHostAddress;
                string token = Session["access_token"].ToString();
                CodigoOTPResponse objResponse = await objDao.verificarCodigoOtp(obj, token);
                if (objResponse.estado)
                {
                    Session["Identificacion"] = obj.identificacion;
                    Session.Contents.Remove("correoUsuarioTemp");
                    Session.Contents.Remove("IdentificacionTemp");
                    if (identificador.Trim() == "1")
                      //  return RedirectToAction("SolicitudesAprobadas", "Solicitud");
                        return RedirectToAction("Index", "Home");
                    else if (identificador.Trim() == "2")
                        return RedirectToAction("Index", "Pago");
                    else
                        return RedirectToAction("Index", "Home");
                }
                else
                    Request.Flash("danger", objResponse.mensaje);
            }
            catch (Exception ex)
            {
                Request.Flash("danger", ex.Message);
            }
            ViewBag.identificador = identificador;
            return View();
        }

        public async Task<ActionResult> ReenviarCodigo()
        {
            try
            {
                string identificacion = Session["IdentificacionTemp"].ToString();
                DatosResponse objResponse = await objDatosDAO.buscarSolicitudPorIdentificacion(identificacion, Session["access_token"].ToString());
                if (objResponse.estado)
                {
                    if (objResponse.obj.Cliente == null || objResponse.obj.Solicitud == null || string.IsNullOrEmpty(objResponse.obj.Cliente.Telefonos[2].Contacto))
                    {
                        SolicitudAprobadaResponse solicitudAprobadaResponse = await objDatosDAO.buscarSolicitudesAprobadasPorIdentificacion(identificacion, Session["access_token"].ToString());
                        if (solicitudAprobadaResponse.estado)
                        {
                            if (solicitudAprobadaResponse.lista.Count > 0)
                            {
                                CodigoOTP obj = new CodigoOTP();
                                obj.idUsuario = Convert.ToInt32(Session["idUsuario"].ToString());
                                obj.correoUsuario = solicitudAprobadaResponse.lista[0].correo;
                                obj.identificacion = identificacion;
                                Session["correoUsuarioTemp"] = obj.correoUsuario;
                                CodigoOTPResponse objCodigoResponse = await objDao.crearCodigoOtp(obj, Session["access_token"].ToString());
                                if (objCodigoResponse.estado)
                                {
                                    Request.Flash("success", "Código Reenviado Correctamente");
                                    return RedirectToAction("verificacionCodigo", "Auth", new { identificador = "1" });
                                }

                                else
                                    Request.Flash("danger", objCodigoResponse.mensaje);
                            }
                            else
                            {
                                CodigoOTP obj = new CodigoOTP();
                                obj.idUsuario = Convert.ToInt32(Session["idUsuario"].ToString());
                                obj.correoUsuario = Session["correoUsuarioTemp"].ToString();
                                obj.identificacion = identificacion;
                                Session["correoUsuarioTemp"] = obj.correoUsuario;
                                Session["IdentificacionTemp"] = identificacion;
                                Session.Contents.Remove("Identificacion");
                                CodigoOTPResponse objCodigoResponse = await objDao.crearCodigoOtp(obj, Session["access_token"].ToString());
                                if (objCodigoResponse.estado)
                                {
                                    Request.Flash("success", "Código Reenviado Correctamente");
                                    return RedirectToAction("verificacionCodigo", "Auth", new { identificador = "3" });
                                }
                                else
                                    Request.Flash("danger", objCodigoResponse.mensaje);
                            }
                        }
                        else
                            Request.Flash("danger", solicitudAprobadaResponse.mensajes);
                    }
                    else
                    {
                        CodigoOTP obj = new CodigoOTP();
                        obj.idUsuario = Convert.ToInt32(Session["idUsuario"].ToString());
                        obj.correoUsuario = objResponse.obj.Cliente.Telefonos[2].Contacto.ToString();
                        obj.identificacion = identificacion;
                        Session.Contents.Remove("Identificacion");
                        CodigoOTPResponse objCodigoResponse = await objDao.crearCodigoOtp(obj, Session["access_token"].ToString());
                        if (objCodigoResponse.estado)
                        {
                            Session["idTipoSolicitud"] = objResponse.obj.Solicitud.IdTipoSolicitud;
                            Session["idTipoContenedor"] = objResponse.obj.Solicitud.IdTipoContenedor;
                            Session["correoUsuarioTemp"] = objResponse.obj.Cliente.Telefonos[2].Contacto;
                            Session["numeroIdentificacion"] = Session["IdentificacionTemp"].ToString();
                            Request.Flash("success", "Código Reenviado Correctamente");
                            return RedirectToAction("verificacionCodigo", "Auth", new { identificador = "2" });
                        }
                        else
                            Request.Flash("danger", objCodigoResponse.mensaje);
                    }
                }
                else Request.Flash("danger", objResponse.mensajes);
            }
            catch (Exception ex)
            {
                Request.Flash("danger", ex.Message);
            }
            return RedirectToAction("Login", "Auth");
        }

        public async Task<ActionResult> TicketCorreo()
        {
            Ticket obj = new Ticket();
            obj.detalle = "No recibí el código";
            ViewBag.idTipo = new SelectList(new List<CatalogoTicket>());
            try
            {
                string token = Session["access_token"].ToString();
                CatalogoTicketResponse objResponse = await objCatalogoDao.ObtenerListaTipoTicket(token);
                if (objResponse.estado)
                    ViewBag.idTipo = new SelectList(objResponse.obj, "idTipoTicket", "tipoTicket");
                else
                    Request.Flash("danger", objResponse.mensaje);
            }
            catch (Exception ex)
            {
                Request.Flash("danger", ex.Message);
            }
            return View(obj);
        }

        [HttpPost]
        public async Task<ActionResult> TicketCorreo(Ticket objTicket, HttpPostedFileBase postedFile)
        {
            try
            {
                string token = Session["access_token"].ToString();
                objTicket.ipRegistro = Request.UserHostAddress;
                objTicket.idEstadoTicket = 1;
                byte[] imagen = null;
                if (postedFile != null)
                {
                    if (postedFile.ContentLength > 3000000)
                    {
                        Request.Flash("danger", "Archivo Demasiado Pesado. Por favor, seleccione un archivo menor a 3MB");
                        return RedirectToAction("Index", "Ticket");
                    }

                    string contenType = postedFile.ContentType;
                    string extension = Path.GetExtension(postedFile.FileName);
                    using (Stream inputStream = postedFile.InputStream)
                    {
                        MemoryStream memoryStream = inputStream as MemoryStream;
                        if (memoryStream == null)
                        {
                            memoryStream = new MemoryStream();
                            inputStream.CopyTo(memoryStream);
                        }
                        imagen = memoryStream.ToArray();
                    }
                }
                TicketResponse objResponse = await objTickeDao.InsertarTicket(objTicket, token, imagen);
                if (objResponse.estado)
                    Request.Flash("success", "Ticket creado correctamente\n En unos momentos nos comunicaremos contigo.");
                else
                    Request.Flash("danger", objResponse.mensaje);
            }
            catch (Exception ex)
            {
                Request.Flash("danger", ex.Message);
            }
            return RedirectToAction("Login", "Auth");
        }

        public ActionResult Logout()
        {
            Session.Contents.Remove("idUsuario");
            Session.Contents.Remove("Identificacion");
            Session.Contents.Remove("numeroIdentificacion");
            Session.Contents.Remove("access_token");
            Session.Contents.Remove("idTipoContenedor");
            Session.Contents.Remove("idTipoPersona");
            Session.Contents.Remove("idTipoSolicitud");
            Session.Contents.Remove("idTipoIdentificacion");
            Session.Contents.Remove("videoPruebaVida");
            Session.Contents.Remove("imagenPruebaVida");
            Session.Contents.Remove("esEcuatoriano");
            Session.Contents.Remove("idTipoElector");
            Session.Contents.Remove("primerApellido");
            Session.Contents.Remove("segundoApellido");
            Session.Contents.Remove("primerNombre");
            Session.Contents.Remove("segundoNombre");
            Session.Contents.Remove("idSolicitud");
            Session.Contents.Remove("cuadroVideo");
            Session.Contents.Remove("imagenDocumento");
            Session.Contents.Remove("imagenPruebaVida");
            Session.Contents.Remove("tipoIdentificacion");
            Session.Contents.Remove("correoCliente");
            Session.Contents.Remove("direccion");
            Session.Contents.Remove("sector");
            return RedirectToAction("Login", "Auth");
        }

    }
}