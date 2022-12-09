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
    public class TipoContenedorController : Controller
    {
        private TipoContenedorDAO tipoContenedorDao = new TipoContenedorDAO();
        private LogDAO logDao = new LogDAO();

        // GET: TipoContenedor
        public async Task<ActionResult> Index()
        {
            try
            {
                string token = Session["access_token"].ToString();
                ViewBag.idTipoContenedor = new SelectList(new List<TipoContenedor>(), "idTipoContenedor", "TipoContenedor");
                TipoContenedorResponse respuesta = await tipoContenedorDao.ObtenerListaTipoContenedor(token);
                if (respuesta.estado)
                    ViewBag.idTipoContenedor = new SelectList(respuesta.obj, "idTipoContenedor", "TipoContenedor");     
                else
                    Request.Flash("danger", "Ha ocurrido un error al momento de recuperar los tipo de contenedores");
            }
            catch (Exception ex)
            {
                ViewBag.idTipoContenedor = new SelectList(new List<TipoContenedor>(), "idTipoContenedor", "TipoContenedor");
                Request.Flash("danger", ex.Message);
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(FormCollection formulario)
        {
            try
            {
                string token = Session["access_token"].ToString();
                int idUsuario = Convert.ToInt32(Session["idUsuario"].ToString());
                int idTipoContenedor = Convert.ToInt32(formulario["idTipoContenedor"].ToString());
                Session["idTipoContenedor"] = idTipoContenedor.ToString();
                string ip = Request.UserHostAddress;
                LogTipoContenedor obj = new LogTipoContenedor { idTipoContenedor = idTipoContenedor, idUsuario = idUsuario, ip = ip };
                LogResponse logResponse = await logDao.GuardarLogTipoContenedor(obj, token);
                if (logResponse.estado)
                    return RedirectToAction("Index", "TipoIdentificacion");
                else
                    Request.Flash("danger", logResponse.mensaje);
            }
            catch (Exception ex)
            {
                Request.Flash("danger", ex.Message);
            }
            return RedirectToAction("Index", "TipoContenedor");
        }
    }
}