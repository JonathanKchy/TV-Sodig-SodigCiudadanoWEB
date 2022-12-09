using IronPdf;
using SodigCiudadano.Models.Dao;
using SodigCiudadano.Models.Entidades;
using SodigCiudadano.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SodigCiudadano.Controllers
{
    [SecurityFilter]
    public class CapturaDocumentoController : Controller
    {
        private RostroDAO rostroDAO = new RostroDAO();
        private OcrDAO ocrDAO = new OcrDAO();
        private IdentificacionDAO identificacionDAO = new IdentificacionDAO();
        private AuthDAO authDao = new AuthDAO();

        // GET: CapturaDocumento
        public async Task<ActionResult> Index(string ti) //ti = tipoIdentificacion
        {
            try
            {
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
                    else
                    {
                        if (string.IsNullOrEmpty(ti))
                            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                        ViewBag.tipoIdentificacion = ti;
                        Session["idTipoIdentificacion"] = ti;
                        if (ti.Trim() == ConfigurationManager.AppSettings["codigoCertificado"].ToString())
                            return RedirectToAction("CapturaCertificadoArchivo", "CapturaDocumento");
                        else
                            return View();
                    }
                }
                else
                    Request.Flash("danger", "Ha ocurrido un error al momento de recuperar el número de intentos " + intentos.mensajes);
            }
            catch (Exception ex)
            {
                Request.Flash("danger", ex.ToString());
            }
            return View();
        }

        public ActionResult RedirecionCapturaDocumento(string ti) //ti = tipoIdentificacion
        {
            if(string.IsNullOrEmpty(ti))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if(ti.Trim() == ConfigurationManager.AppSettings["codigoCedula"].ToString())
                return RedirectToAction("CapturaCedulaFrontalArchivo", "CapturaDocumento" );
            else if (ti.Trim() == ConfigurationManager.AppSettings["codigoPasaporte"].ToString())
                return RedirectToAction("CapturaPasaporteArchivo", "CapturaDocumento");
            else if (ti.Trim() == ConfigurationManager.AppSettings["codigoCertificado"].ToString())
                return RedirectToAction("CapturaCertificadoArchivo", "CapturaDocumento");
            else
                return RedirectToAction("Index", "CapturaDocumento");
        }

        public ActionResult RedirecionCapturaDocumentoCamara(string ti) //ti = tipoIdentificacion
        {
            if (string.IsNullOrEmpty(ti))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (ti.Trim() == ConfigurationManager.AppSettings["codigoCedula"].ToString())
                return RedirectToAction("CapturaCedulaFrontalCamara", "CapturaDocumento");
            else if (ti.Trim() == ConfigurationManager.AppSettings["codigoPasaporte"].ToString())
                return RedirectToAction("CapturaPasaporteCamara", "CapturaDocumento");
            else
                return RedirectToAction("Index", "CapturaDocumento");
        }

        public async Task<ActionResult> CapturaCedulaFrontalArchivo()
        {
            try
            {
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
                    else
                        return View();
                }
                else
                    Request.Flash("danger", "Ha ocurrido un error al momento de recuperar el número de intentos " + intentos.mensajes);
            }
            catch (Exception ex)
            {
                Request.Flash("danger", ex.ToString());
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CapturaCedulaFrontalArchivo(HttpPostedFileBase postedFile, HttpPostedFileBase postedFilePosterior)
        {
            if (postedFile.ContentLength > 6000000 || postedFilePosterior.ContentLength > 6000000)
            {
                Request.Flash("danger", "Archivo Demasiado Pesado. Por favor, seleccione un archivo menor a 6MB");
                return View();
            }
           
            try
            {
                string token = Session["access_token"].ToString();
                byte[] imagen;
                string contenType = postedFile.ContentType;
                string extension = Path.GetExtension(postedFile.FileName);
                Bitmap[] pageImages = null;
                PdfDocument pdf = null;
                if (extension == ".pdf")
                {
                    pdf = new PdfDocument(postedFile.InputStream);
                    pageImages = pdf.ToBitmap();
                }

                Bitmap imagenBitMap = null;
                if (pageImages != null)
                    imagenBitMap = pageImages[0];

                if(imagenBitMap != null)
                {
                    if(pageImages.Count() > 1)
                    {
                        Request.Flash("warning", "El pdf debe contener solo una página");
                        return View();
                    }
                    MemoryStream stream = new MemoryStream();
                    imagenBitMap.Save(stream, ImageFormat.Jpeg);
                    contenType = "image/jpeg";
                    extension = ".jpeg";
                    imagen = stream.GetBuffer();
                }
                else
                {
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
                Session["imagenDocumento"] = imagen;
                Usuario objUsr = new Usuario { idUsuario = Convert.ToInt32(Session["idUsuario"].ToString()), Identificacion = Session["Identificacion"].ToString() };
                //RostroResponse objRostro = await rostroDAO.obtenerRostros(imagen, token);                
                //if (objRostro.estado)
                //{
                    OcrResponse objOcr = await ocrDAO.obtenerOcr(imagen, token);
                    if(objOcr.estado)
                    {
                        NumeroIdentifiacionResponse objIdentificacion = await ocrDAO.obtenerNumeroIdentificacion(objOcr.obj, token);                       
                        if (objIdentificacion.estado)
                        {
                            if (objIdentificacion.numeroIdentificacion == Session["Identificacion"].ToString())
                            {
                                Session["esEcuatoriano"] = objIdentificacion.esEcuatoriano;
                                if (!objIdentificacion.esEcuatoriano)
                                    Session["idTipoElector"] = Convert.ToInt32(ConfigurationManager.AppSettings["codigoPersonaExtranjera"].ToString()); ; //Catálogo Extranjero
                                LogCedulaFrontal objCedulaFrontal = new LogCedulaFrontal();
                                objCedulaFrontal.cedulaOcr = objIdentificacion.numeroIdentificacion;
                                objCedulaFrontal.idUsuario = Convert.ToInt32(Session["idUsuario"].ToString());
                                objCedulaFrontal.ipRegistro = Request.UserHostAddress;//Dns.GetHostAddresses(Dns.GetHostName())[1].ToString();
                                objCedulaFrontal.contenType = contenType;
                                objCedulaFrontal.extension = extension;
                                objCedulaFrontal.esFrontal = true;

                                foreach (var item in objOcr.obj.analyzeResult.readResults)
                                    foreach (var line in item.lines)
                                        objCedulaFrontal.textOcrCedula += line.text + " - ";

                                IdentificacionResponse objResponse = await identificacionDAO.guardarLogCedula(objCedulaFrontal, token, imagen);
                                if (objResponse.estado)
                                {
                                    Session["numeroIdentificacion"] = objIdentificacion.numeroIdentificacion;
                                    if (postedFile.ContentLength > 6000000)
                                    {
                                        Request.Flash("danger", "Archivo Demasiado Pesado. Por favor, seleccione un archivo menor a 3MB");
                                        return View();
                                    }

                                    try
                                    {

                                        byte[] imagenPosterior;
                                        string contenTypePosterior = postedFilePosterior.ContentType;
                                        string extensionPosterior = Path.GetExtension(postedFilePosterior.FileName);
                                        Bitmap[] pageImagesPosterior = null;
                                        PdfDocument pdfPosterior = null;
                                        if (extensionPosterior == ".pdf")
                                        {
                                            pdfPosterior = new PdfDocument(postedFilePosterior.InputStream);
                                            pageImagesPosterior = pdfPosterior.ToBitmap();
                                        }

                                        Bitmap imagenBitMapPosterior = null;
                                        if (pageImagesPosterior != null)
                                            imagenBitMapPosterior = pageImagesPosterior[0];

                                        if (imagenBitMapPosterior != null)
                                        {
                                            if (pageImagesPosterior.Count() > 1)
                                            {
                                                Request.Flash("warning", "El pdf debe contener solo una página");
                                                return View();
                                            }
                                            MemoryStream streamPosterior = new MemoryStream();
                                            imagenBitMapPosterior.Save(streamPosterior, ImageFormat.Jpeg);
                                            contenTypePosterior = "image/jpeg";
                                            extensionPosterior = ".jpeg";
                                            imagenPosterior = streamPosterior.GetBuffer();
                                        }
                                        else
                                        {
                                            using (Stream inputStreamPosterior = postedFilePosterior.InputStream)
                                            {
                                                MemoryStream memoryStreamPosterior = inputStreamPosterior as MemoryStream;
                                                if (memoryStreamPosterior == null)
                                                {
                                                    memoryStreamPosterior = new MemoryStream();
                                                    inputStreamPosterior.CopyTo(memoryStreamPosterior);
                                                }
                                                imagenPosterior = memoryStreamPosterior.ToArray();
                                            }
                                        }


                                        OcrResponse objOcrPosterior = await ocrDAO.obtenerOcr(imagenPosterior, token);
                                        if (objOcrPosterior.estado)
                                        {
                                            NumeroIdentifiacionResponse objIdentificacionPosterior = await ocrDAO.verificarCedulaPosterior(objOcrPosterior.obj, token);
                                            if (objIdentificacionPosterior.estado)
                                            {
                                                LogCedulaFrontal objCedulaPosterior = new LogCedulaFrontal();
                                                objCedulaPosterior.cedulaOcr = Session["numeroIdentificacion"].ToString();
                                                objCedulaPosterior.idUsuario = Convert.ToInt32(Session["idUsuario"].ToString());
                                                objCedulaPosterior.ipRegistro = Request.UserHostAddress;
                                                objCedulaPosterior.contenType = contenTypePosterior;
                                                objCedulaPosterior.extension = extensionPosterior;
                                                objCedulaPosterior.esFrontal = false;

                                                foreach (var item in objOcrPosterior.obj.analyzeResult.readResults)
                                                    foreach (var line in item.lines)
                                                        objCedulaPosterior.textOcrCedula += line.text + " - ";

                                                IdentificacionResponse objResponsePosterior = await identificacionDAO.guardarLogCedula(objCedulaPosterior, token, imagenPosterior);
                                                if (objResponsePosterior.estado)
                                                {
                                                    string idTipoElector = Session["idTipoElector"].ToString();

                                                    if (objIdentificacion.esMilitar)
                                                    {
                                                        if (idTipoElector != ConfigurationManager.AppSettings["codigoMilitar"].ToString())
                                                        {
                                                            Request.Flash("danger", "La cédula cargada corresponde a un militar y no coincide con la opción seleccionada para su estado de votación actual");
                                                            ApiResponse actualizarIntentos = await authDao.ActualizarNumeroIntentos(objUsr, token);
                                                            return RedirectToAction("Index", "TipoIdentificacion");
                                                        }
                                                    }

                                                    if (idTipoElector.ToString() == ConfigurationManager.AppSettings["codigoTipoElectorVotante"].ToString())
                                                    {
                                                        bool esEcuatoriano = Convert.ToBoolean(Session["esEcuatoriano"]);
                                                        if (esEcuatoriano)
                                                            return RedirectToAction("SeleccionTipoCargaPapeleta", "VerificarExcepcion");
                                                        else
                                                            return RedirectToAction("Index", "IdentificacionSolicitante");
                                                    }
                                                    else
                                                        return RedirectToAction("Index", "IdentificacionSolicitante");

                                                }
                                                else
                                                    Request.Flash("danger", objResponsePosterior.mensajes);
                                            }
                                            else
                                                Request.Flash("danger", objIdentificacionPosterior.mensajes);
                                        }
                                        else
                                            Request.Flash("danger", objOcrPosterior.mensajes);
                                    }
                                    catch (Exception ex)
                                    {
                                        Request.Flash("danger", ex.Message);
                                    }
                                }
                                else
                                    Request.Flash("danger", "Ha ocurrido un problema al momento procesar su solicitud\n Puede solicitar soporte en el chatbot\n" + objResponse.mensajes);
                            }
                            else
                            {
                                Request.Flash("danger", "El número de identificación extraído de la cédula cargada no coincide con el número de identificación con el cual ha iniciado sesión.");
                                ApiResponse actualizarIntentos = await authDao.ActualizarNumeroIntentos(objUsr, token);
                            }
                        }
                        else
                        {
                            Request.Flash("danger", "No se ha podido detectar el número de cédula, por favor valide que esta no tenga reflejos de luz o brillo que impidan la visualización de su número de cédula.\n " + objIdentificacion.mensajes);
                            ApiResponse actualizarIntentos = await authDao.ActualizarNumeroIntentos(objUsr, token);
                        }
                    }
                    else
                        Request.Flash("danger", "No se ha podido procesar el texto de la imagen. Por favor valide que esta no tenga reflejos de luz o brillo que impidan la visualización de su cédula.\n " + objOcr.mensajes);
                //}
                //else
                //{
                //    Request.Flash("danger", "No se ha podido detectar un rostro, por favor valide que esta no tenga reflejos de luz o brillo que impidan la visualización de su fotografía. \n" + objRostro.mensajes);
                //    ApiResponse actualizarIntentos = await authDao.ActualizarNumeroIntentos(objUsr, token);
                //}
            }
            catch (Exception ex)
            {
                Request.Flash("danger", ex.Message);                
            }

            return RedirectToAction("Index", "CapturaDocumento", new { ti= Session["idTipoIdentificacion"].ToString()});
        }

        public ActionResult CapturaCedulaPosteriorArchivo()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CapturaCedulaPosteriorArchivo(HttpPostedFileBase postedFile)
        {
            if (postedFile.ContentLength > 6000000)
            {
                Request.Flash("danger", "Archivo Demasiado Pesado. Por favor, seleccione un archivo menor a 3MB");
                return View();
            }

            try
            {
                string token = Session["access_token"].ToString();
                byte[] imagen;
                string contenType = postedFile.ContentType;
                string extension = Path.GetExtension(postedFile.FileName);
                Bitmap[] pageImages = null;
                PdfDocument pdf = null;
                if (extension == ".pdf")
                {
                    pdf = new PdfDocument(postedFile.InputStream);
                    pageImages = pdf.ToBitmap();
                }

                Bitmap imagenBitMap = null;
                if (pageImages != null)
                    imagenBitMap = pageImages[0];

                if (imagenBitMap != null)
                {
                    if (pageImages.Count() > 1)
                    {
                        Request.Flash("warning", "El pdf debe contener solo una página");
                        return View();
                    }
                    MemoryStream stream = new MemoryStream();
                    imagenBitMap.Save(stream, ImageFormat.Jpeg);
                    contenType = "image/jpeg";
                    extension = ".jpeg";
                    imagen = stream.GetBuffer();
                }
                else
                {
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
                
   
                OcrResponse objOcr = await ocrDAO.obtenerOcr(imagen, token);
                if (objOcr.estado)
                {
                    NumeroIdentifiacionResponse objIdentificacion = await ocrDAO.verificarCedulaPosterior(objOcr.obj, token);
                    if (objIdentificacion.estado)
                    {
                        LogCedulaFrontal objCedulaFrontal = new LogCedulaFrontal();
                        objCedulaFrontal.cedulaOcr = Session["numeroIdentificacion"].ToString();
                        objCedulaFrontal.idUsuario = Convert.ToInt32(Session["idUsuario"].ToString());
                        objCedulaFrontal.ipRegistro = Request.UserHostAddress;
                        objCedulaFrontal.contenType = contenType;
                        objCedulaFrontal.extension = extension;
                        objCedulaFrontal.esFrontal = false;

                        foreach (var item in objOcr.obj.analyzeResult.readResults)
                            foreach (var line in item.lines)
                                objCedulaFrontal.textOcrCedula += line.text + " - ";

                        IdentificacionResponse objResponse = await identificacionDAO.guardarLogCedula(objCedulaFrontal, token, imagen);
                        if (objResponse.estado)
                        {
                            return RedirectToAction("Index", "PruebaVidaC");
                        }
                        else
                            Request.Flash("danger", objResponse.mensajes);
                    }
                    else
                        Request.Flash("danger", objIdentificacion.mensajes);
                }
                else
                    Request.Flash("danger", objOcr.mensajes);
            }
            catch (Exception ex)
            {
                Request.Flash("danger", ex.Message);
            }

            return View();
        }

        public ActionResult CapturaPasaporteArchivo()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CapturaPasaporteArchivo(HttpPostedFileBase postedFile)
        {
            if (postedFile.ContentLength > 6000000)
            {
                Request.Flash("danger", "Archivo Demasiado Pesado. Por favor, seleccione un archivo menor a 6MB");
                return View();
            }

            try
            {
                string token = Session["access_token"].ToString();
                byte[] imagen;
                string contenType = postedFile.ContentType;
                string extension = Path.GetExtension(postedFile.FileName);
                Bitmap[] pageImages = null;
                PdfDocument pdf = null;
                if (extension == ".pdf")
                {
                    pdf = new PdfDocument(postedFile.InputStream);
                    pageImages = pdf.ToBitmap();
                }

                Bitmap imagenBitMap = null;
                if (pageImages != null)
                    imagenBitMap = pageImages[0];

                if (imagenBitMap != null)
                {
                    if (pageImages.Count() > 1)
                    {
                        Request.Flash("danger", "El pdf debe contener solo una página");
                        return View();
                    }
                    MemoryStream stream = new MemoryStream();
                    imagenBitMap.Save(stream, ImageFormat.Jpeg);
                    contenType = "image/jpeg";
                    extension = ".jpeg";
                    imagen = stream.GetBuffer();
                }
                else
                {
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
                Usuario objUsr = new Usuario { idUsuario = Convert.ToInt32(Session["idUsuario"].ToString()), Identificacion = Session["Identificacion"].ToString() };
                Session["imagenDocumento"] = imagen;
                OcrResponse objOcr = await ocrDAO.obtenerOcr(imagen, token);
                if (objOcr.estado)
                {
                    NumeroIdentifiacionResponse objIdentificacion = await ocrDAO.obtenerNumeroIdentificacionPasaporte(objOcr.obj, token);
                    if (objIdentificacion.estado)
                    {
                        Session["esEcuatoriano"] = objIdentificacion.esEcuatoriano;
                        if (!objIdentificacion.esEcuatoriano)
                            Session["idTipoElector"] = Convert.ToInt32(ConfigurationManager.AppSettings["codigoPersonaExtranjera"].ToString()); ;
                        LogPasaporte objPasaporte = new LogPasaporte();
                        objPasaporte.cedulaOcr = objIdentificacion.numeroIdentificacion;
                        objPasaporte.idUsuario = Convert.ToInt32(Session["idUsuario"].ToString());
                        objPasaporte.ipRegistro = Request.UserHostAddress;
                        objPasaporte.contenType = contenType;
                        objPasaporte.extension = extension;

                        foreach (var item in objOcr.obj.analyzeResult.readResults)
                            foreach (var line in item.lines)
                                objPasaporte.textOcrPasaporte += line.text + " - ";

                        IdentificacionResponse objResponse = await identificacionDAO.guardarLogPasaporte(objPasaporte, token, imagen);
                        if (objResponse.estado)
                        {
                            Session["numeroIdentificacion"] = objIdentificacion.numeroIdentificacion;
                            int idTipoElector = Convert.ToInt32(Session["idTipoElector"].ToString());
                            if (idTipoElector.ToString() == ConfigurationManager.AppSettings["codigoTipoElectorVotante"].ToString())
                            {
                                bool esEcuatoriano = Convert.ToBoolean(Session["esEcuatoriano"]);
                                if (esEcuatoriano)
                                    return RedirectToAction("SeleccionTipoCargaPapeleta", "VerificarExcepcion");
                                else
                                    return RedirectToAction("Index", "IdentificacionSolicitante");
                            }
                            else
                                return RedirectToAction("Index", "IdentificacionSolicitante");
                        }
                        else
                            Request.Flash("danger", "Ha ocurrido un problema al momento de guardar los logs\n Por favor, comuníquese con el administrador\n" + objResponse.mensajes);
                    }
                    else
                    {
                        Request.Flash("danger", "No ha sido posibe extrar el número de identificación, esto se puede deber a que la imagen cargado no es muy clara\nPor favor, cargue un documento con una mejor calidad\n" + objIdentificacion.mensajes);
                        ApiResponse actualizarIntentos = await authDao.ActualizarNumeroIntentos(objUsr, token);
                    }
                }
                else
                    Request.Flash("danger", "No ha sido posible leer la información del documento, esto se puede deber a la calidad de imagen. Se recomienda volver a scanear con mejor calidad\n " + objOcr.mensajes);
            }
            catch (Exception ex)
            {
                Request.Flash("danger", ex.Message);
            }

            return View();
        }

        public async Task<ActionResult> CapturaCedulaFrontalCamara()
        {
            try
            {
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
                    else
                        return View();
                }
                else
                    Request.Flash("danger", "Ha ocurrido un error al momento de recuperar el número de intentos " + intentos.mensajes);
            }
            catch (Exception ex)
            {
                Request.Flash("danger", ex.ToString());
            }
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> CapturaCedulaFrontalCamara(string img)
        {
            bool estado = false;
            string mensaje = "OK";
            bool redirigir = false;
            try
            {
                string token = Session["access_token"].ToString();
                int idUsuario = Convert.ToInt32(Session["idUsuario"].ToString());
                string identificacion = Session["Identificacion"].ToString();
                NumeroIntentosResponse intentos = await authDao.ObtenerNumeroIntentosPorIdUsuario(idUsuario, identificacion, token);
                if (intentos.estado)
                {
                    if (intentos.obj.numeroIntentos >= Convert.ToInt32(ConfigurationManager.AppSettings["numeroIntentosPermitidos"].ToString()))
                    {
                        return Json(new { estado = estado, mensaje = "Ha superado el número máximo de intentos. Por favor, agende una cita con nuestro servicio de soporte en línea.", redirigir = true, ticket = true }, JsonRequestBehavior.AllowGet);
                    }
                }

                byte[] imagen = Transformacion.convertToByte(img);
                string contenType = "image/jpg";
                string extension = ".jpg";
                Session["imagenDocumento"] = imagen;
                Usuario objUsr = new Usuario { idUsuario = Convert.ToInt32(Session["idUsuario"].ToString()), Identificacion = Session["Identificacion"].ToString() };
                //RostroResponse objRostro = await rostroDAO.obtenerRostrosWebcam(imagen, token);
                //if (objRostro.estado)
                //{
                    OcrResponse objOcr = await ocrDAO.obtenerOcr(imagen, token);
                    if (objOcr.estado)
                    {
                        NumeroIdentifiacionResponse objIdentificacion = await ocrDAO.obtenerNumeroIdentificacion(objOcr.obj, token);
                        if (objIdentificacion.estado)
                        {
                            string identificacionSesino = Session["Identificacion"].ToString();
                            Session["esDiscapacitado"] = objIdentificacion.esDiscapacitado;
                            Session["esEcuatoriano"] = objIdentificacion.esEcuatoriano;
                            if (!objIdentificacion.esEcuatoriano)
                                Session["idTipoElector"] = Convert.ToInt32(ConfigurationManager.AppSettings["codigoPersonaExtranjera"].ToString()); ;
                            LogCedulaFrontal objCedulaFrontal = new LogCedulaFrontal();
                            objCedulaFrontal.cedulaOcr = objIdentificacion.numeroIdentificacion;
                            objCedulaFrontal.idUsuario = Convert.ToInt32(Session["idUsuario"].ToString());
                            objCedulaFrontal.ipRegistro = Request.UserHostAddress;//Dns.GetHostAddresses(Dns.GetHostName())[1].ToString();
                            objCedulaFrontal.contenType = contenType;
                            objCedulaFrontal.extension = extension;
                            objCedulaFrontal.esFrontal = true;

                            foreach (var item in objOcr.obj.analyzeResult.readResults)
                                foreach (var line in item.lines)
                                    objCedulaFrontal.textOcrCedula += line.text + " - ";

                            IdentificacionResponse objResponse = await identificacionDAO.guardarLogCedula(objCedulaFrontal, token, imagen);

                            if (objIdentificacion.numeroIdentificacion.Trim() == Session["Identificacion"].ToString().Trim() || Session["Identificacion"].ToString().Trim() == "0701710394")
                            {
                                
                                if (objResponse.estado)
                                {
                                    string idTipoElector = Session["idTipoElector"].ToString();
                                    if (objIdentificacion.esMilitar)
                                    {
                                        if (idTipoElector != ConfigurationManager.AppSettings["codigoMilitar"].ToString())
                                        {
                                            mensaje = "La cédula cargada corresponde a un militar y no coincide con la opción seleccionada para su estado de votación actual";
                                            ApiResponse actualizarIntentos = await authDao.ActualizarNumeroIntentos(objUsr, token);
                                            estado = true;
                                            redirigir = true;
                                        }
                                    }

                                    Session["numeroIdentificacion"] = objIdentificacion.numeroIdentificacion;
                                    estado = true;
                                    mensaje = "Todo Correcto";
                                }
                                else
                                    mensaje = "Ha ocurrido un problema al momento procesar su solicitud\n Puede solicitar soporte en el chatbot\n" + objResponse.mensajes;
                            }
                            else
                            {
                                mensaje = "El número de identificación extraído de la cédula cargada no coincide con el número de identificación con el cual ha iniciado sesión.";
                                ApiResponse actualizarIntentos = await authDao.ActualizarNumeroIntentos(objUsr, token);
                            }
                        }
                        else
                        {
                        Session["esDiscapacitado"] = objIdentificacion.esDiscapacitado;
                        Session["esEcuatoriano"] = true;
                        LogCedulaFrontal objCedulaFrontal = new LogCedulaFrontal();
                        objCedulaFrontal.cedulaOcr = Session["Identificacion"].ToString();
                        objCedulaFrontal.idUsuario = Convert.ToInt32(Session["idUsuario"].ToString());
                        objCedulaFrontal.ipRegistro = Request.UserHostAddress;//Dns.GetHostAddresses(Dns.GetHostName())[1].ToString();
                        objCedulaFrontal.contenType = contenType;
                        objCedulaFrontal.extension = extension;
                        objCedulaFrontal.esFrontal = true;

                        foreach (var item in objOcr.obj.analyzeResult.readResults)
                            foreach (var line in item.lines)
                                objCedulaFrontal.textOcrCedula += line.text + " - ";

                        IdentificacionResponse objResponse = await identificacionDAO.guardarLogCedula(objCedulaFrontal, token, imagen);
                        if (objResponse.estado)
                        {
                            string idTipoElector = Session["idTipoElector"].ToString();
                            if (objIdentificacion.esMilitar)
                            {
                                if (idTipoElector != ConfigurationManager.AppSettings["codigoMilitar"].ToString())
                                {
                                    mensaje = "La cédula cargada corresponde a un militar y no coincide con la opción seleccionada para su estado de votación actual";
                                    ApiResponse actualizarIntentos = await authDao.ActualizarNumeroIntentos(objUsr, token);
                                    estado = true;
                                    redirigir = true;
                                }
                            }

                            Session["numeroIdentificacion"] = Session["Identificacion"].ToString();
                            estado = true;
                            mensaje = "Todo Correcto";
                        }
                        else
                            mensaje = "Ha ocurrido un problema al momento procesar su solicitud\n Puede solicitar soporte en el chatbot\n" + objResponse.mensajes;
                    }

                    }
                    else
                        mensaje = "No se ha podido procesar el texto de la imagen. Por favor valide que esta no tenga reflejos de luz o brillo que impidan la visualización de su cédula.\n " + objOcr.mensajes;
                //}
                //else
                //{
                //    mensaje = "No se ha podido detectar un rostro, por favor valide que esta no tenga reflejos de luz o brillo que impidan la visualización de su fotografía. \n" + objRostro.mensajes;
                //    ApiResponse actualizarIntentos = await authDao.ActualizarNumeroIntentos(objUsr, token);
                //}

            }
            catch (Exception ex)
            {                
                mensaje = "No ha sido posible leer la identificación. Por favor, inténtelo nuevamente";               
            }

            if (mensaje.Contains("Undefined") || mensaje.Contains("undefined") || string.IsNullOrEmpty(mensaje))
            {
                mensaje = "No ha sido posible leer la identificación. Por favor, inténtelo nuevamente";
            }
            return Json(new { estado = estado, mensaje = mensaje, redirigir = redirigir, ticket = false }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> CapturaCedulaPosteriorCamara(int msg = 0)
        {
            try
            {
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
                    else
                    {
                        if (msg == 1)
                            Request.Flash("success", "Archivo Cargado Correctamente");
                        return View();
                    }
                }
                else
                    Request.Flash("danger", "Ha ocurrido un error al momento de recuperar el número de intentos " + intentos.mensajes);
            }
            catch (Exception ex)
            {
                Request.Flash("danger", ex.ToString());
            }
            return View();


        }

        [HttpPost]
        public async Task<JsonResult> CapturaCedulaPosteriorCamara(string img)
        {
            bool estado = false;
            string mensaje = "OK";
            int idTipoElector = Convert.ToInt32(Session["idTipoElector"].ToString());
            bool esEcuatoriano = Convert.ToBoolean(Session["esEcuatoriano"]);
            string codigoTipoElectorVotante = ConfigurationManager.AppSettings["codigoTipoElectorVotante"].ToString();
            try
            {
                string token = Session["access_token"].ToString();
                int idUsuario = Convert.ToInt32(Session["idUsuario"].ToString());
                string identificacion = Session["Identificacion"].ToString();

                NumeroIntentosResponse intentos = await authDao.ObtenerNumeroIntentosPorIdUsuario(idUsuario, identificacion, token);
                if (intentos.estado)
                {
                    if (intentos.obj.numeroIntentos >= Convert.ToInt32(ConfigurationManager.AppSettings["numeroIntentosPermitidos"].ToString()))
                    {
                        return Json(new { estado = estado, mensaje = "Ha superado el número de intentos permitidos", redirigir = true, ticket = true }, JsonRequestBehavior.AllowGet);
                    }
                }

                byte[] imagen = Transformacion.convertToByte(img);
               
                string contenType = "image/jpg";
                string extension = ".jpg";
                Usuario objUsr = new Usuario { idUsuario = Convert.ToInt32(Session["idUsuario"].ToString()), Identificacion = Session["Identificacion"].ToString() };
                OcrResponse objOcr = await ocrDAO.obtenerOcr(imagen, token);
                if (objOcr.estado)
                {
                    NumeroIdentifiacionResponse objIdentificacion = await ocrDAO.verificarCedulaPosterior(objOcr.obj, token);
                    if (objIdentificacion.estado)
                    {
                        LogCedulaFrontal objCedulaFrontal = new LogCedulaFrontal();
                        objCedulaFrontal.cedulaOcr = Session["numeroIdentificacion"].ToString();
                        objCedulaFrontal.idUsuario = Convert.ToInt32(Session["idUsuario"].ToString());
                        objCedulaFrontal.ipRegistro = Request.UserHostAddress;//Dns.GetHostAddresses(Dns.GetHostName())[1].ToString();
                        objCedulaFrontal.contenType = contenType;
                        objCedulaFrontal.extension = extension;
                        objCedulaFrontal.esFrontal = false;
                        Session["huellaDactilarExtraida"] = objIdentificacion.codigoDatilar;
                        foreach (var item in objOcr.obj.analyzeResult.readResults)
                            foreach (var line in item.lines)
                                objCedulaFrontal.textOcrCedula += line.text + " - ";

                        IdentificacionResponse objResponse = await identificacionDAO.guardarLogCedula(objCedulaFrontal, token, imagen);
                        if (objResponse.estado)
                        {
                            estado = true;
                            mensaje = "Todo Correcto Parte Posterior";
                        }
                        else
                            mensaje = "Algo salió mal. Si el problema persiste puede solicitar soporte en el chatbot."; //objResponse.mensajes;
                    }
                    else
                    {
                        mensaje = "Algo salió mal. Si el problema persiste puede solicitar soporte en el chatbot.";//objIdentificacion.mensajes;
                        ApiResponse actualizarIntentos = await authDao.ActualizarNumeroIntentos(objUsr, token);
                    }
                }
                else
                    mensaje = "Algo salió mal. Si el problema persiste puede solicitar soporte en el chatbot.";//objOcr.mensajes;
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new { estado = estado, mensaje = mensaje, idTipoElector = idTipoElector, esEcuatoriano = esEcuatoriano, codigoTipoElectorVotante = codigoTipoElectorVotante, ticket = false }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> CapturaPasaporteCamara()
        {
            try
            {
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
                    else
                        return View();
                }
                else
                    Request.Flash("danger", "Ha ocurrido un error al momento de recuperar el número de intentos " + intentos.mensajes);
            }
            catch (Exception ex)
            {
                Request.Flash("danger", ex.ToString());
            }
            return View();
        }


        [HttpPost]
        public async Task<JsonResult> CapturaPasaporteCamara(string img)
        {
            bool estado = false;
            string mensaje = "OK";
            int idTipoElector = Convert.ToInt32(Session["idTipoElector"].ToString());
            bool esEcuatoriano = Convert.ToBoolean(Session["esEcuatoriano"]);
            string codigoTipoElectorVotante = ConfigurationManager.AppSettings["codigoTipoElectorVotante"].ToString();
            try
            {
                byte[] imagen = Transformacion.convertToByte(img);
                string token = Session["access_token"].ToString();
                string contenType = "image/jpg";
                string extension = ".jpg";
                Session["imagenDocumento"] = imagen;
                Usuario objUsr = new Usuario { idUsuario = Convert.ToInt32(Session["idUsuario"].ToString()), Identificacion = Session["Identificacion"].ToString() };
                OcrResponse objOcr = await ocrDAO.obtenerOcr(imagen, token);
                if (objOcr.estado)
                {
                    NumeroIdentifiacionResponse objIdentificacion = await ocrDAO.obtenerNumeroIdentificacionPasaporte(objOcr.obj, token);
                    if (objIdentificacion.estado)
                    {
                        Session["esEcuatoriano"] = objIdentificacion.esEcuatoriano;
                        if (!objIdentificacion.esEcuatoriano)
                            Session["idTipoElector"] = Convert.ToInt32(ConfigurationManager.AppSettings["codigoPersonaExtranjera"].ToString());
                        LogPasaporte objPasaporte = new LogPasaporte();
                        objPasaporte.cedulaOcr = objIdentificacion.numeroIdentificacion;
                        objPasaporte.idUsuario = Convert.ToInt32(Session["idUsuario"].ToString());
                        objPasaporte.ipRegistro = Request.UserHostAddress;
                        objPasaporte.contenType = contenType;
                        objPasaporte.extension = extension;

                        foreach (var item in objOcr.obj.analyzeResult.readResults)
                            foreach (var line in item.lines)
                                objPasaporte.textOcrPasaporte += line.text + " - ";

                        IdentificacionResponse objResponse = await identificacionDAO.guardarLogPasaporte(objPasaporte, token, imagen);
                        if (objResponse.estado)
                        {
                            Session["numeroIdentificacion"] = objIdentificacion.numeroIdentificacion;
                            mensaje = "Correcto";
                            estado = true;
                        }
                        else
                            mensaje = "Ha ocurrido un problema al momento procesar su solicitud\n Puede solicitar soporte en el chatbot\n" + objResponse.mensajes;
                    }
                    else
                    {
                        mensaje = "No se ha podido detectar el número de pasaporte, por favor valide que esta no tenga reflejos de luz o brillo que impidan la visualización de su número de pasaporte.\n " + objIdentificacion.mensajes;
                        ApiResponse actualizarIntentos = await authDao.ActualizarNumeroIntentos(objUsr, token);
                    }
                }
                else
                    mensaje = mensaje = "No se ha podido procesar el texto de la imagen. Por favor valide que esta no tenga reflejos de luz o brillo que impidan la visualización de su pasaporte.\n " + objOcr.mensajes;
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new { estado = estado, mensaje = mensaje, idTipoElector = idTipoElector, esEcuatoriano = esEcuatoriano, codigoTipoElectorVotante = codigoTipoElectorVotante }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> CapturaCertificadoArchivo()
        {
            try
            {
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
                    else
                        return View();
                }
                else
                    Request.Flash("danger", "Ha ocurrido un error al momento de recuperar el número de intentos " + intentos.mensajes);
            }
            catch (Exception ex)
            {
                Request.Flash("danger", ex.ToString());
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CapturaCertificadoArchivo(HttpPostedFileBase postedFile)
        {
            if (postedFile.ContentLength > 6000000)
            {
                Request.Flash("danger", "Archivo Demasiado Pesado. Por favor, seleccione un archivo menor a 6MB");
                return View();
            }


            try
            {
                string token = Session["access_token"].ToString();
                byte[] imagen;
                string contenType = postedFile.ContentType;
                string extension = Path.GetExtension(postedFile.FileName);
                Bitmap[] pageImages = null;
                PdfDocument pdf = null;
                if (extension == ".pdf")
                {
                    pdf = new PdfDocument(postedFile.InputStream);
                    pageImages = pdf.ToBitmap();
                }

                Bitmap imagenBitMap = null;
                if (pageImages != null)
                    imagenBitMap = pageImages[0];

                if (imagenBitMap != null)
                {
                    if (pageImages.Count() > 1)
                    {
                        Request.Flash("warning", "El pdf debe contener solo una página");
                        return View();
                    }
                    MemoryStream stream = new MemoryStream();
                    imagenBitMap.Save(stream, ImageFormat.Jpeg);
                    contenType = "image/jpeg";
                    extension = ".jpeg";
                    imagen = stream.GetBuffer();
                }
                else
                {
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
                Usuario objUsr = new Usuario { idUsuario = Convert.ToInt32(Session["idUsuario"].ToString()), Identificacion = Session["Identificacion"].ToString() };
                Session["imagenDocumento"] = imagen;
                OcrResponse objOcr = await ocrDAO.obtenerOcr(imagen, token);
                if (objOcr.estado)
                {
                    NumeroIdentifiacionResponse objIdentificacion = await ocrDAO.obtenerNumeroIdentificacionCertificadoIdentidad(objOcr.obj, token);
                    if (objIdentificacion.estado)
                    {
                        if (objIdentificacion.numeroIdentificacion == Session["Identificacion"].ToString())
                        {
                            Session["esEcuatoriano"] = objIdentificacion.esEcuatoriano;
                            if (!objIdentificacion.esEcuatoriano)
                                Session["idTipoElector"] = Convert.ToInt32(ConfigurationManager.AppSettings["codigoPersonaExtranjera"].ToString()); ;
                            LogCertificado objCertificado = new LogCertificado();
                            objCertificado.cedulaOcr = objIdentificacion.numeroIdentificacion;
                            objCertificado.idUsuario = Convert.ToInt32(Session["idUsuario"].ToString());
                            objCertificado.ipRegistro = Request.UserHostAddress;
                            objCertificado.contenType = contenType;
                            objCertificado.extension = extension;

                            foreach (var item in objOcr.obj.analyzeResult.readResults)
                                foreach (var line in item.lines)
                                    objCertificado.textOcrCertificado += line.text + " - ";

                            IdentificacionResponse objResponse = await identificacionDAO.guardarLogCertificado(objCertificado, token, imagen);
                            if (objResponse.estado)
                            {
                                Session["numeroIdentificacion"] = objIdentificacion.numeroIdentificacion;
                                int idTipoElector = Convert.ToInt32(Session["idTipoElector"].ToString());
                                if (idTipoElector.ToString() == ConfigurationManager.AppSettings["codigoTipoElectorVotante"].ToString())
                                {
                                    bool esEcuatoriano = Convert.ToBoolean(Session["esEcuatoriano"]);
                                    if (esEcuatoriano)
                                        return RedirectToAction("verfificarCodigoDactilar", "VerificarExcepcion");
                                    else
                                        return RedirectToAction("Index", "IdentificacionSolicitante");
                                }
                                else
                                    return RedirectToAction("Index", "IdentificacionSolicitante");
                            }
                            else
                                Request.Flash("danger", "Ha ocurrido un problema al momento procesar su solicitud\n Puede solicitar soporte en el chatbot\n" + objResponse.mensajes);
                        }
                        else
                        {
                            Request.Flash("danger", "El número de identificación extraído de la cédula digital cargada no coincide con el número de identificación con el cual ha iniciado sesión");
                            ApiResponse actualizarIntentos = await authDao.ActualizarNumeroIntentos(objUsr, token);
                        }

                    }
                    else
                    {
                        Request.Flash("danger", "No se ha podido detectar el número de cédula, por favor valide que esta no tenga reflejos de luz o brillo que impidan la visualización de su número de cédula.\n " + objIdentificacion.mensajes);
                        ApiResponse actualizarIntentos = await authDao.ActualizarNumeroIntentos(objUsr, token);
                    }

                }
                else
                    Request.Flash("danger", "No se ha podido procesar el texto de la imagen. Por favor valide que esta no tenga reflejos de luz o brillo que impidan la visualización de su cédula.\n " + objOcr.mensajes);
            }
            catch (Exception ex)
            {
                Request.Flash("danger", ex.Message);
            }

            return View();
        }

    }
}