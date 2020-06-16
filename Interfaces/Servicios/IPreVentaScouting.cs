using MobilityScm.Modelo.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MobilityScm.Modelo.Interfaces.Servicios
{
    /// <summary>
    /// Interfazo de preventa y scouting para envios a ERP
    /// </summary>
    public interface IPreVentaScouting
    {
        #region Obsoletos
        PurchaseOrderHeader ObtenerOrdeneDeCompra(string docEntry);
        OrderHeader ObtenerOrdenDeVenta(string docNum);
        ReturnHeader ObtenerRetorno(string docEntry);
        string MarcarRetornoComoEnviadoAErp(ReturnHeader retorno);
        string MarcarEntradaDeInventarioComoEnviadaAErp(PurchaseOrderHeader ordenDeCompra);
        string RegistrarInsidenciaRetorno(ReturnHeader retorno, string resultadoOperacion);
        string RegistrarInsidenciaEntradaDeInventario(PurchaseOrderHeader ordenDeCompra, string resultadoOperacion);
        string MarcarSalidaDeInventarioComoEnviadaAErp(OrderHeader ordenDeVenta);
        string RegistrarInsidenciaEnSalidaDeInventario(OrderHeader ordenDeVenta, string resultadoOperacion);


        #endregion

        #region SondaOrdenDeVenta


        SondaOrdenDeVentaEncabezado ObtenerSondaOrdenDeVenta(string idOrdenDeVenta);
        List<SondaOrdenDeVentaEncabezado> ObtenerPrimerasCincoOrdenesDeVenta(string owner);
        string MarcarComoEnviadaOrdenDeVentaASbo(string idOrdenDeVenta, string resultadoEnvio, string referenciaErp, string owner, string customerOwner);
        string MarcarComoErradoOrdenDeVentaASbo(string idOrdenDeVenta, string resultadoEnvio, string owner, string customerOwner);

        #endregion

        #region SondaFacturas

        List<SondaFacturaEncabezado> ObtenerPrimerasCincoFacturas();
        string MarcarComoEnviadaFacturaASbo(string idFactura, string resultadoEnvio, string refernciaErp);
        SondaFacturaEncabezado ObtenerSondaFactura(string idFactura);
        string MarcarComoErradoFacturaASbo(string idFactura, string resultadoEnvio);

        #endregion

        #region SondaClientes

        string MarcarComoEnviadoClienteASbo(string idCustomer, string postedResponse, string idCustomerBo);
        string MarcarComoErradoClienteASbo(string idCustomer, string postedResponse);
        Cliente ObtenerCliente(string idCustomer);

        List<Cliente> ObtenerPrimerosCincoClientes(string owner = null);

        string MarcarComoEnviadoModificacionClienteASbo(string idCustomer, string postedResponse);
        string MarcarComoErradoModificacionClienteASbo(string idCustomer, string postedResponse);
        CambiosCliente ObtenerModificacionCliente(string idCustomer);
        List<CambiosCliente> ObtenerPrimerasCincoModificacionesClientes(string owner = null);


        #endregion
    }
}
