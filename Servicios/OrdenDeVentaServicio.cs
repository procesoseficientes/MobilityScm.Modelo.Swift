using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using MobilityScm.Modelo.Entidades;
using MobilityScm.Modelo.Interfaces.Servicios;
using MobilityScm.Modelo.Tipos;
using MobilityScm.Vertical.Entidades;
using Telerik.OpenAccess.Data.Common;

namespace MobilityScm.Modelo.Servicios
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class OrdenDeVentaServicio : IOrdenDeVentaServicio
    {
        // ReSharper disable once MemberCanBePrivate.Global
        public IBaseDeDatosServicio BaseDeDatosServicio { get; set; }

        public SondaOrdenDeVentaEncabezado ObtenerSondaOrdenDeVenta(string idOrdenDeVenta)
        {
            DbParameter[] parameters =
            {
                new OAParameter
                {
                    ParameterName = "@idOrdenDeVenta",
                    Value = idOrdenDeVenta
                }
            };
            return
                BaseDeDatosServicio.ExecuteQuery<SondaOrdenDeVentaEncabezado>("SWIFT_SP_GET_SALE_ORDER_HEADER",
                    CommandType.StoredProcedure, parameters).FirstOrDefault();
        }

        public List<SondaOrdenDeVentaEncabezado> ObtenerPrimerasCincoOrdenesDeVenta(string owner)
        {
            DbParameter[] parameters =
            {
                new OAParameter
                {
                    ParameterName = "@OWNER",
                    Value = owner
                }
            };
            return BaseDeDatosServicio.ExecuteQuery<SondaOrdenDeVentaEncabezado>("SWIFT_SP_GET_TOP5_SALE_ORDER_HEADER", CommandType.StoredProcedure, parameters).ToList();

        }

        public List<SondaOrdenDeVentaEncabezado> ObtenerPrimerasCincoOrdenesDeVenta()
        {
            return BaseDeDatosServicio.ExecuteQuery<SondaOrdenDeVentaEncabezado>("SWIFT_SP_GET_TOP5_SALE_ORDER_HEADER", CommandType.StoredProcedure, null).ToList();

        }


        public Operacion MarcarComoEnviadaOrdenDeVentaASbo(string idOrdenDeVenta, string resultadoEnvio, string refernciaErp, string owner, string customerOwner)
        {
            DbParameter[] parameters =
            {
                new OAParameter
                {
                    ParameterName = "@SALES_ORDER_ID",
                    Value = idOrdenDeVenta
                },
                new OAParameter
                {
                    ParameterName = "@POSTED_RESPONSE",
                    Value = resultadoEnvio
                },
                new OAParameter
                {
                    ParameterName = "@ERP_REFERENCE",
                    Value = refernciaErp
                },
                new OAParameter
                {
                    ParameterName = "@OWNER",
                    Value = owner
                },
                new OAParameter
                {
                    ParameterName = "@CUSTOMER_OWNER",
                    Value = customerOwner
                }
            };


            try
            {
                BaseDeDatosServicio.ExecuteQuery<Operacion>("SWIFT_SP-STATUS-SEND_SO_TO_SAP", CommandType.StoredProcedure, parameters);
                return new Operacion
                {
                    Codigo = 0,
                    Mensaje = "Proceso Exitoso",
                    Resultado = ResultadoOperacionTipo.Exito
                };
            }
            catch (Exception e)
            {
                BaseDeDatosServicio.Rollback();
                return new Operacion
                {
                    Codigo = -1,
                    Mensaje = e.Message,
                    Resultado = ResultadoOperacionTipo.Error
                };
            }
        }

        public Operacion MarcarComoEnviadaOrdenDeVentaASbo(string idOrdenDeVenta, string resultadoEnvio, string refernciaErp)
        {
            DbParameter[] parameters =
            {
                new OAParameter
                {
                    ParameterName = "@SALES_ORDER_ID",
                    Value = idOrdenDeVenta
                },
                new OAParameter
                {
                    ParameterName = "@POSTED_RESPONSE",
                    Value = resultadoEnvio
                },
                new OAParameter
                {
                    ParameterName = "@ERP_REFERENCE",
                    Value = refernciaErp
                }
            };


            try
            {
                BaseDeDatosServicio.ExecuteQuery<Operacion>("SWIFT_SP-STATUS-SEND_SO_TO_SAP", CommandType.StoredProcedure, parameters);
                return new Operacion
                {
                    Codigo = 0,
                    Mensaje = "Proceso Exitoso",
                    Resultado = ResultadoOperacionTipo.Exito
                };
            }
            catch (Exception e)
            {
                BaseDeDatosServicio.Rollback();
                return new Operacion
                {
                    Codigo = -1,
                    Mensaje = e.Message,
                    Resultado = ResultadoOperacionTipo.Error
                };
            }
        }


        public Operacion MarcarComoErradoOrdenDeVentaASbo(string idOrdenDeVenta, string resultadoEnvio, string owner, string customerOwner)
        {

            DbParameter[] parameters =
            {
                new OAParameter
                {
                    ParameterName = "@SALES_ORDER_ID",
                    Value = idOrdenDeVenta
                },
                new OAParameter
                {
                    ParameterName = "@POSTED_RESPONSE",
                    Value = resultadoEnvio
                },
                new OAParameter
                {
                    ParameterName = "@OWNER",
                    Value = owner
                },
                new OAParameter
                {
                    ParameterName = "@CUSTOMER_OWNER",
                    Value = customerOwner
                }
            };

            try
            {

                BaseDeDatosServicio.ExecuteQuery<Operacion>("SWIFT_SP-STATUS-ERROR_SO_TO_SAP", CommandType.StoredProcedure, parameters);
                BaseDeDatosServicio.Commit();
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

        public Operacion MarcarComoErradoOrdenDeVentaASbo(string idOrdenDeVenta, string resultadoEnvio)
        {

            DbParameter[] parameters =
            {
                new OAParameter
                {
                    ParameterName = "@SALES_ORDER_ID",
                    Value = idOrdenDeVenta
                },
                new OAParameter
                {
                    ParameterName = "@POSTED_RESPONSE",
                    Value = resultadoEnvio
                }
            };

            try
            {

                BaseDeDatosServicio.ExecuteQuery<Operacion>("SWIFT_SP-STATUS-ERROR_SO_TO_SAP", CommandType.StoredProcedure, parameters);
                BaseDeDatosServicio.Commit();
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
    }

}
