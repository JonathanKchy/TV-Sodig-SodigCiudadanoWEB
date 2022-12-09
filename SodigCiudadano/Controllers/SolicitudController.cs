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
    public class SolicitudController : Controller
    {
        private DatosDAO objDatosDAO = new DatosDAO();
        private TipoSolicitudDAO tipoSolicitudDao = new TipoSolicitudDAO();
        private TipoPersonaDAO tipoPersonaDao = new TipoPersonaDAO();

        // GET: Solicitud
        public async Task<ActionResult> SolicitudesAprobadas()
        {
            List<SolicitudAprobada> lista = new List<SolicitudAprobada>();
            ViewBag.idTipoSolicitud = new SelectList(new List<TipoSolicitud>());
            ViewBag.idTipoPersona = new SelectList(new List<TipoPersona>());
            try
            {
                string token = Session["access_token"].ToString();
                SolicitudAprobadaResponse solicitudAprobadaResponse = await objDatosDAO.buscarSolicitudesAprobadasPorIdentificacion(Session["Identificacion"].ToString(), token);
                if (solicitudAprobadaResponse.estado)
                {
                    TipoSolicitudResponse respuesta = await tipoSolicitudDao.ObtenerListaTipoSolicitud(token);
                    if (respuesta.estado)
                    {
                        TipoPersonaResponse response = await tipoPersonaDao.ObtenerListaTipoPersona(token);
                        if (response.estado)
                        {
                            ViewBag.idTipoPersona = new SelectList(response.obj, "idTipoPersona", "TipoPersona", "0");
                            ViewBag.idTipoSolicitud = new SelectList(respuesta.obj, "idTipoSolicitud", "TipoSolicitud", "0");
                            lista = solicitudAprobadaResponse.lista;
                        }
                        else Request.Flash("danger", "Ha ocurrido un error al momento de recuperar los tipo de persona");
                    }
                    else Request.Flash("danger", "Ha ocurrido un error al momento de recuperar los tipo de solicitudes");
                    
                }                    
                else Request.Flash("danger", solicitudAprobadaResponse.mensajes);
            }
            catch (Exception ex)
            {
                Request.Flash("danger", ex.Message);
            }            
            return View(lista); 
        }
    }
}
