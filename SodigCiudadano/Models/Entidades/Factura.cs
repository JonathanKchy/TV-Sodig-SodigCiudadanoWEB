using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SodigCiudadano.Models.Entidades
{

    public class Items
    {
        public int qn_item { get; set; }
        public string tx_valorcompleto { get; set; }
        public string bd_tipitem { get; set; }
        public int id { get; set; }
        public string id_item { get; set; }
        public string id_bodega { get; set; }
        public string id_nivelprecio { get; set; }
        public string tx_descripcion { get; set; }
        public decimal? qn_cantdisp { get; set; }
        public decimal? qn_cantidad { get; set; }
        public decimal? qn_cantaent { get; set; }
        public decimal? qn_cantent { get; set; }
        public decimal? qn_cantxent { get; set; }
        public int? id_unidad { get; set; }
        public string tx_unidad { get; set; }
        public decimal? qn_peso { get; set; }
        public decimal? qn_volumen { get; set; }
        public decimal? vm_costo { get; set; }
        public int? id_unidadinv { get; set; }
        public string bd_muldiv { get; set; }
        public decimal? qn_factor { get; set; }
        public decimal? vm_preciorig { get; set; }
        public decimal? qn_desc { get; set; }
        public decimal? qn_porcdescitem { get; set; }
        public decimal? vm_descuento { get; set; }
        public decimal? vm_preciodesc { get; set; }
        public decimal? vm_subtotal { get; set; }
        public decimal? qn_porcivaorig { get; set; }
        public decimal? qn_porciva { get; set; }
        public decimal? vm_valoriva { get; set; }
        public decimal? qn_porcice { get; set; }
        public decimal? vm_valorice { get; set; }
        public decimal? vm_total { get; set; }
        public string bd_itemrelacionado { get; set; }
        public decimal? qn_itemrelacionado { get; set; }
        public decimal? qn_itempromo { get; set; }
        public string bd_itempromocionado { get; set; }
        public string bd_itempromocion { get; set; }
        public string bd_modificar { get; set; }


    }

    public class TipoPagoItems
    {
        public string id_ingresodet { get; set; }
        public string id_tipopago { get; set; }
        public string tx_tipopago { get; set; }
        public string tx_enumerador { get; set; }
        public string fe_emision { get; set; }
        public string vm_valor { get; set; }
        public string id_ctactb { get; set; }
        public string id_banco { get; set; }
        public string tx_banco { get; set; }
        public string tx_emisor { get; set; }
        public string tx_numcta { get; set; }
        public string tx_numcheque { get; set; }
        public string id_ctabanc { get; set; }
        public string tx_ctabanc { get; set; }
        public string tx_numcomprobante { get; set; }
        public string id_tipomovbanc { get; set; }
        public string tx_descripcion { get; set; }
        public string id_control { get; set; }
        public string id_otraformapago { get; set; }
        public string id_tipotarjcred { get; set; }
        public string tx_numtar { get; set; }
        public string tx_numautori { get; set; }
        public string fe_caducidad { get; set; }
        public string id_retencion { get; set; }
        public string id_docventa { get; set; }
        public string tx_numretencion { get; set; }
        public string tx_enumdocventa { get; set; }
        public string fe_retencion { get; set; }
        public string qn_porcret { get; set; }
        public string vm_baseimp { get; set; }
        public string tx_codigorext { get; set; }
        public string bd_control { get; set; }
    }

    public class Factura
    {
        public string nombreCliente { get; set; }
        public string mail { get; set; }
        public string direccion { get; set; }
        public string identificacionCliente { get; set; }
        public string idTipoCredito { get; set; }
        public string fechaEmision { get; set; }
        public string fechaValidez { get; set; }
        public string txtTipoCambio { get; set; }
        public string txtPorcGrav { get; set; }
        public string ddlEstado { get; set; }
        public string txtComentario { get; set; }
        public string txtDiasGracia { get; set; }
        public string txtDiasCredito { get; set; }
        public string txtDiasProrroga { get; set; }
        public string txtPorcIntCorr { get; set; }
        public string txtPorcIntMora { get; set; }
        public string ddlid_ice { get; set; }
        public string idTipoDocumento { get; set; }
        public bool compraReembolso { get; set; }
        public ulong idPuntoVenta { get; set; }
        public ulong idEmpresa { get; set; }
        public int idUsuario { get; set; }
        public ulong idTipoMoneda { get; set; }
        public ulong idTipoCambio { get; set; }
        public ulong idVendedor { get; set; }
        public string obra { get; set; }
        public List<Items> items { get; set; }
        public List<TipoPagoItems> listaTipoPago { get; set; }
        public double subtotal_gravado { get; set; }
        public double _subtotal_exento { get; set; }
        public double _qn_descuento { get; set; }
        public double _vm_descuento { get; set; }
        public double _vm_subtotal_desc_gravado { get; set; }
        public double _vm_subtotal_desc_exento { get; set; }
        public double _vm_subtotal_desc { get; set; }
        public double _vm_iva { get; set; }
        public double _vm_ice { get; set; }
        public double _vm_subtotalice { get; set; }
        public double _vm_total { get; set; }
        public double _qn_peso { get; set; }
        public double _qn_volumen { get; set; }
    }

    public class FacturaResponse
    {
        public bool estado { get; set; }
        public string mensajes { get; set; }
        public string numeroFactura { get; set; }
        public string asientoContable { get; set; }
    }

}