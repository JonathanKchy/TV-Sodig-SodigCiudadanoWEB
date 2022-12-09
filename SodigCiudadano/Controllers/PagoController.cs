using SodigCiudadano.Data;
using SodigCiudadano.Models.Dao;
using SodigCiudadano.Models.Entidades;
using SodigCiudadano.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SodigCiudadano.Controllers
{
    public class PagoController : Controller
    {
        private DatosDAO objDatosDAO = new DatosDAO();
        private PagoDAO objPagoDAO = new PagoDAO();
        private PrecioDAO objPrecioDAO = new PrecioDAO();
        private CatalogoDAO objCatalogoDAO = new CatalogoDAO();
        // GET: Pago
        [SecurityFilter]
        public async Task<ActionResult> Index()
        {
            try
            {
                ViewBag.total = "0";
                ViewBag.SubTotal = "0";
                ViewBag.descripciónProducto = "0";
                ViewBag.checkOut = "";
                ViewBag.urlPago = ConfigurationManager.AppSettings["urlPago"].ToString();
                //cedula de identidad
                string identificacion = Session["numeroIdentificacion"].ToString();
                int idTipoSolicitud = Convert.ToInt32(Session["idTipoSolicitud"].ToString());
                int idTipoContenedor = Convert.ToInt32(Session["idTipoContenedor"].ToString());
                string token = Session["access_token"].ToString();
                DatosResponse objResponse = await objDatosDAO.buscarSolicitud(identificacion, idTipoSolicitud, idTipoContenedor, token);
                if (objResponse.estado)
                {
                    if (objResponse.obj.Solicitud.IdSolicitud == 0)
                        Request.Flash("danger", "Se produjo un error: No existe una solicitud creada");
                    else
                    {
                        Session["idSolicitud"] = objResponse.obj.Solicitud.IdSolicitud.ToString();
                        string nombreProducto = "";


                        //llamado a la base de datos
                        bool respuesta = false;

                        ConexionDB con = new ConexionDB();

                        try
                        {
                            using (SqlConnection conn = new SqlConnection(ConexionDB.rutaConexion))
                            {
                                //SqlCommand cmd = new SqlCommand("usuario.crearUsuario", conn);
                                SqlCommand cmd = new SqlCommand("consulta.Zona6", conn);
                                cmd.Parameters.AddWithValue("identificacion", identificacion);
                                cmd.Parameters.Add("registrado", System.Data.SqlDbType.Bit).Direction = System.Data.ParameterDirection.Output;
                                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                                conn.Open();

                                cmd.ExecuteNonQuery();
                                respuesta = Convert.ToBoolean(cmd.Parameters["registrado"].Value);
                                conn.Close();

                            }
                        }
                        catch (Exception ex)
                        {
                            respuesta=false;
                        }

                        //si existe la cedula en la base de datos
                        if (respuesta) {
                            ViewData["Mensaje"] = "Pertenece al Ministerio de Eduación Zona 6";
                            nombreProducto = ConfigurationManager.AppSettings["nombreEmisionZona6"].ToString();
                        }
                        //pongo el numbre del producto
                        else if (idTipoSolicitud.ToString() == ConfigurationManager.AppSettings["codigoEmision"].ToString())
                        {
                            nombreProducto = ConfigurationManager.AppSettings["nombreEmision"].ToString();
                        }
                        else if (idTipoSolicitud.ToString() == ConfigurationManager.AppSettings["codigoRenovacion"].ToString())
                        {
                            nombreProducto = ConfigurationManager.AppSettings["nombreRenovacion"].ToString();
                        }
                        else if (idTipoSolicitud.ToString() == ConfigurationManager.AppSettings["codigoRenovacionOlvidoClave"].ToString())
                        {
                            nombreProducto = ConfigurationManager.AppSettings["nombreRenovacionOlvidoClave"].ToString();
                        }



                        PrecioReponse precio = await objPrecioDAO.ObtenerPrecioProducto(nombreProducto, token);
                        if (precio.estado)
                        {
                            CheckOutEnvio objCheck = new CheckOutEnvio();
                            objCheck.amount = Math.Round(precio.obj.item.vm_precioventa, 2).ToString();
                            objCheck.givenName = objResponse.obj.Cliente.PrimerNombre;
                            objCheck.middleName = objResponse.obj.Cliente.SegundoNombre;
                            objCheck.surname = objResponse.obj.Cliente.PrimerApellido + " " + objResponse.obj.Cliente.SegundoApellido;
                            objCheck.ip = Request.UserHostAddress;
                            objCheck.merchantCustomerId = Session["idUsuario"].ToString();
                            NumeroTransaccionResponse numeroTransaccionResponse =  await objCatalogoDAO.ObtenerNumeroTransaccion(token);
                            if (!numeroTransaccionResponse.estado)
                                objCheck.merchantTransactionId = objResponse.obj.Solicitud.IdSolicitud.ToString();
                            else
                                objCheck.merchantTransactionId = numeroTransaccionResponse.numeroTransaccion.ToString() ;
                            objCheck.email = objResponse.obj.Cliente.Telefonos[2].Contacto;
                            objCheck.identificationDocId = objResponse.obj.Cliente.Identificacion;
                            objCheck.phone = objResponse.obj.Cliente.Telefonos[1].Contacto;
                            objCheck.street1 = objResponse.obj.Cliente.Direcciones[0].direccion;
                            objCheck.country = "EC";
                            PaisDAO paisDAO = new PaisDAO();
                            var responsePais = await paisDAO.ObtenerPaisPorId(objResponse.obj.Cliente.Direcciones[0].IdPais, token);
                            if (responsePais.estado)
                                objCheck.country = responsePais.obj.codigoPais;
                            objCheck.postcode = objResponse.obj.Cliente.Direcciones[0].codigoPostal;
                            objCheck.SHOPPER_VAL_BASE0 = "0";
                            objCheck.SHOPPER_VAL_BASEIMP = Math.Round(precio.obj.item.vm_preciolista, 2).ToString();
                            objCheck.SHOPPER_VAL_IVA = Math.Round(precio.obj.item.vm_valoriva, 2).ToString();
                            objCheck.name = nombreProducto;
                            objCheck.description = nombreProducto;
                            objCheck.quantity = "1";
                            objCheck.price = Math.Round(precio.obj.item.vm_precioventa, 2).ToString();
                            objCheck.SHOPPER_VERSIONDF = "2";
                            ViewBag.total = objCheck.price;
                            ViewBag.SubTotal = objCheck.SHOPPER_VAL_BASEIMP;
                            ViewBag.descripciónProducto = nombreProducto;
                            CheckOutResponse res = await objPagoDAO.generarCheckOutIdFase2(objCheck, identificacion, token);
                            if (res.estado)
                            {
                                if (!string.IsNullOrEmpty(res.id))
                                    ViewBag.checkOut = res.id;
                                else
                                    Request.Flash("danger", res.result.description);
                            }
                            else
                                Request.Flash("danger", res.mensaje);
                        }
                        else
                            Request.Flash("danger", precio.mensajes);
                    }
                }
                else Request.Flash("danger", objResponse.mensajes);
            }
            catch (Exception ex)
            {
                Request.Flash("danger", ex.Message);
            }

            return View();
        }

        [SecurityFilter]
        [HttpPost]
        public async Task<ActionResult> Pago(FormCollection form)
        {
            try
            {
                string token = Session["access_token"].ToString();
                Pago obj = new Pago();
                obj.fecha = DateTime.Now;
                obj.fechaAutorizacion = DateTime.Now;
                obj.idSolicitud = Convert.ToInt32(Session["idSolicitud"].ToString());
                obj.sha = "código Sha Prueba";
                obj.status = "Approved";
                obj.trankey = "{ 'status': { 'status': 'approved', 'message': 'test' } }";
                obj.ip = Request.UserHostAddress;
                PagoResponse objResponse = await objPagoDAO.insertarPago(obj, token);
                if (objResponse.estado)
                    return RedirectToAction("Redireccion", "Pago");
                else Request.Flash("danger", objResponse.mensajes);
            }
            catch (Exception ex)
            {
                Request.Flash("danger", ex.Message);
            }
            return RedirectToAction("Index", "Pago");
        }

        [SecurityFilter]
        public async Task<ActionResult> Redireccion()
        {
            try
            {
                ViewBag.precio = "";
                ViewBag.showMessage = false;
                ViewBag.nombreProceso = "";
                string token = Session["access_token"].ToString();
                int idTipoSolicitud = Convert.ToInt32(Session["idTipoSolicitud"].ToString());
                string nombreProceso = "";

                if (idTipoSolicitud.ToString() == ConfigurationManager.AppSettings["codigoEmision"].ToString())
                    nombreProceso = ConfigurationManager.AppSettings["nombreProcesoEmision"].ToString();
                else if (idTipoSolicitud.ToString() == ConfigurationManager.AppSettings["codigoRenovacion"].ToString())
                    nombreProceso = ConfigurationManager.AppSettings["nombreProcesoRenovacion"].ToString();
                else if (idTipoSolicitud.ToString() == ConfigurationManager.AppSettings["codigoRenovacionOlvidoClave"].ToString())
                {
                    string nombreProducto = "";
                    //llamado a la base de datos
                    bool respuesta = false;
                    string identificacion = Session["numeroIdentificacion"].ToString();
                    ConexionDB con = new ConexionDB();

                    try
                    {
                        using (SqlConnection conn = new SqlConnection(ConexionDB.rutaConexion))
                        {
                            //SqlCommand cmd = new SqlCommand("usuario.crearUsuario", conn);
                            SqlCommand cmd = new SqlCommand("consulta.Zona6", conn);
                            cmd.Parameters.AddWithValue("identificacion", identificacion);
                            cmd.Parameters.Add("registrado", System.Data.SqlDbType.Bit).Direction = System.Data.ParameterDirection.Output;
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;

                            conn.Open();

                            cmd.ExecuteNonQuery();
                            respuesta = Convert.ToBoolean(cmd.Parameters["registrado"].Value);
                            conn.Close();

                        }
                    }
                    catch (Exception ex)
                    {
                        respuesta = false;
                    }

                    //si existe la cedula en la base de datos
                    if (respuesta)
                    {
                        ViewData["Mensaje"] = "Pertenece al Ministerio de Eduación Zona 6";
                        nombreProducto = ConfigurationManager.AppSettings["nombreEmisionZona6"].ToString();
                    }
                    //pongo el numbre del producto
                    else if (idTipoSolicitud.ToString() == ConfigurationManager.AppSettings["codigoEmision"].ToString())
                    {
                        nombreProducto = ConfigurationManager.AppSettings["nombreEmision"].ToString();
                    }
                    else if (idTipoSolicitud.ToString() == ConfigurationManager.AppSettings["codigoRenovacion"].ToString())
                    {
                        nombreProducto = ConfigurationManager.AppSettings["nombreRenovacion"].ToString();
                    }
                    else if (idTipoSolicitud.ToString() == ConfigurationManager.AppSettings["codigoRenovacionOlvidoClave"].ToString())
                    {
                        nombreProducto = ConfigurationManager.AppSettings["nombreRenovacionOlvidoClave"].ToString();
                    }

                    PrecioReponse precio = await objPrecioDAO.ObtenerPrecioProducto(nombreProducto, token);
                    ViewBag.showMessage = true;
                    ViewBag.precio = Math.Round(precio.obj.item.vm_preciolista, 2).ToString();
                    nombreProceso = ConfigurationManager.AppSettings["nombreProcesoRenovacionOlvidoClave"].ToString();
                }                    
                else if (idTipoSolicitud.ToString() == ConfigurationManager.AppSettings["codigoRevocacion"].ToString())
                    nombreProceso = ConfigurationManager.AppSettings["nombreProcesoRevocacion"].ToString();




                ViewBag.nombreProceso = nombreProceso;
            }
            catch (Exception)
            {

                throw;
            }
            return View();
        }


        //factura
        [SecurityFilter]
        public async Task<ActionResult> Pago(string resourcePath)
        {
            try
            {
                string identificacion = Session["numeroIdentificacion"].ToString();
                int idUsuario = Convert.ToInt32(Session["idUsuario"].ToString());
                string token = Session["access_token"].ToString();
                string correo = Session["correoCliente"].ToString();
                var pagoResponse = await objPagoDAO.ProcesarPago(resourcePath, identificacion, idUsuario, token, correo);
                if (pagoResponse.estado)
                {
                    if (pagoResponse.result.code == ConfigurationManager.AppSettings["codigoPagoExitoso"].ToString())
                    {
                        int idTipoSolicitud = Convert.ToInt32(Session["idTipoSolicitud"].ToString());
                        int idTipoContenedor = Convert.ToInt32(Session["idTipoContenedor"].ToString());
                        DatosResponse objResponseSolicitud = await objDatosDAO.buscarSolicitud(identificacion, idTipoSolicitud, idTipoContenedor, token);
                        
                        if (objResponseSolicitud.estado)
                        {
                            string nombreProducto = "";


                            //llamado a la base de datos
                            bool respuesta = false;

                            ConexionDB con = new ConexionDB();

                            try
                            {
                                using (SqlConnection conn = new SqlConnection(ConexionDB.rutaConexion))
                                {
                                    //SqlCommand cmd = new SqlCommand("usuario.crearUsuario", conn);
                                    SqlCommand cmd = new SqlCommand("consulta.Zona6", conn);
                                    cmd.Parameters.AddWithValue("identificacion", identificacion);
                                    cmd.Parameters.Add("registrado", System.Data.SqlDbType.Bit).Direction = System.Data.ParameterDirection.Output;
                                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                                    conn.Open();

                                    cmd.ExecuteNonQuery();
                                    respuesta = Convert.ToBoolean(cmd.Parameters["registrado"].Value);
                                    conn.Close();

                                }
                            }
                            catch (Exception ex)
                            {
                                respuesta = false;
                            }

                            //si existe la cedula en la base de datos
                            if (respuesta)
                            {
                                ViewData["Mensaje"] = "Pertenece al Ministerio de Eduación Zona 6";
                                nombreProducto = ConfigurationManager.AppSettings["nombreEmisionZona6"].ToString();
                            }
                            //pongo el numbre del producto
                            else if (idTipoSolicitud.ToString() == ConfigurationManager.AppSettings["codigoEmision"].ToString())
                            {
                                nombreProducto = ConfigurationManager.AppSettings["nombreEmision"].ToString();
                            }
                            else if (idTipoSolicitud.ToString() == ConfigurationManager.AppSettings["codigoRenovacion"].ToString())
                            {
                                nombreProducto = ConfigurationManager.AppSettings["nombreRenovacion"].ToString();
                            }
                            else if (idTipoSolicitud.ToString() == ConfigurationManager.AppSettings["codigoRenovacionOlvidoClave"].ToString())
                            {
                                nombreProducto = ConfigurationManager.AppSettings["nombreRenovacionOlvidoClave"].ToString();
                            }

                            PrecioReponse precio = await objPrecioDAO.ObtenerPrecioProducto(nombreProducto, token);
                            if (precio.estado)
                            {
                                #region Factura
                                Factura objFactura = new Factura();
                                string primerNombre = objResponseSolicitud.obj.Cliente.PrimerNombre;
                                string segundoNombre = objResponseSolicitud.obj.Cliente.SegundoNombre;
                                string primerApellido = objResponseSolicitud.obj.Cliente.PrimerApellido;
                                string segundoApellido = objResponseSolicitud.obj.Cliente.SegundoApellido;
                                objFactura.nombreCliente = objResponseSolicitud.obj.Solicitud.nombreFactura;
                                objFactura.mail = objResponseSolicitud.obj.Cliente.Telefonos[2].Contacto;
                                objFactura.direccion = objResponseSolicitud.obj.Cliente.Direcciones[0].direccion + ", " + objResponseSolicitud.obj.Cliente.Direcciones[0].Sector;
                                objFactura.identificacionCliente = objResponseSolicitud.obj.Solicitud.identificacionFactura;
                                objFactura.idTipoCredito = ConfigurationManager.AppSettings["idTipoCredito"].ToString();
                                objFactura.fechaEmision = DateTime.Now.ToString("dd/MM/yyyy");
                                objFactura.fechaValidez = "31/12/" + DateTime.Now.Year;
                                objFactura.txtTipoCambio = ConfigurationManager.AppSettings["txtTipoCambio"].ToString();
                                objFactura.txtPorcGrav = ConfigurationManager.AppSettings["txtPorcGrav"].ToString();
                                objFactura.ddlid_ice = ConfigurationManager.AppSettings["ddlid_ice"].ToString();
                                objFactura.idTipoDocumento = ConfigurationManager.AppSettings["idTipoDocumento"].ToString();
                                objFactura.ddlEstado = "";
                                objFactura.txtComentario = "";
                                objFactura.txtDiasGracia = "";
                                objFactura.txtDiasCredito = "";
                                objFactura.txtDiasProrroga = "";
                                objFactura.txtPorcIntCorr = "";
                                objFactura.txtPorcIntMora = "";
                                objFactura.compraReembolso = false;
                                objFactura.idPuntoVenta = Convert.ToUInt32(ConfigurationManager.AppSettings["idPuntoVenta"].ToString());
                                objFactura.idEmpresa = Convert.ToUInt32(ConfigurationManager.AppSettings["idEmpresa"].ToString());
                                objFactura.idUsuario = Convert.ToInt32(ConfigurationManager.AppSettings["idUsuario"].ToString());
                                objFactura.idTipoMoneda = Convert.ToUInt32(ConfigurationManager.AppSettings["idTipoMoneda"].ToString());
                                objFactura.idTipoCambio = Convert.ToUInt32(ConfigurationManager.AppSettings["idTipoCambio"].ToString());
                                objFactura.idVendedor = Convert.ToUInt32(ConfigurationManager.AppSettings["idVendedor"].ToString());
                                objFactura.obra = "";
                                objFactura.subtotal_gravado = Convert.ToDouble(Math.Round(precio.obj.item.vm_costo, 2));
                                objFactura._subtotal_exento = 0;
                                objFactura._vm_descuento = 0;
                                objFactura._vm_subtotal_desc_gravado = 0;
                                objFactura._vm_subtotal_desc_exento = 0;
                                objFactura._vm_subtotal_desc = Convert.ToDouble(Math.Round(precio.obj.item.vm_costo, 2));
                                objFactura._vm_iva = Convert.ToDouble(Math.Round(precio.obj.item.vm_valoriva, 2));
                                objFactura._vm_ice = 0;
                                objFactura._vm_subtotalice = 0;
                                objFactura._vm_total = Convert.ToDouble(Math.Round(precio.obj.item.vm_precioventa, 2));
                                objFactura._qn_peso = 0;
                                objFactura._qn_volumen = 0;
                                objFactura.items = new List<Items>();
                                Items item = new Items();
                                item.qn_item = 1;
                                item.tx_valorcompleto = precio.obj.descripcion.id_iteminterno + precio.obj.descripcion.tx_descripcion;
                                item.bd_tipitem = "S";
                                item.id = Convert.ToInt32(precio.obj.descripcion.id_item);
                                item.id_item = precio.obj.descripcion.id_iteminterno;
                                item.tx_descripcion = precio.obj.descripcion.tx_descripcion;
                                item.qn_cantidad = 1;
                                item.qn_cantaent = 0;
                                item.qn_cantent = 0;
                                item.qn_cantxent = 0;
                                item.id_unidad = 1;
                                item.qn_peso = 0;
                                item.qn_volumen = 0;
                                item.vm_costo = 0;
                                item.id_unidadinv = 1;
                                item.bd_muldiv = "M";
                                item.qn_factor = 1;
                                item.vm_preciorig = Convert.ToDecimal(Math.Round(precio.obj.item.vm_costo, 2));
                                item.qn_desc = 0;
                                item.qn_porcdescitem = 0;
                                item.vm_descuento = 0;
                                item.vm_preciodesc = Convert.ToDecimal(Math.Round(precio.obj.item.vm_costo, 2));
                                item.vm_subtotal = Convert.ToDecimal(Math.Round(precio.obj.item.vm_costo, 2));
                                item.qn_porcivaorig = Convert.ToDecimal(Math.Round(precio.obj.item.qn_porciva, 2));
                                item.vm_valoriva = Convert.ToDecimal(Math.Round(precio.obj.item.vm_valoriva, 2));
                                item.qn_porcice = Convert.ToDecimal(Math.Round(precio.obj.item.qn_porcice, 2));
                                item.vm_valorice = Convert.ToDecimal(Math.Round(precio.obj.item.vm_valorice, 2));
                                item.vm_total = Convert.ToDecimal(Math.Round(precio.obj.item.vm_precioventa, 2));
                                item.bd_itemrelacionado = "N";
                                item.bd_itempromocionado = "N";
                                item.bd_itempromocion = "N";
                                item.bd_modificar = "S";
                                item.qn_itemrelacionado = 0;
                                item.qn_itempromo = 0;

                                objFactura.items.Add(item);

                                objFactura.listaTipoPago = new List<TipoPagoItems>();
                                TipoPagoItems tipoPago = new TipoPagoItems();
                                tipoPago.id_tipopago = ConfigurationManager.AppSettings["id_tipopago"].ToString();
                                tipoPago.tx_tipopago = ConfigurationManager.AppSettings["tx_tipopago"].ToString();
                                tipoPago.tx_enumerador = ConfigurationManager.AppSettings["tx_enumerador"].ToString();
                                tipoPago.fe_emision = DateTime.Now.ToString("dd/MM/yyyy");
                                tipoPago.vm_valor = Convert.ToDecimal(Math.Round(precio.obj.item.vm_precioventa, 2)).ToString();
                                tipoPago.id_ctactb = ConfigurationManager.AppSettings["id_ctactb"].ToString();
                                tipoPago.tx_descripcion = ConfigurationManager.AppSettings["tx_descripcion"].ToString();
                                tipoPago.id_control = ConfigurationManager.AppSettings["id_control"].ToString();
                                objFactura.listaTipoPago.Add(tipoPago);

                                #endregion

                                FacturaResponse facturaResponse = await objPagoDAO.generarFactura(objFactura, token);
                                if (facturaResponse.estado)
                                {
                                    BCEDAO objBancoDAO = new BCEDAO();
                                    RegistroPagoEnvio objEnvio = new RegistroPagoEnvio();
                                    objEnvio.fechaFactura = DateTime.Now;
                                    objEnvio.codigoSolicitud = Convert.ToInt32(objResponseSolicitud.obj.Solicitud.codigoSolicitud);
                                    objEnvio.identificacion = objResponseSolicitud.obj.Cliente.Identificacion;
                                    objEnvio.idUsuario = Convert.ToInt32(Session["idUsuario"].ToString());
                                    objEnvio.numeroAsientoContable = facturaResponse.asientoContable;
                                    objEnvio.numeroFactura = facturaResponse.numeroFactura;
                                    objEnvio.observacion = "N/A";
                                    objEnvio.valorFactura = Convert.ToDecimal(Math.Round(precio.obj.item.vm_precioventa, 2));
                                    objEnvio.ipRegistro = Request.UserHostAddress;
                                    RegistroPagoResponse registroPagoResponse = await objBancoDAO.RegistroPago(objEnvio, token);
                                    if (registroPagoResponse.estado)
                                    {
                                        if (registroPagoResponse.obj.banderaPago)
                                        {
                                            Pago obj = new Pago();
                                            obj.fecha = DateTime.Now;
                                            obj.fechaAutorizacion = DateTime.Now;
                                            obj.idSolicitud = Convert.ToInt32(Session["idSolicitud"].ToString());
                                            obj.sha = facturaResponse.asientoContable;
                                            obj.status = registroPagoResponse.obj.Serial.ToString();
                                            obj.trankey = pagoResponse.ToString();
                                            obj.ip = Request.UserHostAddress;
                                            obj.numeroFactura = facturaResponse.numeroFactura;
                                            obj.valor = Convert.ToDouble(Math.Round(precio.obj.item.vm_precioventa, 2)).ToString();
                                            PagoResponse objResponse = await objPagoDAO.insertarPago(obj, token);
                                            if (objResponse.estado)
                                            {
                                                string nombreProceso = "";
                                                if (idTipoSolicitud.ToString() == ConfigurationManager.AppSettings["codigoEmision"].ToString())
                                                    nombreProceso = ConfigurationManager.AppSettings["nombreProcesoEmision"].ToString();
                                                else if (idTipoSolicitud.ToString() == ConfigurationManager.AppSettings["codigoRenovacion"].ToString())
                                                    nombreProceso = ConfigurationManager.AppSettings["nombreProcesoRenovacion"].ToString();
                                                else if (idTipoSolicitud.ToString() == ConfigurationManager.AppSettings["codigoRenovacionOlvidoClave"].ToString())
                                                    nombreProceso = ConfigurationManager.AppSettings["nombreProcesoRenovacionOlvidoClave"].ToString();
                                                else if (idTipoSolicitud.ToString() == ConfigurationManager.AppSettings["codigoRevocacion"].ToString())
                                                    nombreProceso = ConfigurationManager.AppSettings["nombreProcesoRevocacion"].ToString();

                                                PagoCorreo objEnvioCorreo = new PagoCorreo();
                                                objEnvioCorreo.correoEnvio = correo;
                                                objEnvioCorreo.identificacion = identificacion;
                                                objEnvioCorreo.nombreProceso = nombreProceso;
                                                objEnvioCorreo.nombreUsuario = primerNombre + " " + segundoNombre + " " + primerApellido + " " + segundoApellido;
                                                PagoResponse responseCorreo = await objPagoDAO.enviarCorreProcesoCulminadoo(objEnvioCorreo, token);

                                                return RedirectToAction("Redireccion", "Pago");
                                            }
                                            else {
                                             //  bool anulacionPago = await objPagoDAO.AnularPago(pagoResponse.id, pagoResponse.amount, identificacion, idUsuario, token);
                                                Request.Flash("danger", objResponse.mensajes);
                                                return RedirectToAction("mensajeError", "Pago");
                                            } 
                                        }
                                        else
                                        {
                                            bool anulacionPago = await objPagoDAO.AnularPago(pagoResponse.id, pagoResponse.amount, identificacion, idUsuario, token);
                                            Request.Flash("danger", registroPagoResponse.obj.mensajeError);
                                            return RedirectToAction("mensajeError", "Pago");
                                        }
                                            

                                    }
                                    else
                                    {
                                        bool anulacionPago = await objPagoDAO.AnularPago(pagoResponse.id, pagoResponse.amount, identificacion, idUsuario, token);
                                        Request.Flash("danger", registroPagoResponse.obj.mensajeError);
                                        Request.Flash("danger", registroPagoResponse.mensaje);
                                        return RedirectToAction("mensajeError", "Pago");
                                    }
                                        
                                }
                                else
                                {
                                    bool anulacionPago = await objPagoDAO.AnularPago(pagoResponse.id, pagoResponse.amount, identificacion, idUsuario, token);
                                    Request.Flash("danger", facturaResponse.mensajes);
                                    return RedirectToAction("mensajeError", "Pago");
                                }
                            }
                            else
                            {
                                bool anulacionPago = await objPagoDAO.AnularPago(pagoResponse.id, pagoResponse.amount, identificacion, idUsuario, token);
                                Request.Flash("danger", precio.mensajes);
                                return RedirectToAction("mensajeError", "Pago");
                            }
                        }
                        else
                        {
                            bool anulacionPago = await objPagoDAO.AnularPago(pagoResponse.id, pagoResponse.amount, identificacion, idUsuario, token);
                            Request.Flash("danger", objResponseSolicitud.mensajes);
                            return RedirectToAction("mensajeError", "Pago");
                        }
                    }
                    else
                    {
                        PagoCorreo obj = new PagoCorreo();
                        obj.correoEnvio = correo;
                        obj.identificacion = identificacion;
                        obj.motivo = pagoResponse.result.code + " " + pagoResponse.result.description;
                        PagoResponse responseCorreo = await objPagoDAO.EnviarCorreoPago(obj, token);
                        PagoCodigoResponse codigoResponse = await objPagoDAO.ObtenerRespuestaPorCodigo(pagoResponse.result.code, token);
                        if (!string.IsNullOrEmpty(codigoResponse.obj.descripcion))
                            Request.Flash("danger", "No ha sido posible procesar el pago: " + codigoResponse.obj.descripcion);
                        else
                            Request.Flash("danger", "No ha sido posible procesar el pago: " + pagoResponse.result.description);
                    }

                }
                else
                    Request.Flash("danger", pagoResponse.result.description);

            }
            catch (Exception ex)
            {
                Request.Flash("danger", ex.Message);
            }
            return RedirectToAction("Index", "Pago");
        }

        [SecurityFilter]
        public async Task<ActionResult> PagoRenovacionPrimeraVez()
        {
            try
            {
                string identificacion = Session["numeroIdentificacion"].ToString();
                int idUsuario = Convert.ToInt32(Session["idUsuario"].ToString());
                string token = Session["access_token"].ToString();
                string correo = Session["correoCliente"].ToString();

                int idTipoSolicitud = Convert.ToInt32(Session["idTipoSolicitud"].ToString());
                int idTipoContenedor = Convert.ToInt32(Session["idTipoContenedor"].ToString());
                DatosResponse objResponseSolicitud = await objDatosDAO.buscarSolicitud(identificacion, idTipoSolicitud, idTipoContenedor, token);
                if (objResponseSolicitud.estado)
                {

                    BCEDAO objBancoDAO = new BCEDAO();
                    RegistroPagoEnvio objEnvio = new RegistroPagoEnvio();
                    objEnvio.fechaFactura = DateTime.Now;
                    objEnvio.codigoSolicitud = Convert.ToInt32(objResponseSolicitud.obj.Solicitud.codigoSolicitud);
                    objEnvio.identificacion = objResponseSolicitud.obj.Cliente.Identificacion;
                    objEnvio.idUsuario = Convert.ToInt32(Session["idUsuario"].ToString());
                    objEnvio.numeroAsientoContable = "00000000010";// facturaResponse.asientoContable;
                    objEnvio.numeroFactura = "00000000010";// facturaResponse.numeroFactura;
                    objEnvio.observacion = "N/A";
                    objEnvio.valorFactura = 0;// Convert.ToDecimal(Math.Round(precio.obj.item.vm_precioventa, 2));
                    objEnvio.ipRegistro = Request.UserHostAddress;
                    RegistroPagoResponse registroPagoResponse = await objBancoDAO.RegistroPago(objEnvio, token);
                    if (registroPagoResponse.estado)
                    {
                        if (registroPagoResponse.obj.banderaPago)
                        {
                            Pago obj = new Pago();
                            obj.fecha = DateTime.Now;
                            obj.fechaAutorizacion = DateTime.Now;
                            obj.idSolicitud = objResponseSolicitud.obj.Solicitud.IdSolicitud;// Convert.ToInt32(Session["idSolicitud"].ToString());
                            obj.sha = "00000000010";//;facturaResponse.asientoContable;
                            obj.status = registroPagoResponse.obj.Serial.ToString();
                            obj.trankey = "Sin Pago Primera Vez";
                            obj.ip = Request.UserHostAddress;
                            obj.numeroFactura = "00000000010";// facturaResponse.numeroFactura;
                            obj.valor = "0";// Convert.ToDouble(Math.Round(precio.obj.item.vm_precioventa, 2)).ToString();
                            PagoResponse objResponse = await objPagoDAO.insertarPago(obj, token);
                            if (objResponse.estado)
                                return RedirectToAction("Redireccion", "Pago");
                            else Request.Flash("danger", objResponse.mensajes);
                        }
                        else
                            Request.Flash("danger", registroPagoResponse.obj.mensajeError);

                    }
                    else
                        Request.Flash("danger", registroPagoResponse.mensaje);

                }
                else
                {
                    Request.Flash("danger", objResponseSolicitud.mensajes);
                }

            }
            catch (Exception ex)
            {
                Request.Flash("danger", ex.ToString());
            }
            return RedirectToAction("Redireccion", "Pago");
        }

        [SecurityFilter]
        public ActionResult mensajeError()
        {
            return View();
        }
    }
}