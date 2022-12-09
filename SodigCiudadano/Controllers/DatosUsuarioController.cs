using SodigCiudadano.Models.Dao;
using SodigCiudadano.Models.Entidades;
using SodigCiudadano.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SodigCiudadano.Controllers
{
    [SecurityFilter]
    public class DatosUsuarioController : Controller
    {
        private PaisDAO objPaisDAO = new PaisDAO();
        private ProvinciaDAO objProvinciaDAO = new ProvinciaDAO();
        private CiudadDAO objCiudadDAO = new CiudadDAO();
        private ActividadEconomicaDAO objActividadDAO = new ActividadEconomicaDAO();
        private UsoCertificadoDAO objUsoDAO = new UsoCertificadoDAO();
        private DatosDAO objDatosDAO = new DatosDAO();
        private InformacionDAO objInformacionDAO = new InformacionDAO();
        private TipoIdentificacionDAO tipoidentificacionDao = new TipoIdentificacionDAO();
        private BCEDAO objBceDao = new BCEDAO();
        private AuthDAO authDao = new AuthDAO();
        private Util.Transformacion util = new Util.Transformacion();

        // GET: DatosUsuario
        public async Task<ActionResult> Index()
        {
            try
            {
                string token = Session["access_token"].ToString();

                int idUsuario = Convert.ToInt32(Session["idUsuario"].ToString());
                string identificacion = Session["numeroIdentificacion"].ToString().Trim();
                NumeroIntentosResponse intentos = await authDao.ObtenerNumeroIntentosPorIdUsuario(idUsuario, identificacion, token);
                if (intentos.estado)
                {
                    //if (intentos.obj.numeroIntentos >= Convert.ToInt32(ConfigurationManager.AppSettings["numeroIntentosPermitidos"].ToString()))
                    //{
                    //    Request.Flash("danger", "Ha superado el número máximo de intentos. Por favor, agende una cita");
                    //    return RedirectToAction("Index", "Ticket");
                    //}
                    //else
                    //{
                        //Session["direccion"] = "";
                        //Session["sector"] = "";
                        int idTipoElector = Convert.ToInt32(Session["idTipoElector"].ToString());
                        string comprobacionElector = "NO";
                        if (idTipoElector == Convert.ToInt32(ConfigurationManager.AppSettings["codigoTipoElectorVotante"].ToString()))
                            comprobacionElector = "SI";



                        string idTipoIdentificacion = Session["idTipoIdentificacion"].ToString();
                        int idTipoPersona = Convert.ToInt32(Session["idTipoPersona"].ToString());
                        var lista = new SelectList(new List<TipoIdentificacion>());
                        TipoIdentificacionResponse respuesta = await tipoidentificacionDao.ObtenerListaTipoidentificacionPorIdPersona(idTipoPersona, token);
                        if (respuesta.estado)
                        {
                            lista = new SelectList(respuesta.obj, "idTipoIdentificacion", "TipoIdentificacion", idTipoIdentificacion);
                            foreach (var item in lista)
                                if (item.Value == idTipoIdentificacion)
                                    Session["tipoIdentificacion"] = item.Text;
                        }
                        else
                        {
                            Request.Flash("danger", "Ha ocurrido un error al momento de recuperar el tipo de documento");
                            return View();
                        }


                        //InformacionResponse response = await objInformacionDAO.ObtenerInformacionCNE(identificacion, token);
                        //if (response.estado || idTipoElector.ToString() != ConfigurationManager.AppSettings["codigoTipoElectorVotante"].ToString())
                        //{
                        //    //if (response.obj == null || response.obj.Count > 0)
                        //    //{
                        //    //    if (response.obj != null)
                        //    //    {
                        //    //        bool activarComprobacion = response.verificacionVotante;
                        //    //        if (activarComprobacion)
                        //    //        {
                        //    //            if (response.obj[0].Valor.Trim() != comprobacionElector)
                        //    //            {
                        //    //                Request.Flash("danger", "Los datos ingresados no coinciden con los datos registrados por el CNE.");
                        //    //                return RedirectToAction("Index", "Home");
                        //    //            }
                        //    //        }
                        //    //    }



                        //    //    //InformacionResponse responseDatosDemograficos = await objInformacionDAO.ObtenerInformacionDatosDemograficos(identificacion, token);
                        //    //    //if (responseDatosDemograficos.estado)
                        //    //    //{
                        //    //    //    if (responseDatosDemograficos.obj.Count > 0)
                        //    //    //    {
                        //    //    //        foreach (var item in responseDatosDemograficos.obj)
                        //    //    //        {
                        //    //    //            if (item.Nombre.Trim() == "domicilio")
                        //    //    //            {
                        //    //    //                string[] domicilio = item.Valor.Split('/');
                        //    //    //                if (domicilio.Length > 2)
                        //    //    //                {
                        //    //    //                    Session["direccion"] = domicilio[0];
                        //    //    //                    Session["sector"] = domicilio[1];
                        //    //    //                }
                        //    //    //            }

                        //    //    //            if (item.Nombre.Trim() == "nombre".Trim())
                        //    //    //            {
                        //    //    //                string nombre = item.Valor;
                        //    //    //                NombreApellido objNombres = new NombreApellido();
                        //    //    //                var nombres = objNombres.GetNombreApellido(nombre);
                        //    //    //                Session["primerApellido"] = nombres.primerApellido;
                        //    //    //                Session["segundoApellido"] = nombres.segundoApellido;
                        //    //    //                Session["primerNombre"] = nombres.primerNombre;
                        //    //    //                Session["segundoNombre"] = nombres.segundoNombre;
                        //    //    //            }
                        //    //    //        }
                        //    //    //    }
                        //    //    //    else Request.Flash("danger", "Ha existido un error. Por favor, inténtelo más tarde. obtener los datos demográficos del CNE.");
                        //    //    //}
                        //    //    //else Request.Flash("danger", responseDatosDemograficos.mensajes);
                        //    //}
                        //    //else Request.Flash("danger", "Ha existido un error. Por favor, inténtelo más tarde. obtener los datos del CNE.");
                        //}
                        //else Request.Flash("danger", response.mensajes);
                   // }
                }
                else
                    Request.Flash("danger", "Ha ocurrido un error al momento de recuperar el número de intentos " + intentos.mensajes);

            }
            catch (Exception ex)
            {
                Request.Flash("danger", ex.Message);
            }

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(FormCollection form)
        {
            // return RedirectToAction("DatosUsuario", "DatosUsuario");
            try
            {
                RegistroSolicitanteEnvio obj = new RegistroSolicitanteEnvio();
                obj.apellidoPaterno = form["apellidoPaterno"].TrimStart().TrimEnd();
                obj.apellidoMaterno = form["apellidoMaterno"].TrimStart().TrimEnd();
                string primerNombre = form["primerNombre"].TrimStart().TrimEnd();
                string segundoNombre = form["segundoNombre"].TrimEnd().TrimStart();
                Session["primerApellido"] = obj.apellidoPaterno;
                Session["segundoApellido"] = obj.apellidoMaterno;
                Session["primerNombre"] = primerNombre;
                Session["segundoNombre"] = segundoNombre;
                obj.cedula = form["cedula"];
                obj.nombres = primerNombre + " "+ segundoNombre;
                int numeroNombre = Convert.ToInt32(Session["numeroNombres"].ToString());
                if(numeroNombre<4)
                {
                    string nombreCompleto = obj.apellidoPaterno + obj.apellidoMaterno + primerNombre + segundoNombre;
                    nombreCompleto = nombreCompleto.Replace(" ", string.Empty).ToUpper();
                    string nombreDinardap = Session["nombreDinardap"].ToString().Replace(" ", string.Empty).ToUpper();
                    if(nombreCompleto != nombreDinardap)
                    {
                        Request.Flash("danger", "Los nombres ingresados no coinciden con los del registro civil");
                        return RedirectToAction("Index", "DatosUsuario");
                    }    
                }
                obj.idUsuario = Convert.ToInt32(Session["idUsuario"].ToString());
                obj.tipoIdentificacion = Convert.ToInt32(Session["idTipoidentificacion"].ToString());
                obj.ipRegistro = Request.UserHostAddress;
                string token = Session["access_token"].ToString();
                int idTipoSolicitud = Convert.ToInt32(Session["idTipoSolicitud"].ToString());
                if (idTipoSolicitud != Convert.ToInt32(ConfigurationManager.AppSettings["codigoEmision"].ToString()))
                {
                    Session["codigoSolicitante"] = Session["codigoSolicitanteBCE"].ToString();
                    return RedirectToAction("DatosUsuario", "DatosUsuario");
                }
                else
                {
                    obj.apellidoPaterno = util.reemplazarCaracteres(obj.apellidoPaterno);
                    obj.apellidoMaterno = util.reemplazarCaracteres(obj.apellidoMaterno);
                    obj.nombres = util.reemplazarCaracteres(obj.nombres);
                    obj.nombre = Session["nombreComparacion"].ToString();
                     obj.banderaNombre = Convert.ToBoolean(Session["banderaNombre"].ToString());
                    RegistroSolicitanteResponse objResponse = await objBceDao.RegistroSolicitante(obj, token);
                    if (objResponse.estado)
                    {
                        if (objResponse.obj.banderaResultadoSolicitante || objResponse.obj.codigoRespuesta == Convert.ToInt32(ConfigurationManager.AppSettings["codigoSolicitanteRegistrado"].ToString()))
                        {
                            Session["codigoSolicitante"] = objResponse.obj.codigoSolicitante;
                            return RedirectToAction("DatosUsuario", "DatosUsuario");
                        }
                        else
                        {
                            Request.Flash("danger", objResponse.obj.mensajeError);
                            Usuario objUsr = new Usuario { idUsuario = Convert.ToInt32(Session["idUsuario"].ToString()), Identificacion = Session["Identificacion"].ToString() };
                            ApiResponse actualizarIntentos = await authDao.ActualizarNumeroIntentos(objUsr, token);
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                        Request.Flash("danger", objResponse.mensaje);
                }
            }
            catch (Exception ex)
            {
                Request.Flash("danger", ex.Message);
            }
            return RedirectToAction("Index", "DatosUsuario");
        }

        public ActionResult Redireccion()
        {
            Request.Flash("warning", "El proceso ha finalizado.");
            return RedirectToAction("Index", "Home");
        }

        public async Task<JsonResult> ObtenerListaProvincias(string idPais)
        {
            bool estado = false;
            string mensaje = "OK";
            List<Provincia> lista = new List<Provincia>();
            ProvinciaResponse objProvinciaResponse;
            try
            {
                string token = Session["access_token"].ToString();
                objProvinciaResponse = await objProvinciaDAO.ObtenerProvinciaPorIdPais(idPais, token);
                if (objProvinciaResponse.estado)
                {
                    lista = objProvinciaResponse.lista;
                    estado = true;
                }
                else
                    mensaje = objProvinciaResponse.mensajes;
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new { estado = estado, lista = lista, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> ObtenerListaCiudades(string idProvincia)
        {
            bool estado = false;
            string mensaje = "OK";
            List<Ciudad> lista = new List<Ciudad>();
            CiudadResponse objCiudadResponse;
            try
            {
                string token = Session["access_token"].ToString();
                objCiudadResponse = await objCiudadDAO.ObtenerCiudadPorIdProvincia(idProvincia, token);
                if (objCiudadResponse.estado)
                {
                    lista = objCiudadResponse.lista;
                    estado = true;
                }
                else
                    mensaje = objCiudadResponse.mensajes;
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new { estado = estado, lista = lista, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> DatosUsuario()
        {
            Datos objDatos = new Datos();
            #region ViewBag
            ViewBag.idPais = new List<Pais>();
            ViewBag.idProvincia = new List<Provincia>();
            ViewBag.idCiudad = new List<Ciudad>();
            ViewBag.idActividadEconomica = new List<ActividadEconomica>();
            ViewBag.IdUsoCertificado = new List<UsoCertificado>();
            #endregion
            try
            {
                ViewBag.correo = "";
                ViewBag.correoAlternativo = "";
                ViewBag.codigoTelefono = "02";
                ViewBag.codigoPaisEcuador = ConfigurationManager.AppSettings["codigoPaisEcuador"].ToString();
                ViewBag.codigoOtroUsoCertificado = ConfigurationManager.AppSettings["codigoOtroUsoCertificado"].ToString();
                string identificacion = Session["numeroIdentificacion"].ToString();
                int idTipoSolicitud = Convert.ToInt32(Session["idTipoSolicitud"].ToString());
                int idTipoContenedor = Convert.ToInt32(Session["idTipoContenedor"].ToString());
                string token = Session["access_token"].ToString();

                if (string.IsNullOrEmpty(Session["correoCliente"].ToString()))
                {
                    Request.Flash("danger", "La sesión ha caducado, Por favor, Inténtelo de nuevo");
                    return RedirectToAction("Logout", "Auth");
                }
                ViewBag.correo = Session["correoCliente"].ToString();
                ViewBag.correoAlternativo = Session["correoCliente"].ToString();

                int idUsuario = Convert.ToInt32(Session["idUsuario"].ToString());
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
                        PaisResponse paisResponse = await objPaisDAO.ObtenerListaPaises(token);
                        if (paisResponse.estado)
                        {
                            ActividadEconomicaResponse actResponse = await objActividadDAO.ObtenerListaPaises(token);
                            if (actResponse.estado)
                            {
                                ProvinciaResponse acProv = await objProvinciaDAO.ObtenerListaProvincias(token);
                                if (acProv.estado)
                                {
                                    CiudadResponse resCiudad = await objCiudadDAO.ObtenerListaCiudades(token);
                                    if (resCiudad.estado)
                                    {
                                        UsoCertificadoResponse usoResponse = await objUsoDAO.ObtenerListaUsoCertificado(token);
                                        if (usoResponse.estado)
                                        {

                                            DatosResponse objResponse = await objDatosDAO.buscarSolicitud(identificacion, idTipoSolicitud, idTipoContenedor, token);
                                            if (objResponse.estado)
                                            {
                                                if (objResponse.obj.Cliente == null)
                                                {
                                                    string codigoPaisEcuador = ConfigurationManager.AppSettings["codigoPaisEcuador"].ToString();
                                                    string codigoProvincia = "17";
                                                    string codigoCiudad = "170";

                                                    objDatos.Cliente = new Cliente();
                                                    string direccion = Session["direccion"].ToString();
                                                    string sector = Session["sector"].ToString();
                                                    if(string.IsNullOrEmpty(direccion))
                                                    {
                                                        direccion = "PICHINCHA";
                                                        sector = "QUITO";
                                                    }
                                                   
                                                    ProvinciaResponse pResponse = await objProvinciaDAO.ObtenerListaProvinciasPorNombre(direccion, token);
                                                    if (pResponse.estado)
                                                        if (pResponse.lista.Count > 0)
                                                        {
                                                            ViewBag.codigoTelefono = pResponse.lista[0].codigoProvincia;
                                                            codigoProvincia = pResponse.lista[0].idProvincia;
                                                        }


                                                    CiudadResponse cResponse = await objCiudadDAO.ObtenerCiudadPorNombreCiudad(sector, token);
                                                    if (cResponse.estado)
                                                        if (cResponse.lista.Count > 0)
                                                            codigoCiudad = cResponse.lista[0].idCiudad;

                                                    Direccion objDireccion = new Direccion { IdProvincia = codigoProvincia, IdPais = codigoPaisEcuador, IdCiudad = codigoCiudad, direccion = "", Sector = "" };
                                                    Direccion objDireccion2 = new Direccion { IdProvincia = codigoProvincia, IdPais = codigoPaisEcuador, IdCiudad = codigoCiudad };
                                                    List<Direccion> listaDir = new List<Direccion>();
                                                    listaDir.Add(objDireccion);
                                                    listaDir.Add(objDireccion2);
                                                    objDatos.Cliente.Direcciones = listaDir;
                                                }
                                                else objDatos = objResponse.obj;
                                                ViewBag.idPais = paisResponse.lista;
                                                ViewBag.idProvincia = acProv.lista;
                                                ViewBag.idCiudad = resCiudad.lista;
                                                ViewBag.idActividadEconomica = actResponse.lista;
                                                if (objResponse.obj.Solicitud == null)
                                                {
                                                    string primerApellido =  Session["primerApellido"].ToString();
                                                    string segundoApellido = Session["segundoApellido"].ToString();
                                                    string primerNombre = Session["primerNombre"].ToString();                                                    
                                                    string segundoNombre = "";
                                                    if (!string.IsNullOrEmpty(Session["segundoNombre"].ToString())) {
                                                        segundoNombre = Session["segundoNombre"].ToString();
                                                    }                                                    
                                                    objDatos.Solicitud = new Solicitud();
                                                    objDatos.Solicitud.identificacionFactura = Session["numeroIdentificacion"].ToString();
                                                    objDatos.Solicitud.nombreFactura = primerApellido + " " + segundoApellido + " " + primerNombre + " " + segundoNombre;
                                                    ViewBag.IdUsoCertificado = new SelectList(usoResponse.lista, "IdUsoCertificado", "usoCertificado");
                                                }
                                                else
                                                    ViewBag.IdUsoCertificado = new SelectList(usoResponse.lista, "IdUsoCertificado", "usoCertificado", objResponse.obj.Solicitud.IdUsoCertificado);
                                            }
                                            else Request.Flash("danger", objResponse.mensajes);
                                        }
                                        else Request.Flash("danger", usoResponse.mensajes);
                                    }
                                    else Request.Flash("danger", resCiudad.mensajes);
                                }
                                else Request.Flash("danger", acProv.mensajes);
                            }
                            else Request.Flash("danger", actResponse.mensajes);
                        }
                        else Request.Flash("danger", paisResponse.mensajes);
                    }
                }
                else
                    Request.Flash("danger", "Ha ocurrido un error al momento de recuperar el número de intentos " + intentos.mensajes);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    Request.Flash("danger", ex.InnerException.ToString());
                else
                    Request.Flash("danger", ex.Message);

                return RedirectToAction("Index", "Home");
                //#region Excepcion
                //objDatos.Cliente = new Cliente();
                //Direccion objDireccion = new Direccion { IdProvincia = "0", IdPais = "0", IdCiudad = "0" };
                //Direccion objDireccion2 = new Direccion { IdProvincia = "0", IdPais = "0", IdCiudad = "0" };
                //List<Direccion> listaDir = new List<Direccion>();
                //listaDir.Add(objDireccion);
                //listaDir.Add(objDireccion2);
                //objDatos.Cliente.Direcciones = listaDir;
                //#endregion
            }

            return View(objDatos);
        }

        [HttpPost]
        public async Task<ActionResult> DatosUsuario(Datos obj, string[] idUsoCertificado)
        {
            try
            {
                Session["numeroInterntosRenovacionOlvidoClave"] = "";
                string token = Session["access_token"].ToString();
                obj.Solicitud.IdUsoCertificado = idUsoCertificado[0];
                if (obj.Solicitud.IdSolicitud == 0)
                {
                    #region Solicitud                    
                    obj.Solicitud.IdTipoSolicitud = Convert.ToInt32(Session["idTipoSolicitud"].ToString());
                    obj.Solicitud.Identificacion = Session["numeroIdentificacion"].ToString();
                    obj.Solicitud.IdTipoContenedor = Convert.ToInt32(Session["idTipoContenedor"].ToString());
                    obj.Solicitud.IdMotivoSolicitud = 1;
                    if (obj.Solicitud.IdTipoSolicitud.ToString() == ConfigurationManager.AppSettings["codigoEmision"].ToString())
                        obj.Solicitud.IdEstado = Convert.ToInt32(ConfigurationManager.AppSettings["codigoEstadoAprobado"].ToString());
                    else if (obj.Solicitud.IdTipoSolicitud.ToString() == ConfigurationManager.AppSettings["codigoRenovacion"].ToString())
                        obj.Solicitud.IdEstado = Convert.ToInt32(ConfigurationManager.AppSettings["codigoEstadoRenovado"].ToString());
                    else if (obj.Solicitud.IdTipoSolicitud.ToString() == ConfigurationManager.AppSettings["codigoRenovacionOlvidoClave"].ToString())
                        obj.Solicitud.IdEstado = Convert.ToInt32(ConfigurationManager.AppSettings["codigoEstadoRenovado"].ToString());
                    obj.Solicitud.CodigoSolicitante = Session["codigoSolicitante"].ToString();
                    obj.Solicitud.Fecha = DateTime.Now;
                    obj.Solicitud.Ip = Request.UserHostAddress;
                    obj.Solicitud.IdUsoCertificado = idUsoCertificado[0];
                    #endregion
                }


                if (string.IsNullOrEmpty(obj.Cliente.Identificacion))
                {
                    #region Cliente   
                    obj.Cliente.Identificacion = Session["numeroIdentificacion"].ToString();
                    obj.Cliente.IdTipoIdentificacion = Convert.ToInt32(Session["idTipoIdentificacion"].ToString());
                    obj.Cliente.PrimerApellido = Session["primerApellido"].ToString();
                    obj.Cliente.SegundoApellido = Session["segundoApellido"].ToString();
                    obj.Cliente.PrimerNombre = Session["primerNombre"].ToString();
                    obj.Cliente.SegundoNombre = Session["segundoNombre"].ToString();
                    obj.Cliente.IdTipoElector = Convert.ToInt32(Session["idTipoElector"].ToString());
                    obj.Cliente.Elector = false;
                    if (obj.Cliente.IdTipoElector == 0)
                        obj.Cliente.Elector = true;
                    #endregion
                    obj.Cliente.Actividad.IdActividad = 0;
                    #region Teléfonos
                    obj.Cliente.Telefonos[0].IdTipoContacto = 1;
                    obj.Cliente.Telefonos[0].IdCategoria = 1;
                    obj.Cliente.Telefonos[1].IdTipoContacto = 2;
                    obj.Cliente.Telefonos[1].IdCategoria = 1;
                    obj.Cliente.Telefonos[2].IdTipoContacto = 3;
                    obj.Cliente.Telefonos[2].IdCategoria = 1;
                    obj.Cliente.Telefonos[3].IdTipoContacto = 3;
                    obj.Cliente.Telefonos[3].IdCategoria = 3;
                    obj.Cliente.Telefonos[4].IdTipoContacto = 1;
                    obj.Cliente.Telefonos[4].IdCategoria = 2;
                    obj.Cliente.Telefonos[5].IdTipoContacto = 4;
                    obj.Cliente.Telefonos[5].IdCategoria = 2;
                    #endregion
                    #region Direcciones
                    obj.Cliente.Direcciones[0].IdCategoria = 1;
                    obj.Cliente.Direcciones[1].IdCategoria = 2;
                    #endregion
                }


                #region ObjetoParaConsumoSevicioBCE
                ActividadEconomicaResponse actResponse = await objActividadDAO.ObtenerListaPaises(token);
                RegistroSolicitudEnvio objEnvio = new RegistroSolicitudEnvio();
                if (actResponse.estado)
                {
                    foreach (var item in actResponse.lista)
                    {
                        if (item.idActividadEconomica == obj.Cliente.Actividad.IdActividadEconomica)
                        {
                            objEnvio.idActividadEconomica = item.idActividadEconomica;
                            objEnvio.actividadEconomica = item.actividadEconomica;
                            break;
                        }
                    }
                    if (Session["idTipoSolicitud"].ToString() != ConfigurationManager.AppSettings["codigoEmision"].ToString())
                    {
                        objEnvio.codSolicitud = Convert.ToInt32(Session["codigoSolicitudBCE"].ToString());
                        objEnvio.numeroSerieCertificado = Session["numeroSerieSolicitud"].ToString();
                    }
                    objEnvio.tipoSolicitud = Convert.ToInt32(Session["idTipoPersona"].ToString());
                    objEnvio.estadoSolicitud = Convert.ToInt32(ConfigurationManager.AppSettings["codigoEstadoAprobado"].ToString());
                    objEnvio.motivoSolicitud = Convert.ToInt32(Session["idTipoSolicitud"].ToString());
                    objEnvio.tipoContenedorCertificado = Convert.ToInt32(Session["idTipoContenedor"].ToString());
                    objEnvio.estadoRecuperacion = false;
                    List<int> listaUso = new List<int>();
                    foreach (var item in idUsoCertificado)
                    {
                        listaUso.Add(Convert.ToInt32(item));
                    }
                    
                    objEnvio.usoCertificadoDigital = listaUso;
                    objEnvio.txtOtroUso = obj.Solicitud.otroUso;
                    objEnvio.oficinaTerceroVinculado = Convert.ToInt32(ConfigurationManager.AppSettings["codigoOficinaTV"].ToString());
                    objEnvio.codigoSolicitante = Convert.ToInt32(Session["codigoSolicitante"].ToString());
                    objEnvio.rucSolicitante = obj.Cliente.Actividad.Ruc;
                    objEnvio.rupSolicitante = obj.Cliente.Actividad.Rup;
                    objEnvio.celularSolicitante = obj.Cliente.Telefonos[1].Contacto;
                    objEnvio.telefonoDomicilioSolicitante = obj.Cliente.Telefonos[0].Contacto;
                    objEnvio.mailSolicitante = obj.Cliente.Telefonos[2].Contacto;
                    objEnvio.mailAlternoSolicitante = obj.Cliente.Telefonos[3].Contacto;
                    objEnvio.paisDomicilioSolicitante = obj.Cliente.Direcciones[0].IdPais;
                    if (objEnvio.paisDomicilioSolicitante == ConfigurationManager.AppSettings["codigoPaisEcuador"].ToString())
                    {
                        objEnvio.provinciaSolicitante = obj.Cliente.Direcciones[0].IdProvincia;
                        objEnvio.ciudadSolicitante = obj.Cliente.Direcciones[0].IdCiudad.ToString();
                        objEnvio.parroquiaSolicitante = obj.Cliente.Direcciones[0].Sector;
                    }
                    else
                    {
                        objEnvio.estadoDomicilioSolicitante = obj.Cliente.Direcciones[0].direccion;
                        objEnvio.estadoOficinaSolicitante = obj.Cliente.Direcciones[1].direccion;
                        objEnvio.ciudadDomicilioSolicitante = obj.Cliente.Direcciones[0].Sector;
                        objEnvio.ciudadOficinaSolicitante = obj.Cliente.Direcciones[1].Sector;
                    }
                    objEnvio.direccionSolicitante = obj.Cliente.Direcciones[0].direccion;
                    objEnvio.paisOficinaSolicitante = Convert.ToInt32(obj.Cliente.Direcciones[1].IdPais);
                    objEnvio.provinciaOficinaSolicitante = Convert.ToInt32(obj.Cliente.Direcciones[1].IdProvincia);
                    objEnvio.ciudadOficinaSolicitante = obj.Cliente.Direcciones[1].IdCiudad;
                    objEnvio.direccionOficinaSolitante = obj.Cliente.Direcciones[1].direccion;
                    objEnvio.telefonoOficinaSolicitante = obj.Cliente.Telefonos[4].Contacto;
                    objEnvio.extensionOficinaSolicitante = obj.Cliente.Telefonos[4].Extension;
                    if (Session["idTipoElector"].ToString() == ConfigurationManager.AppSettings["codigoTipoElectorVotante"].ToString())
                        objEnvio.estadoPapeletaVotacion = 1;
                    else
                    {
                        objEnvio.estadoPapeletaVotacion = 1;
                        objEnvio.exepcionesPapeletaVotacion = Convert.ToInt32(Session["idTipoElector"].ToString());
                    }
                    objEnvio.porcentajeCoincidencia = Convert.ToDouble(Session["porcentajeDocumento"].ToString());
                    objEnvio.archivoFoto = new List<byte>((byte[])Session["imagenDocumento"]);
                    objEnvio.archivoFotoBase64 = Session["imagenBase64Dinardap"].ToString();
                    //   objEnvio.archivoFoto1 =  (byte)Session["imagenDocumento"]; 

                    objEnvio.idUsuario = Convert.ToInt32(Session["idUsuario"].ToString());
                    objEnvio.ipRegistro = Request.UserHostAddress;
                    objEnvio.identificacion = Session["numeroIdentificacion"].ToString();
                }
                else
                {
                    Request.Flash("danger", actResponse.mensajes);
                    return RedirectToAction("DatosUsuario", "DatosUsuario");
                }

                #endregion
                Usuario objUsr = new Usuario { idUsuario = Convert.ToInt32(Session["idUsuario"].ToString()), Identificacion = Session["Identificacion"].ToString() };
                objEnvio.parroquiaSolicitante = util.reemplazarCaracteres(objEnvio.parroquiaSolicitante);
                objEnvio.direccionSolicitante = util.reemplazarCaracteres(objEnvio.direccionSolicitante);
                objEnvio.direccionOficinaSolitante = util.reemplazarCaracteres(objEnvio.direccionOficinaSolitante);

                int codigoSolicitud = 524086;
                if (Session["idTipoSolicitud"].ToString() == ConfigurationManager.AppSettings["codigoRenovacion"].ToString())
                {
                    RegistroSolicitudRenovacionResponse registroSolicitudRenovacionResponse = await objBceDao.RegistroSolicitudRenovacion(objEnvio, token);
                    if (!registroSolicitudRenovacionResponse.estado)
                    {
                        Request.Flash("danger", registroSolicitudRenovacionResponse.mensaje);
                        ApiResponse actualizarIntentos = await authDao.ActualizarNumeroIntentos(objUsr, token);
                        return RedirectToAction("DatosUsuario", "DatosUsuario");
                    }
                    else
                    {
                        if (registroSolicitudRenovacionResponse.obj.banderaSolicitudRenovacion)
                        {
                            codigoSolicitud = registroSolicitudRenovacionResponse.obj.codigoSolicitudRenovacion;
                        }
                        else
                        {
                            Request.Flash("danger", registroSolicitudRenovacionResponse.obj.mensajeSolicitudRenovacion);
                            return RedirectToAction("DatosUsuario", "DatosUsuario");
                        }
                    }

                }
                else if (Session["idTipoSolicitud"].ToString() == ConfigurationManager.AppSettings["codigoRenovacionOlvidoClave"].ToString())
                {

                    RegistroOlvidoClaveEnvio objClave = new RegistroOlvidoClaveEnvio();
                    objClave.codigoSolicitud = Convert.ToInt32(Session["codigoSolicitudBCE"].ToString());
                    objClave.identificacion = Session["numeroIdentificacion"].ToString();
                    objClave.idUsuario = Convert.ToInt32(Session["idUsuario"].ToString());
                    objClave.ipRegistro = Request.UserHostAddress;
                    objClave.numeroSerieCertificado = Session["numeroSerieSolicitud"].ToString();
                    RegistroOlvidoClaveResponse responseClave = await objBceDao.RegistroSolicitudOlvidoClave(objClave, token);
                    if (responseClave.estado)
                    {
                        if (responseClave.obj.banderaSolicitudOlvidoClave)
                        {
                            Session["numeroInterntosRenovacionOlvidoClave"] = responseClave.obj.numeroIntentosOlvidoClave;
                            codigoSolicitud = responseClave.obj.codigoSolicitudOlvidoClave;
                        }
                        else
                        {
                            Request.Flash("danger", responseClave.obj.mensajeError);
                          //  ApiResponse actualizarIntentos = await authDao.ActualizarNumeroIntentos(objUsr, token);
                            return RedirectToAction("DatosUsuario", "DatosUsuario");
                        }
                    }
                    else
                    {
                        Request.Flash("danger", responseClave.mensaje);
                        return RedirectToAction("DatosUsuario", "DatosUsuario");
                    }
                }
                else
                {
                    RegistroSolicitudResponse registroSolicitudResponse = await objBceDao.RegistroSolicitud(objEnvio, token);
                    if (!registroSolicitudResponse.estado)
                    {
                        Request.Flash("danger", registroSolicitudResponse.mensaje);
                      //  ApiResponse actualizarIntentos = await authDao.ActualizarNumeroIntentos(objUsr, token);
                        return RedirectToAction("DatosUsuario", "DatosUsuario");
                    }
                    else
                    {
                        if (registroSolicitudResponse.obj.banderaResultadoSolicitud)
                            codigoSolicitud = registroSolicitudResponse.obj.codigoSolicitud;
                        else
                        {
                            Request.Flash("danger", registroSolicitudResponse.obj.mensajeError);
                          //  ApiResponse actualizarIntentos = await authDao.ActualizarNumeroIntentos(objUsr, token);
                            return RedirectToAction("DatosUsuario", "DatosUsuario");
                        }
                    }
                }

                if (Session["numeroInterntosRenovacionOlvidoClave"].ToString() == ConfigurationManager.AppSettings["numeroPago"].ToString())
                    obj.Solicitud.IdEstado = Convert.ToInt32(ConfigurationManager.AppSettings["idEstadoFinalizado"].ToString());

                DatosResponse respuesta = new DatosResponse();
                if (obj.Solicitud.IdSolicitud == 0)
                {
                    obj.Solicitud.codigoSolicitud = codigoSolicitud.ToString();
                    respuesta = await objDatosDAO.insertarDatosSolicitud(obj, token);
                }
                else
                    respuesta = await objDatosDAO.actualizarDatosSolicitud(obj, token);
                if (respuesta.estado)
                {
                    if (Session["numeroInterntosRenovacionOlvidoClave"].ToString() == ConfigurationManager.AppSettings["numeroPago"].ToString())
                        return RedirectToAction("PagoRenovacionPrimeraVez", "Pago");
                    else
                        return RedirectToAction("Index", "Pago");
                }
                else
                    Request.Flash("danger", respuesta.mensajes);
            }
            catch (Exception ex)
            {
                Request.Flash("danger", ex.Message);
            }
            return RedirectToAction("DatosUsuario", "DatosUsuario");
        }

        public async Task<ActionResult> MotivoRevocacion()
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
                        MotivoRevocacionDAO objMotivoDao = new MotivoRevocacionDAO();
                        ViewBag.idMotivo = new SelectList(new List<MotivoRevocacion>());
                        ViewBag.codigoSolicitanteBCE = Session["codigoSolicitanteBCE"].ToString();
                        ViewBag.codigoSolicitudBCE = Session["codigoSolicitudBCE"].ToString();
                        MotivoRevocacionResponse response = await objMotivoDao.ObtenerListaMotivoRenovacion(token);
                        if (response.estado)
                            ViewBag.idMotivo = new SelectList(response.obj, "idMotivoSolicitud", "motivoSolicitud");
                        else Request.Flash("danger", "Ha ocurrido un problema al cargar los motivos. " + response.mensaje);
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
        public async Task<ActionResult> MotivoRevocacion(int idMotivo, string codigoSolicitud)
        {
            try
            {

                string token = Session["access_token"].ToString();
                RegistroSolicitudRevocacionEnvio objEnvio = new RegistroSolicitudRevocacionEnvio();
                objEnvio.codSolicitud = Convert.ToInt32(codigoSolicitud);
                objEnvio.identificacion = Session["numeroIdentificacion"].ToString();
                objEnvio.idUsuario = Convert.ToInt32(Session["idUsuario"].ToString());
                objEnvio.ipRegistro = Request.UserHostAddress;
                objEnvio.motivoRevocacion = idMotivo;
                objEnvio.motivoSolicitud = Convert.ToInt32(Session["idTipoSolicitud"].ToString());
                Usuario objUsr = new Usuario { idUsuario = Convert.ToInt32(Session["idUsuario"].ToString()), Identificacion = Session["Identificacion"].ToString() };
                RegistroSolicitudRevocacionResponse response = await objBceDao.RegistroSolicitudRevocacion(objEnvio, token);
                if (response.estado)
                {
                    if (response.obj.banderaSolicitudRevocacion)
                    {
                        Solicitud objSolicitud = new Solicitud();
                        objSolicitud.CodigoSolicitante = Session["codigoSolicitanteBCE"].ToString();
                        objSolicitud.codigoSolicitud = codigoSolicitud;
                        objSolicitud.Fecha = DateTime.Now;
                        objSolicitud.Identificacion = Session["numeroIdentificacion"].ToString();
                        objSolicitud.IdEstado = Convert.ToInt32(ConfigurationManager.AppSettings["idEstadoRevocacion"].ToString());
                        objSolicitud.IdMotivoSolicitud = idMotivo;
                        objSolicitud.IdTipoContenedor = Convert.ToInt32(Session["idTipoContenedor"].ToString());
                        objSolicitud.IdTipoSolicitud = Convert.ToInt32(Session["idTipoSolicitud"].ToString());
                        objSolicitud.IdUsoCertificado = ConfigurationManager.AppSettings["codigoOtroUSo"].ToString();
                        objSolicitud.Ip = Request.UserHostAddress;
                        objSolicitud.otroUso = "";
                        DatosResponse datosResponse = await objDatosDAO.crearSolicitudRevocacion(objSolicitud, token);
                        if (datosResponse.estado)
                            return RedirectToAction("Redireccion", "Pago");
                        else
                            Request.Flash("danger", datosResponse.mensajes);
                    }
                    else
                    {
                        ApiResponse actualizarIntentos = await authDao.ActualizarNumeroIntentos(objUsr, token);
                        Request.Flash("danger", response.obj.mensajeError);
                    }

                }
                else
                    Request.Flash("danger", response.mensaje);
            }
            catch (Exception ex)
            {
                Request.Flash("danger", ex.ToString());
            }
            return RedirectToAction("MotivoRevocacion", "DatosUsuario");
        }

        public async Task<ActionResult> FormularioRecuperacionClave()
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
                        ViewBag.idMotivo = new SelectList(new List<MotivoRevocacion>());
                        ViewBag.codigoSolicitanteBCE = Session["codigoSolicitanteBCE"].ToString();
                        ViewBag.codigoSolicitudBCE = Session["codigoSolicitudBCE"].ToString();
                    }
                }
                else
                    Request.Flash("danger", "Ha ocurrido un error al momento de recuperar el número de intentos " + intentos.mensajes);

            }
            catch (Exception ex)
            {
                Request.Flash("danger", ex.ToString());
            }
            return  RedirectToAction("Index", "PruebaVida");
        }

        [HttpPost]
        public async Task<ActionResult> FormularioRecuperacionClave(FormCollection form)
        {
            //try
            //{

            //}
            //catch (Exception ex)
            //{
            //    Request.Flash("danger", ex.ToString());
            //}
            return RedirectToAction("Index", "PruebaVida");
        }

    }
}