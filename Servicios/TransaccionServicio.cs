using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MobilityScm.Modelo.Entidades;
using MobilityScm.Modelo.Estados;
using MobilityScm.Modelo.Interfaces.Repositorio;
using MobilityScm.Modelo.Interfaces.Servicios;
using MobilityScm.Modelo.Tipos;
using MobilityScm.Utilerias;
using MobilityScm.Vertical.Entidades;
using System.Data.Common;
using Telerik.OpenAccess.Data.Common;
using System.Data;

namespace MobilityScm.Modelo.Servicios
{
    public class TransaccionServicio : ITransaccionServicio
    {
        public Operacion RegistrarInsidenciaRetorno(ReturnHeader retorno, Operacion op)
        {
            try
            {
                _retornoRepositorio.RegistrarInsidencia(retorno, op);
                return new Operacion
                {
                    Codigo = 0,
                    Mensaje = "Proceso Exitoso",
                    Resultado = ResultadoOperacionTipo.Exito
                };
            }
            catch (Exception e)
            {

                return new Operacion
                {
                    Codigo = -1,
                    Mensaje = e.Message,
                    Resultado = ResultadoOperacionTipo.Error
                };
            }
            
        }

        public IList<PurchaseSerieDetail> ObtenerSeriesOrdenDeCompra(string referencia)
        {
            return  _transaccionRepositorio.ObtenerSeriesOrdenDeCompra(referencia);
        }

        public IList<OrderSerieDetail> ObtenerSeriesOrdeDeVenta(string docNum, string docEntry)
        {
            return _transaccionRepositorio.ObtenerSeriesOrdeDeVenta(docNum, docEntry); 
        }

        public Operacion MarcarRetornoComoEnviadoAErp(ReturnHeader retorno)
        {
            try
            {
                _retornoRepositorio.MarcarRetornoComoEnviadoAErp(retorno);
                return new Operacion
                {
                    Codigo = 0,
                    Mensaje = "Proceso Exitoso",
                    Resultado = ResultadoOperacionTipo.Exito
                };
            }
            catch (Exception e)
            {

                return new Operacion
                {
                    Codigo = -1,
                    Mensaje = e.Message,
                    Resultado = ResultadoOperacionTipo.Error
                };
            }
        }

        private readonly ITransaccionRepositorio _transaccionRepositorio;
        private readonly IRetornoRepositorio _retornoRepositorio;
        private readonly IBaseDeDatosServicio _baseDeDatosServicio;

        public TransaccionServicio()
        {
            _transaccionRepositorio = Mvx.Ioc.Resolve<ITransaccionRepositorio>();
            _retornoRepositorio = Mvx.Ioc.Resolve<IRetornoRepositorio>();
            _baseDeDatosServicio = Mvx.Ioc.Resolve<IBaseDeDatosServicio>();
        }

        //public Operacion ObtenerOrdenesDeVenta()
        //{
        //    try
        //    {
        //        return new Operacion
        //        {
        //            Codigo = 0,
        //            Mensaje = "Proceso Exitoso",
        //            Dato = _transaccionRepositorio.ObtenerOrdenesDeVenta(),
        //            Resultado = ResultadoOperacionTipo.Exito
        //        };
        //    }
        //    catch (Exception e)
        //    {

        //        return new Operacion
        //        {
        //            Codigo = -1,
        //            Mensaje = e.Message,
        //            Resultado = ResultadoOperacionTipo.Error
        //        };
        //    }
        //}


        public Operacion ObtenerOrdenDeCompra(Func<IPurchaseOrderHeader, bool> predicado, string docEntry)
        {
            try
            {

                return new Operacion
                {
                    Codigo = 0,
                    Mensaje = "Proceso Exitoso",
                    Dato = _transaccionRepositorio.ObtenerOrdenDeCompra(predicado, docEntry),
                    Resultado = ResultadoOperacionTipo.Exito
                };

            }
            catch (Exception e)
            {

                return new Operacion
                {
                    Codigo = -1,
                    Mensaje = e.Message,
                    Resultado = ResultadoOperacionTipo.Error
                };
            }

        }

        public Operacion ObtenerOrdenDeVenta(Func<IOrderHeader, bool> predicado, string docNum)
        {
            try
            {
                return new Operacion
                {
                    Codigo = 0,
                    Mensaje = "Proceso Exitoso",
                    Dato = _transaccionRepositorio.ObtenerOrdenDeVenta(predicado, docNum),
                    Resultado = ResultadoOperacionTipo.Exito
                };

            }
            catch (Exception e)
            {

                return new Operacion
                {
                    Codigo = -1,
                    Mensaje = e.Message,
                    Resultado = ResultadoOperacionTipo.Error
                };
            }

        }

        public Operacion ObtenerRetorno(Func<IReturnHeader, bool> predicado, string docEntry)
        {
            try
            {
                return new Operacion
                {
                    Codigo = 0,
                    Mensaje = "Proceso Exitoso",
                    Dato = _transaccionRepositorio.ObtenerRetorno(predicado, docEntry),
                    Resultado = ResultadoOperacionTipo.Exito
                };
            }
            catch (Exception e)
            {
                return new Operacion
                {
                    Codigo = -1,
                    Mensaje = e.Message,
                    Resultado = ResultadoOperacionTipo.Error
                };
            }
        }

        //public Operacion ObtenerOrdenesDeCompra()
        //{
        //    try
        //    {
        //        return new Operacion
        //        {
        //            Codigo = 0,
        //            Mensaje = "Proceso Exitoso",
        //            Dato = _transaccionRepositorio.ObtenerOrdenesDeCompra(),
        //            Resultado = ResultadoOperacionTipo.Exito
        //        };
        //    }
        //    catch (Exception e)
        //    {

        //        return new Operacion
        //        {
        //            Codigo = -1,
        //            Mensaje = e.Message,
        //            Resultado = ResultadoOperacionTipo.Error
        //        };
        //    }
        //}


        public Operacion MarcarSalidaDeInventarioComoEnviadaAErp(OrderHeader ordenDeVenta)
        {
            try
            {

                IList<SwiftTxn> transacciones = new List<SwiftTxn>();
                foreach (var linea in ordenDeVenta.SwiftViewSboOrderDetails)
                {
                    var transaccionId = linea.TransactionId;
                    var transaccion = _transaccionRepositorio.ObtenerTransaciones(t => t.TxnId == transaccionId).FirstOrDefault();
                    if (transaccion != null)
                    {
                        transaccion.TxnIsPostedErp = Convert.ToInt16(SiNo.Si);
                        transaccion.TxnPostedErp = DateTime.Now;
                        transaccion.TxnPostedResponse = ordenDeVenta.Comments;
                        transacciones.Add(transaccion);
                    }

                    

                }

                _transaccionRepositorio.Actualizar(transacciones);

                

                return new Operacion
                {
                    Codigo = 0,
                    Mensaje = "Proceso Exitoso",
                    Resultado = ResultadoOperacionTipo.Exito
                };

            }
            catch (Exception e)
            {

                return new Operacion
                {
                    Codigo = -1,
                    Mensaje = e.Message,
                    Resultado = ResultadoOperacionTipo.Error
                };
            }
        }

      

        public Operacion MarcarEntradaDeInventarioComoEnviadaAErp(PurchaseOrderHeader ordenDeCompra)
        {
            try
            {
                IList<SwiftTxn> transacciones=new List<SwiftTxn>(); 
                foreach (var linea in ordenDeCompra.SwiftViewSboPurchaseOrderDetails)
                {
                    var transaccionId = linea.TransactionId; 
                    var transaccion = _transaccionRepositorio.ObtenerTransaciones(t => t.TxnId == transaccionId).FirstOrDefault();
                    if (transaccion != null)
                    {
                        transaccion.TxnIsPostedErp = Convert.ToInt16(SiNo.Si);
                        transaccion.TxnPostedErp = DateTime.Now;
                        transaccion.TxnPostedResponse = ordenDeCompra.Comments;
                        transacciones.Add(transaccion);
                    }

                    
                    
                }

                _transaccionRepositorio.Actualizar(transacciones);
               
                return new Operacion
                {
                    Codigo = 0,
                    Mensaje = "Proceso Exitoso",
                    Resultado = ResultadoOperacionTipo.Exito
                };

            }
            catch (Exception e)
            {

                return new Operacion
                {
                    Codigo = -1,
                    Mensaje = e.Message,
                    Resultado = ResultadoOperacionTipo.Error
                };
            }
        }

        public Operacion RegistrarInsidenciaEnSalidaDeInventario(OrderHeader ordenDeVenta, Operacion op)
        {
            try
            {

                IList<SwiftTxn> transacciones = new List<SwiftTxn>();
                foreach (var linea in ordenDeVenta.SwiftViewSboOrderDetails)
                {
                    var transaccionId = linea.TransactionId;
                    var transaccion = _transaccionRepositorio.ObtenerTransaciones(t => t.TxnId == transaccionId).FirstOrDefault();
                    if (transaccion != null)
                    {
                        transaccion.TxnIsPostedErp = Convert.ToInt16(SiNo.No);
                        transaccion.TxnPostedResponse = op.Mensaje;
                        transaccion.TxnAttemptedWithError = transaccion.TxnAttemptedWithError == null ? 1 : transaccion.TxnAttemptedWithError + 1;
                        transaccion.TxnPostedErp = DateTime.Now;
                        transacciones.Add(transaccion);
                    }



                }

                _transaccionRepositorio.Actualizar(transacciones);


                return new Operacion
                {
                    Codigo = 0,
                    Mensaje = "Proceso Exitoso",
                    Resultado = ResultadoOperacionTipo.Exito
                };

            }
            catch (Exception e)
            {

                return new Operacion
                {
                    Codigo = -1,
                    Mensaje = e.Message,
                    Resultado = ResultadoOperacionTipo.Error
                };
            }
        }

       

        public Operacion RegistrarInsidenciaEntradaDeInventario(PurchaseOrderHeader ordenDeCompra, Operacion op)
        {
            try
            {

                IList<SwiftTxn> transacciones = new List<SwiftTxn>();
                foreach (var linea in ordenDeCompra.SwiftViewSboPurchaseOrderDetails)
                {
                    var transaccionId = linea.TransactionId;
                    var transaccion = _transaccionRepositorio.ObtenerTransaciones(t => t.TxnId == transaccionId).FirstOrDefault();
                    if (transaccion != null)
                    {
                        transaccion.TxnIsPostedErp = Convert.ToInt16(SiNo.No);
                        transaccion.TxnPostedResponse = op.Mensaje;
                        transaccion.TxnAttemptedWithError = transaccion.TxnAttemptedWithError == null ? 1 : transaccion.TxnAttemptedWithError + 1;
                        transaccion.TxnPostedErp = DateTime.Now;
                        transacciones.Add(transaccion);
                    }



                }

                _transaccionRepositorio.Actualizar(transacciones);



              
                return new Operacion
                {
                    Codigo = 0,
                    Mensaje = "Proceso Exitoso",
                    Resultado = ResultadoOperacionTipo.Exito
                };

            }
            catch (Exception e)
            {

                return new Operacion
                {
                    Codigo = -1,
                    Mensaje = e.Message,
                    Resultado = ResultadoOperacionTipo.Error
                };
            }
            
        }

        
        public Operacion ObtenerTransacciones(Expression<Func<SwiftTxn, bool>> predicado)
        {
            try
            {
                return new Operacion
                {
                    Codigo = 0,
                    Mensaje = "Proceso Exitoso",
                    Dato = _transaccionRepositorio.ObtenerTransaciones(predicado),
                    Resultado = ResultadoOperacionTipo.Exito
                };
            }
            catch (Exception e)
            {

                return new Operacion
                {
                    Codigo = -1,
                    Mensaje = e.Message,
                    Resultado = ResultadoOperacionTipo.Error
                };
            }
        }

        public Operacion InsertarLogDeTransaccion(string erpTarget, string operationType, string objectName, string message, string docId, string erpReference, int erpPostingAttempts)
        {
            try
            {
                _baseDeDatosServicio.BeginTransaction();
                DbParameter[] parameters ={
                    new OAParameter
                    {
                        ParameterName = "@ERP_TARGET",
                        Value = erpTarget
                    }
                    ,new OAParameter
                    {
                        ParameterName = "@OPERATION_TYPE",
                        Value = operationType
                    }
                    ,new OAParameter
                    {
                        ParameterName = "@OBJECT_NAME",
                        Value = objectName
                    }
                    ,new OAParameter
                    {
                        ParameterName = "@MESSAGE",
                        Value = message
                    }
                    ,new OAParameter
                    {
                        ParameterName = "@DOC_ID",
                        Value = docId
                    }
                    ,new OAParameter
                    {
                        ParameterName = "@ERP_REFERENCE",
                        Value = erpReference
                    }
                    ,new OAParameter
                    {
                        ParameterName = "@ERP_POSTING_ATTEMPTS",
                        Value = erpPostingAttempts
                    }
                };

                var op = _baseDeDatosServicio.ExecuteQuery<Operacion>("SWIFT_SP_INSERT_LOG_INTERFACE", CommandType.StoredProcedure, parameters).ToList()[0];
                _baseDeDatosServicio.Commit();
                return op;
            }
            catch (Exception e)
            {
                _baseDeDatosServicio.Rollback();
                return new Operacion
                {
                    Codigo = -1,
                    Mensaje = e.Message,
                    Resultado = ResultadoOperacionTipo.Error
                };
            }
        }
    }
}