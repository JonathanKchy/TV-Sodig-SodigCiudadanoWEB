using Emgu.CV;
using Emgu.CV.Structure;
using Newtonsoft.Json;
using SodigCiudadano.Models.Dao;
using SodigCiudadano.Models.Entidades;
using SodigCiudadano.Util;
using System;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SodigCiudadano.Controllers
{
    [SecurityFilter]
    public class PruebaVidaController : Controller
    {
        private CascadeClassifier cascadeClassifier;
        private PruebaVidaDAO pruebaVidaDAO = new PruebaVidaDAO();
        private DialogoDAO dialogoDAO = new DialogoDAO();
        private InformacionDAO informacionDAO = new InformacionDAO();
        private AuthDAO authDao = new AuthDAO();
        private InformacionDAO objInformacionDAO = new InformacionDAO();

        // GET: PruebaVida
        public async Task<ActionResult> Index()
        {
            DialogoResponse obj = new DialogoResponse();

            try
            {
                string token = Session["access_token"].ToString();
                int idUsuario = Convert.ToInt32(Session["idUsuario"].ToString());
                string identificacion = Session["Identificacion"].ToString();
                //  NumeroIntentosResponse intentos = await authDao.ObtenerNumeroIntentosPorIdUsuario(idUsuario, identificacion, token);
                ViewBag.primerNombre = "";
                ViewBag.segndoNombre = "";
                ViewBag.primerApellido = "";
                ViewBag.segundoApellido = "";
                ViewBag.identificacion = identificacion;
                //if (intentos.estado)
                //{
                //if (intentos.obj.numeroIntentos >= Convert.ToInt32(ConfigurationManager.AppSettings["numeroIntentosPermitidos"].ToString()))
                //{
                //    Request.Flash("danger", "Ha superado el número máximo de intentos. Por favor, agende una cita");
                //    return RedirectToAction("Index", "Ticket");
                //}
                //else
                //{
                obj = await dialogoDAO.ObtenerDialogo(token);
                if (!obj.estado)
                    Request.Flash("danger", obj.mensaje);
                else
                {
                    //InformacionResponse responseDatosDemograficos = await objInformacionDAO.ObtenerInformacionDatosDemograficos(identificacion, token);
                    InformacionResponse responseDatosDemograficos = (InformacionResponse)Session["responseDatosDemograficos"];
                    if (responseDatosDemograficos.estado)
                    {
                        if (responseDatosDemograficos.obj.Count > 0)
                        {
                            foreach (var item in responseDatosDemograficos.obj)
                            {
                                if (item.Nombre.Trim() == "domicilio")
                                {
                                    string[] domicilio = item.Valor.Split('/');
                                    if (domicilio.Length > 2)
                                    {
                                        Session["direccion"] = domicilio[0];
                                        Session["sector"] = domicilio[1];
                                    }
                                    else
                                    {
                                        Session["direccion"] = "";
                                        Session["sector"] = "";
                                    }
                                }

                                if (item.Nombre.Trim() == "nombre".Trim())
                                {
                                    Session["nombreDinardap"] = item.Valor;
                                    string nombre = item.Valor;
                                    NombreApellido objNombres = new NombreApellido();
                                    var nombres = objNombres.GetNombreApellido(nombre);
                                    Session["primerApellido"] = "...";
                                    Session["segundoApellido"] = "...";
                                    Session["primerNombre"] = "...";

                                    int numeroNombre = 0;
                                    Session["segundoNombre"] = "";

                                    if (!string.IsNullOrEmpty(nombres.primerApellido))
                                    {
                                        Session["primerApellido"] = nombres.primerApellido;
                                        numeroNombre += 1;
                                    }


                                    if (!string.IsNullOrEmpty(nombres.segundoApellido))
                                    {
                                        Session["segundoApellido"] = nombres.segundoApellido;
                                        numeroNombre += 1;
                                    }


                                    if (!string.IsNullOrEmpty(nombres.primerNombre))
                                    {
                                        Session["primerNombre"] = nombres.primerNombre;
                                        numeroNombre += 1;
                                    }



                                    if (!string.IsNullOrEmpty(nombres.segundoNombre))
                                    {
                                        Session["segundoNombre"] = nombres.segundoNombre;
                                        numeroNombre += 1;
                                    }
                                    Session["numeroNombres"] = numeroNombre;

                                    ViewBag.primerNombre = nombres.primerNombre;
                                    ViewBag.segndoNombre = nombres.segundoNombre;
                                    ViewBag.primerApellido = nombres.primerApellido; ;
                                    ViewBag.segundoApellido = nombres.segundoApellido;
                                }
                            }
                        }
                        else Request.Flash("danger", "Ha existido un error. Por favor, inténtelo más tarde. No ha sido posible obtener los datos demográficos del CNE.");
                    }
                    else Request.Flash("danger", responseDatosDemograficos.mensajes);

                }
                //}
                //}
                //else
                //    Request.Flash("danger", "Ha ocurrido un error al momento de recuperar el número de intentos " + intentos.mensajes);

            }
            catch (Exception ex)
            {
                Request.Flash("danger", ex.Message);
            }
            return View(obj);
        }

        [HttpPost]
        public JsonResult SubirVideo(HttpPostedFileBase video, string frame)
        {
            bool estado = false;
            string mensaje = "OK";
            try
            {
                if (video == null)
                    mensaje = "Por favor, cargar un video";
                else
                {
                    Session["cuadroVideo"] = frame;
                    PruebaVida obj = new PruebaVida();
                    obj.video = video;
                    byte[] archivo;
                    using (Stream inputStream = video.InputStream)
                    {
                        MemoryStream memoryStream = inputStream as MemoryStream;
                        if (memoryStream == null)
                        {
                            memoryStream = new MemoryStream();
                            inputStream.CopyTo(memoryStream);
                        }
                        archivo = memoryStream.ToArray();
                    }
                    obj.imagen = archivo;
                    Session["videoPruebaVida"] = obj;
                    estado = true;
                }
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new { estado = estado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> SubirImagen()
        {
            try
            {
                string token = Session["access_token"].ToString();
                int idUsuario = Convert.ToInt32(Session["idUsuario"].ToString());
                string identificacion = Session["Identificacion"].ToString();
                //NumeroIntentosResponse intentos = await authDao.ObtenerNumeroIntentosPorIdUsuario(idUsuario, identificacion, token);
                //if (intentos.estado)
                //{
                //    if (intentos.obj.numeroIntentos >= Convert.ToInt32(ConfigurationManager.AppSettings["numeroIntentosPermitidos"].ToString()))
                //    {
                //        Request.Flash("danger", "Ha superado tres (3) intentos fallidos, para poder procesar su solicitud. Por favor, sacar una cita a través de nuestro chat ubicado en la parte inferior de nuestra pantalla");
                //        return RedirectToAction("Index", "Home");
                //    }
                //    else
                //        return View();
                //}
                //else
                //    Request.Flash("danger", "Ha ocurrido un error al momento de recuperar el número de intentos " + intentos.mensajes);
            }
            catch (Exception ex)
            {
                Request.Flash("danger", ex.ToString());
            }
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> SubirImagen(string img)
        {
            bool estado = false;
            string mensaje = "OK";
            bool esEcuatoriano = false;
            bool resultadoVerificacion = false;
            try
            {
                esEcuatoriano = Convert.ToBoolean(Session["esEcuatoriano"]);
                string token = Session["access_token"].ToString();
                if (img == null)
                    mensaje = "Por favor, cargar una foto";
                else
                {
                    Usuario objUsr = new Usuario { idUsuario = Convert.ToInt32(Session["idUsuario"].ToString()), Identificacion = Session["Identificacion"].ToString() };
                    var result = CapturarCuadroVideo(img);
                    dynamic dresult = result.Data;
                    var existeRostro = dresult.existeRostro;
                    if (!existeRostro)
                    {
                        resultadoVerificacion = true;
                        mensaje = dresult.mensaje;
                        //ApiResponse actualizarIntentos = await authDao.ActualizarNumeroIntentos(objUsr, token);
                    }
                    else
                    {
                        PruebaVida objVideo = (PruebaVida)Session["videoPruebaVida"];
                        PruebaVida objImagen = new PruebaVida();
                        objImagen.imagen = Transformacion.convertToByte(img);
                        Session["imagenPruebaVida"] = objImagen;
                        PruebaVida objPruebaVida = new PruebaVida();
                        objPruebaVida.correo = Session["correoCliente"].ToString();
                        objPruebaVida.cedulaOcr = Session["numeroIdentificacion"].ToString();
                        objPruebaVida.idUsuario = Convert.ToInt32(Session["idUsuario"].ToString());
                        objPruebaVida.ipRegistro = Request.UserHostAddress;
                        objPruebaVida.contenType = "video/mpg";
                        objPruebaVida.extension = ".mpg";
                        objPruebaVida.esVideo = true;
                        objPruebaVida.esImage = false;

                        PruebaVidaResponse objResponse = await pruebaVidaDAO.guardarLogPruebaVida(objPruebaVida, token, objVideo.imagen);
                        if (objResponse.estado)
                        {
                            objPruebaVida.contenType = "imagen/png";
                            objPruebaVida.extension = ".png";
                            objPruebaVida.esVideo = false;
                            objPruebaVida.esImage = true;
                            PruebaVidaResponse objResponseImagen = await pruebaVidaDAO.guardarLogPruebaVida(objPruebaVida, token, objImagen.imagen);
                            if (objResponseImagen.estado)
                            {
                                InformacionResponse informacionResponse = await informacionDAO.ObtenerImagen(objPruebaVida.cedulaOcr, token);
                                if (informacionResponse.estado)
                                {
                                    if (informacionResponse.obj.Count > 0)
                                    {
                                        if (!string.IsNullOrEmpty(informacionResponse.obj[0].Valor))
                                        {
                                            byte[] imagenCuadroVideo = Transformacion.convertToByte(Session["cuadroVideo"].ToString());
                                            Session["imagenBase64Dinardap"] = informacionResponse.obj[0].Valor;
                                            byte[] imagenBase64 = Transformacion.convertToByte(informacionResponse.obj[0].Valor);
                                            byte[] imagenDocumento = (byte[])Session["imagenDocumento"];
                                            InformacionPruebaVidaResponse info = await informacionDAO.VerificarPruebaVida(imagenBase64, imagenDocumento, imagenCuadroVideo, objImagen.imagen, token, objPruebaVida);
                                            if (info.estado)
                                            {
                                                Session["porcentajeDocumento"] = 0;
                                                if (info.obj.resultadoVerificacion)
                                                {
                                                    Session["porcentajeDocumento"] = info.obj.verificacion[0].porcentaje * 100;
                                                    estado = true;
                                                    resultadoVerificacion = info.obj.resultadoVerificacion;
                                                }
                                                else
                                                {
                                                    //  ApiResponse actualizarIntentos = await authDao.ActualizarNumeroIntentos(objUsr, token);
                                                    mensaje = "No ha superado la validación biométrica por el siguiente motivo: \n" + info.obj.mensaje;
                                                }
                                            }
                                            else
                                                mensaje = info.mensajes;
                                        }
                                        else
                                            mensaje = "La DINARDAP no tiene registro de su foto de ciudadanía, puede solicitar un ticket de soporte o acercarse al registro civil para actualizar sus datos";
                                    }
                                    else
                                        mensaje = "La DINARDAP no tiene registro de su foto de ciudadanía, puede solicitar un ticket de soporte o acercarse al registro civil para actualizar sus datos";
                                }
                                else
                                    mensaje = informacionResponse.mensajes;

                            }
                            else
                                mensaje = objResponseImagen.mensajes;
                        }
                        else
                            mensaje = objResponse.mensajes;
                    }

                }
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new { estado = estado, mensaje = mensaje, esEcuatoriano = esEcuatoriano, resultadoVerificacion = resultadoVerificacion }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CapturarCuadroVideo(string img)
        {
            bool estado = false;
            string mensaje = "No se ha detectado un rostro , esto se puede deber a que  Usted tiene la cámara apagada, está muy alejado a la cámara, la posición del rostro no está centrado a la cámara, la imagen es de baja calidad o la iluminación no es la adecuado. Por favor, vuelva a tomarse la fotografía en vivo.";
            bool existeRostro = false;
            try
            {
                byte[] cuadroVideo = Transformacion.convertToByte(img);
                string ruta = Server.MapPath("~/Content/haarcascade_frontalface_default.xml");
                cascadeClassifier = new CascadeClassifier(ruta);
                Bitmap bmp;
                using (var ms = new MemoryStream(cuadroVideo))
                    bmp = new Bitmap(ms);
                Image<Bgr, byte> grayImage = new Image<Bgr, byte>(bmp);
                Rectangle[] rectangles = cascadeClassifier.DetectMultiScale(grayImage, 1.3, 3);
                if (rectangles.Length == 1)
                {
                    existeRostro = true;
                    mensaje = "Sí existe rostro";
                }
                else if (rectangles.Length > 1)
                {
                    mensaje = "Se ha detectado más de un rostro. Por favor, vuelva a intentarlo";
                }
                estado = true;
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new { estado = estado, mensaje = mensaje, existeRostro = existeRostro }, JsonRequestBehavior.AllowGet);
        }

    }
}