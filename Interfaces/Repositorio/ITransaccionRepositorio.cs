using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MobilityScm.Modelo.Entidades;

namespace MobilityScm.Modelo.Interfaces.Repositorio
{
    public interface ITransaccionRepositorio
    {
        IEnumerable< SwiftTxn> ObtenerTransaciones(Expression<Func<SwiftTxn, bool>>  predicado);
        void Actualizar(SwiftTxn transaccion);
        //IEnumerable<SwiftViewSboPurchaseOrderHeader> ObtenerOrdenesDeCompra();
        void Actualizar(IEnumerable<SwiftTxn> transacciones);
        //IEnumerable<SwiftViewSboOrderHeader> ObtenerOrdenesDeVenta();

        IPurchaseOrderHeader ObtenerOrdenDeCompra(Func<IPurchaseOrderHeader, bool> predicado, string docNum);
        IOrderHeader ObtenerOrdenDeVenta(Func<IOrderHeader, bool> predicado, string docNum);
        IReturnHeader ObtenerRetorno(Func<IReturnHeader, bool> predicado, string docEntry);

        IList<PurchaseSerieDetail> ObtenerSeriesOrdenDeCompra(string referencia);
        IList<OrderSerieDetail> ObtenerSeriesOrdeDeVenta(string docNum, string docEntry);
    }
}