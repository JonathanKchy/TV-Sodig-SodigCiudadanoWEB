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
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace SodigCiudadano.Controllers
{
    [SecurityFilter]
    public class VerificarExcepcionController : Controller
    {
        private TipoElectorDAO objDAO = new TipoElectorDAO();
        private RostroDAO rostroDAO = new RostroDAO();
        private OcrDAO ocrDAO = new OcrDAO();
        private IdentificacionDAO identificacionDAO = new IdentificacionDAO();
        private LogDAO logDAO = new LogDAO();
        private AuthDAO authDao = new AuthDAO();
        private InformacionDAO objInformacionDAO = new InformacionDAO();

        // GET: VerificarExcepcion
        public async Task<ActionResult> Index()
        {
            List<TipoElector> lista = new List<TipoElector>();
            try
            {
                string token = Session["access_token"].ToString();
                TipoElectorResponse objResponse = await objDAO.ObtenerListaTipoElector(token);
                if (objResponse.estado)
                    lista = objResponse.obj;
                else
                    Request.Flash("danger", objResponse.mensaje);
            }
            catch (Exception ex)
            {
                Request.Flash("danger", ex.Message);
            }
            return View(lista);
        }

        [HttpPost]
        public async Task<ActionResult> Index (int idTipoElector)
        {
            try
            {
                string token = Session["access_token"].ToString();
                int idUsuario = Convert.ToInt32(Session["idUsuario"].ToString());
                string ip = Request.UserHostAddress;
                LogTipoElector obj = new LogTipoElector { idTipoElector = idTipoElector, idUsuario = idUsuario, ip = ip };
                LogResponse logResponse = await logDAO.GuardarLogTipoElector(obj, token);
                if (logResponse.estado)
                {
                    if (idTipoElector == 0)
                    {
                        Session["idTipoElector"] = idTipoElector;
                        return RedirectToAction("SeleccionTipoCargaPapeleta", "VerificarExcepcion");
                    }
                    else
                    {
                        Session["idTipoElector"] = idTipoElector;
                        return RedirectToAction("Index", "DatosUsuario");
                    }
                }
                else
                    Request.Flash("danger", logResponse.mensaje);
            }
            catch (Exception ex)
            {
                Request.Flash("danger", ex.Message);
            }
            return RedirectToAction("Index", "VerificarExcepcion");
        }

        public async Task<ActionResult> SeleccionTipoCargaPapeleta()
        {
            try
            {
                string comprobacionElector = "NO";
                string identificacion = Session["numeroIdentificacion"].ToString().Trim();
                string token = Session["access_token"].ToString();
                InformacionResponse response = await objInformacionDAO.ObtenerInformacionCNE(identificacion, token);
                if(response.estado)
                {
                    if (Session["idTipoElector"].ToString() == ConfigurationManager.AppSettings["ningunaAnteriores"].ToString())
                    {
                        if (response.obj[0].Valor.Trim() != comprobacionElector)
                            return RedirectToAction("Index", "IdentificacionSolicitante");
                        else
                        {
                            if(identificacion == "1715455794" || identificacion == "1715790513" || identificacion == "0706335122" || identificacion == "1710689751") // Se aplica una excepción para estas identificación
                            {
                                return RedirectToAction("Index", "IdentificacionSolicitante");
                            }
                            Request.Flash("danger", "Usted no ha cumplido con su obligación de VOTO , Por favor acercarse al REGISTRO CIVIL para realizar el proceso de manera presencial");
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                        return RedirectToAction("Index", "IdentificacionSolicitante");                                       
                }
                else
                {
                    Request.Flash("danger", "No se encuentran disponibles los servicios de la Dinardap. Por favor, inténtelo en una momentos.");
                    return RedirectToAction("Index", "Home");
                }
                    
            }
            catch (Exception ex)
            {                
                Request.Flash("danger", "No hay conexión con los servicios de la DINARDAP, por favor intentar nuevamente más tarde.");
                return RedirectToAction("Index", "Home");
            }           
           // return View();
        }

        public async Task<ActionResult> CapturaPapeletaArchivo()
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
        public async Task<ActionResult> CapturaPapeletaArchivo(HttpPostedFileBase postedFile)
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
                //RostroResponse objRostro = await rostroDAO.obtenerRostrosPapeletaVotacion(imagen, token);
                //if (objRostro.estado)
                //{
                    OcrResponse objOcr = await ocrDAO.obtenerOcr(imagen, token);
                    if (objOcr.estado)
                    {
                        NumeroIdentifiacionResponse objIdentificacion = await ocrDAO.obtenerNumeroIdentificacionPapeleta(objOcr.obj, token);
                        if (objIdentificacion.estado)
                        {
                            if (objIdentificacion.numeroIdentificacion.Trim() == Session["numeroIdentificacion"].ToString().Trim())
                            {
                                LogPapeleta objPapeleta = new LogPapeleta();
                                objPapeleta.cedulaOcr = objIdentificacion.numeroIdentificacion;
                                objPapeleta.idUsuario = Convert.ToInt32(Session["idUsuario"].ToString());
                                objPapeleta.ipRegistro = Request.UserHostAddress;
                                objPapeleta.contenType = contenType;
                                objPapeleta.extension = extension;
                                foreach (var item in objOcr.obj.analyzeResult.readResults)
                                    foreach (var line in item.lines)
                                        objPapeleta.textOcr += line.text + " - ";

                                IdentificacionResponse objResponse = await identificacionDAO.guardarLogPapeleta(objPapeleta, token, imagen);
                                if (objResponse.estado)
                                    return RedirectToAction("Index", "IdentificacionSolicitante");
                                else
                                    Request.Flash("danger", objResponse.mensajes);
                            }
                            else
                            {
                                Request.Flash("danger", "El número de identificación del documento no coincide con el número de identificación de la papeleta. Por favor, vuelva a cargar el archivo de la papeleta");
                                ApiResponse actualizarIntentos = await authDao.ActualizarNumeroIntentos(objUsr, token);
                            }
                        }
                        else
                        {
                            Request.Flash("danger", "Por favor, cargar un documento con mejor iluminación, de forma horizontal y en un solo documento\n" + objIdentificacion.mensajes);
                            ApiResponse actualizarIntentos = await authDao.ActualizarNumeroIntentos(objUsr, token);
                        }

                    }
                    else
                        Request.Flash("danger", "No ha sido posible extraer la información del documento\n" + objOcr.mensajes);
                //}
                //else
                //{
                //    Request.Flash("danger", objRostro.mensajes + "\nPor favor, cargar un documento de forma horizontal, con buena iluminación y que ocupe todo el archivo.");
                //    ApiResponse actualizarIntentos = await authDao.ActualizarNumeroIntentos(objUsr, token);
                //}

            }
            catch (Exception ex)
            {
                Request.Flash("danger", ex.Message);
            }
            return View();
        }

        public async Task<ActionResult> CapturaPapeletaCamara()
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
        public async Task<JsonResult> CapturaPapeletaCamara(string img)
        {
            bool estado = false;
            string mensaje = "OK";
            try
            {
                byte[] imagen = Transformacion.convertToByte(img);
                string token = Session["access_token"].ToString();
                string contenType = "image/jpg";
                string extension = ".jpg";
                Usuario objUsr = new Usuario { idUsuario = Convert.ToInt32(Session["idUsuario"].ToString()), Identificacion = Session["Identificacion"].ToString() };
                //RostroResponse objRostro = await rostroDAO.obtenerRostrosPapeletaVotacion(imagen, token);
                //if (objRostro.estado)
                //{
                    OcrResponse objOcr = await ocrDAO.obtenerOcr(imagen, token);
                    if (objOcr.estado)
                    {
                        NumeroIdentifiacionResponse objIdentificacion = await ocrDAO.obtenerNumeroIdentificacionPapeleta(objOcr.obj, token);
                        if (objIdentificacion.estado)
                        {
                            if (objIdentificacion.numeroIdentificacion.Trim() == Session["numeroIdentificacion"].ToString().Trim())
                            {
                                LogPapeleta objPapeleta = new LogPapeleta();
                                objPapeleta.cedulaOcr = objIdentificacion.numeroIdentificacion;
                                objPapeleta.idUsuario = Convert.ToInt32(Session["idUsuario"].ToString());
                                objPapeleta.ipRegistro = Request.UserHostAddress;
                                objPapeleta.contenType = contenType;
                                objPapeleta.extension = extension;
                                foreach (var item in objOcr.obj.analyzeResult.readResults)
                                    foreach (var line in item.lines)
                                        objPapeleta.textOcr += line.text + " - ";

                                IdentificacionResponse objResponse = await identificacionDAO.guardarLogPapeleta(objPapeleta, token, imagen);
                                if (objResponse.estado)
                                    estado = true;
                                else
                                    mensaje = "Ha ocurrido un problema al momento procesar su solicitud\n Puede solicitar soporte en el chatbot\n" + objResponse.mensajes;
                            }
                            else
                            {
                                mensaje = "El número de identificación del documento no coincide con el número de identificación de la papeleta. Por favor, vuelva a cargar el archivo de la papeleta";
                                ApiResponse actualizarIntentos = await authDao.ActualizarNumeroIntentos(objUsr, token);
                            }
                        }
                        else
                            mensaje = "No se ha podido detectar el número de cédula, por favor valide que esta no tenga reflejos de luz o brillo que impidan la visualización de su número de cédula.\n " + objIdentificacion.mensajes;
                    }
                    else
                        mensaje = "No se ha podido procesar el texto de la imagen. Por favor valide que esta no tenga reflejos de luz o brillo que impidan la visualización de su cédula.\n " + objOcr.mensajes; 
                //}
                //else
                //{
                //    mensaje = objRostro.mensajes + "\nPor favor, cargar un documento de forma horizontal, con buena iluminación y que ocupe todo el archivo.";
                //    ApiResponse actualizarIntentos = await authDao.ActualizarNumeroIntentos(objUsr, token);
                //}

            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new { estado = estado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult verfificarCodigoDactilar()
        {
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> verfificarCodigoDactilar(string codigoDactilar)
        {
            try
            {
                bool existeCodigoDactilar = false;
                string identificacion = Session["numeroIdentificacion"].ToString().Trim();
                string token = Session["access_token"].ToString();
                string codigoDactilarDinardap = "";
                //int contador = 0;
                //InformacionResponse responseDatosDemograficos;
                ////primera validación a DINARDAP (Se da despues de ingresar el código Dactilar)
                //do {

                //    //temporizados de 3 segundos
                //    Thread.Sleep(3000);
                //    responseDatosDemograficos = await objInformacionDAO.ObtenerInformacionDatosDemograficos(identificacion, token);

                //    //aumento contador
                //    contador += 1;
                //} while (responseDatosDemograficos.estado == true || contador<13);

                InformacionResponse responseDatosDemograficos = await objInformacionDAO.ObtenerInformacionDatosDemograficos(identificacion, token);
                Session["responseDatosDemograficos"] = responseDatosDemograficos;
                //InformacionResponse responseDatosDemograficos = (InformacionResponse)Session["responseDatosDemograficos"];

                if (responseDatosDemograficos.estado)
                {
                    if (responseDatosDemograficos.obj.Count > 0)
                    {
                        foreach (var item in responseDatosDemograficos.obj)
                        {
                            if (item.Nombre.Trim() == "nacionalidad")
                            {
                               if(item.Valor != "ECUATORIANA")
                                {
                                    Request.Flash("danger", "El proceso para personas extranjeras es de forma presencial. Por favor, acercarse a la agencia del Registro Civil más cercana");
                                    return RedirectToAction("Index", "Home");
                                }
                               
                            }
                            
                            if (item.Nombre.Trim() == "fechaNacimiento")
                            {
                                DateTime nacimiento = DateTime.ParseExact(item.Valor, "dd/MM/yyyy", null); //Fecha de nacimiento
                                int edad = DateTime.Today.AddTicks(-nacimiento.Ticks).Year - 1;
                                
                                
                                if(Session["idTipoElector"].ToString() == ConfigurationManager.AppSettings["PersonaMayor"].ToString())
                                {
                                    if(edad < 65)
                                    {
                                        
                                        Request.Flash("Danger", "Usted seleccionó que es persona mayor a 65 años, sin embargo en los registros del Registro Civil Usted nació el "+item.Valor.ToString()+". Usted tiene actualmente "+edad+" años y no es una persona mayor de 65 años. Por favor validar su información personal antes de seguir con el proceso.");
                                        return RedirectToAction("index", "Home");
                                    }
                                }
                                else if (Session["idTipoElector"].ToString() == ConfigurationManager.AppSettings["ningunaAnteriores"].ToString())
                                {
                                    if (edad >= 65)
                                    {
                                        Request.Flash("Danger", "En los registros del Registro Civil Usted nació el '"+item.Valor.ToString()+"'. Usted tiene actualmente '"+edad+" años', por tanto debe seleccionar la opción 'Personas mayores de 65 años' para poder seguir con el proceso.");
                                        return RedirectToAction("index", "Home");
                                    }
                                    else if (Convert.ToBoolean(Session["esDiscapacitado"]) == true)
                                    {
                                        Request.Flash("Danger", "La cédula cargada registra que tiene discapacidad, sin embargo, Usted seleccionó en información persona “Ninguna de las anteriores”. Para poder continuar con el proceso, por favor, debe marcar la opción “Personas con discapacidad”");
                                        return RedirectToAction("index", "Home");
                                    }
                                }
                                else if (Session["idTipoElector"].ToString() == ConfigurationManager.AppSettings["PersonaDiscapacidad"].ToString())
                                {
                                    if (Convert.ToBoolean(Session["esDiscapacitado"]) == false)
                                    {
                                        Request.Flash("Danger", "Usted seleccionó que es persona con discapacidad, sin embargo, Usted en los registros del Registro Civil no tiene esta clasificación. Por favor validar su información personal antes de seguir con el proceso");
                                        return RedirectToAction("index", "Home");
                                    }
                                }
                            }

                            if (item.Nombre.Trim() == "profesion")
                            {
                                string profesion = item.Valor.ToString();
                                if (Session["idTipoElector"].ToString() == ConfigurationManager.AppSettings["ningunaAnteriores"].ToString())
                                {
                                    if(profesion == "MILITAR" || profesion == "POLICIA")
                                    {
                                        Request.Flash("Danger", "Usted ha seleccionado en información personal 'Ninguna de las anteriores' , sin embargo, el Registro Civil indica que Usted pertenece a la Fuerza "+item.Valor.ToString()+". Para poder continuar con el proceso, por favor, debe marcar la opción “Policías y Fuerzas Militares”");
                                        return RedirectToAction("index", "Home");
                                    } 
                                }
                                else if (Session["idTipoElector"].ToString() == ConfigurationManager.AppSettings["codigoMilitar"].ToString())
                                {
                                    if (profesion != "MILITAR" && profesion != "MARINO DE GUERRA" && profesion != "POLICIA")
                                    {
                                        Request.Flash("Danger", "Usted seleccionó que pertenece a Policía o Fuerzas Militar, sin embargo Usted en los registros del Registro Civil no tiene esta clasificación. Por favor validar su información personal antes de seguir con el proceso.");
                                        return RedirectToAction("index", "Home");
                                    }
                                }
                            }   
                            
                            if (item.Nombre.Trim() == "individualDactilar")
                            {
                                string[] domicilio = item.Valor.Split('/');
                                Session["individualDactilar"] = item.Valor.ToString();
                                codigoDactilarDinardap = item.Valor.ToString();
                                existeCodigoDactilar = true;
                            }
                        }
                    }
                    else Request.Flash("danger", "Ha existido un error. Por favor, inténtelo más tarde o puede solicitar ayuda en el chatbot.");
                }

                //valida que el código dactilar ingresado sea igual al devuelto por DINARDAP
                if (existeCodigoDactilar)
                {
                    if (codigoDactilarDinardap.ToUpper() == codigoDactilar.ToUpper())
                        return RedirectToAction("SeleccionTipoCargaPapeleta", "VerificarExcepcion");
                    else
                    {
                        Request.Flash("danger", "El código dactilar ingresado, no coincide con el código dactilar de los registros oficiales. Por favor, inténtelo nuevamente");
                        return RedirectToAction("verfificarCodigoDactilar", "VerificarExcepcion");
                    }
                }
                else
                {
                    Request.Flash("danger", "Ha existido un error al momento de obtener los datos del los registro oficiales. Por Favor, inténtelo más tardes. Si el problema continua, puedo solicitar ayuda a través del chatbot.");
                    return RedirectToAction("Index", "Home");
                }

            }
            catch (Exception ex)
            {
                Request.Flash("danger", ex.ToString());
                return RedirectToAction("Index", "Home");
            }

        }

    }
}