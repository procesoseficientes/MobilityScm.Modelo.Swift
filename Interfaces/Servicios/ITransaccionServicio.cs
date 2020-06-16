using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MobilityScm.Modelo.Entidades;
using MobilityScm.Vertical.Entidades;

namespace MobilityScm.Modelo.Interfaces.Servicios
{
    public interface ITransaccionServicio
    {
        Operacion ObtenerTransacciones(Expression<Func<SwiftTxn, bool>> predicado);
        //Operacion ObtenerOrdenesDeCompra();
        Operacion MarcarEntradaDeInventarioComoEnviadaAErp(PurchaseOrderHeader ordenDeCompra);
        Operacion RegistrarInsidenciaEntradaDeInventario(PurchaseOrderHeader ordenDeCompra, Operacion op);
        //Operacion ObtenerOrdenesDeVenta();
        Operacion MarcarSalidaDeInventarioComoEnviadaAErp(OrderHeader ordenDeVenta);
        Operacion RegistrarInsidenciaEnSalidaDeInventario(OrderHeader ordenDeVenta, Operacion op);


        Operacion ObtenerOrdenDeCompra(Func<IPurchaseOrderHeader, bool> predicado, string docNum);
        Operacion ObtenerOrdenDeVenta(Func<IOrderHeader, bool> predicado, string docNum);
        Operacion ObtenerRetorno(Func<IReturnHeader, bool> predicado, string docEntry);

        Operacion MarcarRetornoComoEnviadoAErp(ReturnHeader retorno);
        Operacion RegistrarInsidenciaRetorno(ReturnHeader retorno, Operacion op);
        IList<PurchaseSerieDetail> ObtenerSeriesOrdenDeCompra(string referencia);
        IList<OrderSerieDetail> ObtenerSeriesOrdeDeVenta(string docNum, string docEntry);

        Operacion InsertarLogDeTransaccion(string erpTarget, string operationType, string objectName, string message, string salesOrderId, string erpReference, int erpPostingAttempts);
    }
}