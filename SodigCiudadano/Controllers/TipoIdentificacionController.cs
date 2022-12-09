using SodigCiudadano.Models.Dao;
using SodigCiudadano.Models.Entidades;
using SodigCiudadano.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SodigCiudadano.Controllers
{
    [SecurityFilter]
    public class TipoIdentificacionController : Controller
    {
        private TipoIdentificacionDAO tipoidentificacionDao = new TipoIdentificacionDAO();
        private LogDAO logDao = new LogDAO();

        // GET: TipoIdentificacion
        public async Task<ActionResult> Index(string msg = "")
        {
            try
            {
                if (!string.IsNullOrEmpty(msg))
                    Request.Flash("danger", msg);
                string token = Session["access_token"].ToString();
                int idTipoPersona = Convert.ToInt32(Session["idTipoPersona"].ToString());
                ViewBag.idTipoIdentificacion = new SelectList(new List<TipoIdentificacion>());
                
                   TipoIdentificacionResponse respuesta = await tipoidentificacionDao.ObtenerListaTipoidentificacionPorIdPersona(idTipoPersona, token);
                if (respuesta.estado)
                    ViewBag.idTipoIdentificacion = new SelectList(respuesta.obj, "idTipoIdentificacion", "TipoIdentificacion");
                else
                    Request.Flash("danger", "Ha ocurrido un error al momento de recuperar los tipos de documentos");
            }
            catch (Exception ex)
            {
                ViewBag.idTipoIdentificacion = new SelectList(new List<TipoIdentificacion>());
                Request.Flash("danger", ex.Message);
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(FormCollection formulario)
        {            
            try
            {
                int idUsuario = Convert.ToInt32(Session["idUsuario"].ToString());
                string token = Session["access_token"].ToString();
                int idTipoIdentificacion = Convert.ToInt32(formulario["idTipoIdentificacion"].ToString());
                string ti = idTipoIdentificacion.ToString();
                Session["idTipoIdentificacion"] = idTipoIdentificacion.ToString();
                string ip = Request.UserHostAddress;
                LogTipoIdentificacion obj = new LogTipoIdentificacion { idTipoIdentificacion = idTipoIdentificacion, idUsuario = idUsuario, ip = ip };
                LogResponse logResponse = await logDao.GuardarLogTipoIdentificacion(obj, token);
                if (logResponse.estado)
                    return RedirectToAction("Index", "CapturaDocumento", new { ti = ti }); // ti = idTipoDocumento
                else
                    Request.Flash("danger", logResponse.mensaje);
                
            }
            catch (Exception ex)
            {
                Request.Flash("danger", ex.Message);
            }
            return RedirectToAction("Index", "TipoIdentificacion");
        }
    }
}