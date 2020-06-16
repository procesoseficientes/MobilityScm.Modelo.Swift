using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MobilityScm.Modelo.Entidades;
using MobilityScm.Modelo.Interfaces.Repositorio;
using MobilityScm.Utilerias;

namespace MobilityScm.Modelo.Repositorio
{
    public class TransaccionRepositorio : ITransaccionRepositorio
    {
        private readonly IBaseDeDatosServicio _baseDeDatosServicio;

        public TransaccionRepositorio()
        {
            _baseDeDatosServicio = Mvx.Ioc.Resolve<IBaseDeDatosServicio>(); 
        }

        public IEnumerable< SwiftTxn> ObtenerTransaciones(Expression<Func<SwiftTxn, bool>>  predicado)
        {
            //var baseDeDatosServicio = Mvx.Resolve<IBaseDeDatosServicio>();
            return _baseDeDatosServicio.GetSwiftTxns().Where(predicado);

        }


        public void Actualizar(SwiftTxn transaccion)
        {
            //var baseDeDatosServicio = Mvx.Resolve<IBaseDeDatosServicio>();
            _baseDeDatosServicio.BeginTransaction();
            _baseDeDatosServicio.Update(transaccion);
            _baseDeDatosServicio.Commit();
        }

        //public IEnumerable<SwiftViewSboPurchaseOrderHeader> ObtenerOrdenesDeCompra()
        //{
        //    return BaseDeDatosServicio.GetSwiftViewSboPurchaseOrderHeaders();
        //}


        //public IEnumerable<SwiftViewSboOrderHeader> ObtenerOrdenesDeVenta()
        //{
        //    return BaseDeDatosServicio.GetSwiftViewSboOrderHeaders();
        //}

        public IPurchaseOrderHeader ObtenerOrdenDeCompra(Func<IPurchaseOrderHeader, bool> predicado, string docEntry)
        {

            return _baseDeDatosServicio.GetSwiftViewSboPurchaseOrderHeaders(docEntry).FirstOrDefault(predicado);
        }

        public IOrderHeader ObtenerOrdenDeVenta(Func<IOrderHeader, bool> predicado, string docNum)
        {
            return _baseDeDatosServicio.GetSwiftViewSboOrderHeaders(docNum).FirstOrDefault(predicado);
        }

        public IReturnHeader ObtenerRetorno(Func<IReturnHeader, bool> predicado, string docEntry)
        {
            return _baseDeDatosServicio.GetSwiftViewSboReturnHeaders(docEntry).FirstOrDefault(predicado);
        }

        public IList<PurchaseSerieDetail> ObtenerSeriesOrdenDeCompra(string referencia)
        {
            return _baseDeDatosServicio.ObtenerSeriesOrdenDeCompra(referencia);
        }

        public IList<OrderSerieDetail> ObtenerSeriesOrdeDeVenta(string docNum, string docEntry)
        {
            return _baseDeDatosServicio.ObtenerSeriesOrdeDeVenta(docNum, docEntry);
        }

        public void Actualizar(IEnumerable<SwiftTxn> transacciones)
        {
            //var baseDeDatosServicio = Mvx.Resolve<IBaseDeDatosServicio>();
            _baseDeDatosServicio.BeginTransaction();
            foreach (var transaccion in transacciones)
            {
                _baseDeDatosServicio.Update(transaccion);    
            }

            _baseDeDatosServicio.Commit();
        }

     
    }
}