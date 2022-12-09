using SodigCiudadano.Models.Dao;
using SodigCiudadano.Models.Entidades;
using SodigCiudadano.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SodigCiudadano.Controllers
{
    [SecurityFilter]
    public class TicketController : Controller
    {
        private CatalogoDAO objCatalogoDao = new CatalogoDAO();
        private TicketDAO objDao = new TicketDAO();

        // GET: Ticket
        public async Task<ActionResult> Index(int msg=0)
        {
            Ticket obj = new Ticket();
            ViewBag.idTipo = new SelectList(new List<CatalogoTicket>());
            try
            {
                if(msg == 1)
                    Request.Flash("danger", "Ha superado el número máximo de intentos. Por favor, agende una cita");
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
        public async Task<ActionResult> CrearTicket(Ticket objTicket, HttpPostedFileBase postedFile)
        {
            try
            {               
                string token = Session["access_token"].ToString();
                objTicket.ipRegistro = Request.UserHostAddress;
                objTicket.idEstadoTicket = 1;
                byte[] imagen = null;
                if (postedFile != null)
                {
                    if (postedFile.ContentLength > 6000000)
                    {
                        Request.Flash("danger", "Archivo Demasiado Pesado. Por favor, seleccione un archivo menor a 6MB");
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

                TicketResponse objResponse = await objDao.InsertarTicket(objTicket, token, imagen);
                if (objResponse.estado)
                {
                    Request.Flash("success", "Ticket creado correctamente");
                    return RedirectToAction("Index", "Home");
                }                   
                else
                    Request.Flash("danger", objResponse.mensaje);
            }
            catch (Exception ex)
            {
                Request.Flash("danger", ex.Message);
            }
            return RedirectToAction("Index", "Ticket");
        }

        public ActionResult BuscarTicket() 
        {
            List<ComentarioTicket> listaComentario = new List<ComentarioTicket>();            
            return View(listaComentario);
        }

        [HttpPost]
        public async Task<ActionResult> BuscarTicket(int nTicket, string identificacion) 
        {
            List<ComentarioTicket> listaComentario = new List<ComentarioTicket>();
            ViewBag.estadoTicket = "";
            try
            {
                string token = Session["access_token"].ToString();
                ListaComentarioTicketResponse objListaReponse = await objDao.ObtenerListaComentarioPorIdTicketEIdentificacion(nTicket, identificacion, token);
                if (objListaReponse.estado)
                {
                    listaComentario = objListaReponse.obj;
                    if (listaComentario.Count <= 0)
                        Request.Flash("warning", "No existen registros con los datos ingresados");
                    else
                        ViewBag.estadoTicket = listaComentario[0].EstadoTicketGeneral;
                }
                else Request.Flash("danger", objListaReponse.mensaje);
               
            }
            catch (Exception ex)
            {
                Request.Flash("danger", ex.Message);
            }
            return View(listaComentario);
        }
    }
}