using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;




namespace SodigCiudadano.Models.Entidades
{
  public  class PrecioItem
    {
        public long id_nivelprecio { get; set; }
        public long id_empresa { get; set; }
        public long id_item { get; set; }
        public long id_unidad { get; set; }
        public long id_moneda { get; set; }
        public decimal vm_costo { get; set; }
        public string bd_tipomargen { get; set; }
        public decimal vm_margen { get; set; }
        public decimal vm_preciolista { get; set; }
        public decimal qn_porciva { get; set; }
        public decimal vm_valoriva { get; set; }
        public decimal qn_porcice { get; set; }
        public decimal vm_valorice { get; set; }
        public decimal vm_otros { get; set; }
        public decimal vm_precioventa { get; set; }
        public string tx_nivelprecio { get; set; }
        public string bd_tipoitem { get; set; }
        public string id_iteminterno { get; set; }
        public string tx_item { get; set; }
        public string tx_unidaddesc { get; set; }
    }

    public class PrecioProducto
    {
        public long id_item { get; set; }
        public long id_clasificacionitem { get; set; }
        public string bd_tipoitem { get; set; }
        public string id_iteminterno { get; set; }
        public string tx_descripcion { get; set; }
        public string tx_siglas { get; set; }
        public string tx_desccomercial { get; set; }
        public string tx_desccientifica { get; set; }
        public string tx_desctecnica { get; set; }
        public string tx_desccomun { get; set; }
        public Nullable<long> id_unidadinv { get; set; }
        public long id_itemrelac { get; set; }
        public string bd_itemespecial { get; set; }
        public string bd_actprecioauto { get; set; }
        public string bd_tipodocpersonal { get; set; }
        public decimal qn_porcdescexcl { get; set; }
        public decimal qn_porcdescrebate { get; set; }
        public decimal qn_peso { get; set; }
        public decimal qn_volumen { get; set; }
        public string tx_rutafoto { get; set; }
        public string tx_comentario { get; set; }
        public long id_creadopor { get; set; }
        public System.DateTime fe_creacion { get; set; }
        public Nullable<long> id_modificadopor { get; set; }
        public Nullable<System.DateTime> fe_ultmod { get; set; }
        public string tx_unidad { get; set; }
        public string bd_estado { get; set; }
    }

    public class Precio
    {
        public PrecioItem item { get; set; }
        public PrecioProducto descripcion { get; set; }
    }

    public class PrecioReponse
    {
        public bool estado { get; set; }
        public Precio obj { get; set; }
        public string mensajes { get; set; }
    }
}



