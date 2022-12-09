using SodigCiudadano.Models.Dao;
using SodigCiudadano.Models.Entidades;
using SodigCiudadano.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SodigCiudadano.Controllers
{
    [SecurityFilter]
    public class OCRController : Controller
    {
        AuthDAO objDao = new AuthDAO();
        // GET: OCR
        public async Task<ActionResult> VerificacionCodigo()
        {
            try
            {
                CodigoOTP obj = new CodigoOTP();
                obj.idUsuario = Convert.ToInt32(Session["idUsuario"].ToString());
                obj.correoUsuario = Session["correoBCE"].ToString();
                obj.identificacion = Session["numeroIdentificacion"].ToString();
                string token = Session["access_token"].ToString();
               
                CodigoOTPResponse objCodigoResponse = await objDao.crearCodigoOtp(obj, token);
                if (!objCodigoResponse.estado)
                    Request.Flash("danger", objCodigoResponse.mensaje);
            }
            catch (Exception ex)
            {
                Request.Flash("danger", ex.ToString());
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> VerificacionCodigo(string codigoOtp)
        {
            try
            {
                CodigoOTP obj = new CodigoOTP();
                obj.correoUsuario = Session["correoBCE"].ToString();
                obj.codigoOtpIngresado = codigoOtp;
                obj.identificacion = Session["numeroIdentificacion"].ToString();
                obj.idUsuario = Convert.ToInt32(Session["idUsuario"].ToString());
                obj.ip = Request.UserHostAddress;
                string token = Session["access_token"].ToString();
                CodigoOTPResponse objResponse = await objDao.verificarCodigoOtp(obj, token);
                if (objResponse.estado)
                {
                    int idTipoSolicitud = Convert.ToInt32(Session["idTipoSolicitud"].ToString());
                    if(idTipoSolicitud.ToString() == ConfigurationManager.AppSettings["codigoRevocacion"].ToString() )
                        return RedirectToAction("MotivoRevocacion", "DatosUsuario");
                    else if (idTipoSolicitud.ToString() == ConfigurationManager.AppSettings["codigoRenovacionOlvidoClave"].ToString())
                        return RedirectToAction("FormularioRecuperacionClave", "DatosUsuario");
                    else
                        return RedirectToAction("Index", "PruebaVida");
                }
                   
                else
                    Request.Flash("danger", objResponse.mensaje);
            }
            catch (Exception ex)
            {
                Request.Flash("danger", ex.Message);
            }
            return View();
        }

    }
}