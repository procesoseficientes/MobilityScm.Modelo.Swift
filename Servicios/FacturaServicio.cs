using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using MobilityScm.Modelo.Entidades;
using MobilityScm.Modelo.Tipos;
using MobilityScm.Vertical.Entidades;
using Telerik.OpenAccess.Data.Common;

namespace MobilityScm.Modelo.Servicios
{
    public class FacturaServicio : IFacturaServicio
    {
        public IBaseDeDatosServicio BaseDeDatosServicio { get; set; }

        public SondaFacturaEncabezado ObtenerSondaFactura(string idFactura)
        {
            var valores = idFactura.Split('$');
            DbParameter[] parameters =
            {
                new OAParameter
                {
                    ParameterName = "@INVOICE_ID",
                    Value = valores[0]
                },
                new OAParameter
                {
                    ParameterName = "@CDF_SERIE",
                    Value = valores[1]
                },
                new OAParameter
                {
                    ParameterName = "@CDF_RESOLUCION",
                    Value = valores[2]
                },
                new OAParameter
                {
                    ParameterName = "@IS_CREDIT_NOTE",
                    Value = valores[3]
                }

            };
            var factura =
                BaseDeDatosServicio.ExecuteQuery<SondaFacturaEncabezado>("SWIFT_SP_GET_INVOICE_HEADER",
                    CommandType.StoredProcedure, parameters).FirstOrDefault();
            BaseDeDatosServicio.Commit();
            return factura;
        }

        public List<SondaFacturaEncabezado> ObtenerPrimerasCincoFacturas()
        {
            var facturas = BaseDeDatosServicio.ExecuteQuery<SondaFacturaEncabezado>("SWIFT_SP_GET_TOP5_INVOICE_HEADER",
                CommandType.StoredProcedure);
            BaseDeDatosServicio.Commit();

            return facturas.ToList();
        }


        public Operacion MarcarComoEnviadaFacturaASbo(string idFactura, string resultadoEnvio, string referenciaErp)
        {
            var valores = idFactura.Split('$');
            DbParameter[] parameters =
            {
                new OAParameter
                {
                    ParameterName = "@INVOICE_ID",
                    Value = valores[0]
                },
                new OAParameter
                {
                    ParameterName = "@CDF_SERIE",
                    Value = valores[1]
                },
                new OAParameter
                {
                    ParameterName = "@CDF_RESOLUCION",
                    Value = valores[2]
                },
                new OAParameter
                {
                    ParameterName = "@IS_CREDIT_NOTE",
                    Value = valores[3]
                },
                new OAParameter
                {
                    ParameterName = "@POSTED_RESPONSE",
                    Value = resultadoEnvio
                }
                ,
                new OAParameter
                {
                    ParameterName = "@ERP_REFERENCE",
                    Value = referenciaErp
                }
            };


            try
            {
                BaseDeDatosServicio.ExecuteQuery<Operacion>("SWIFT_SP_MARK_INVOICE_AS_SEND_TO_ERP",
                    CommandType.StoredProcedure, parameters);
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


        public Operacion MarcarComoErradoFacturaASbo(string idFactura, string resultadoEnvio)
        {
            var valores = idFactura.Split('$');
            DbParameter[] parameters =
            {
                new OAParameter
                {
                    ParameterName = "@INVOICE_ID",
                    Value = valores[0]
                },
                new OAParameter
                {
                    ParameterName = "@CDF_SERIE",
                    Value = valores[1]
                },
                new OAParameter
                {
                    ParameterName = "@CDF_RESOLUCION",
                    Value = valores[2]
                },
                new OAParameter
                {
                    ParameterName = "@IS_CREDIT_NOTE",
                    Value = valores[3]
                },
                new OAParameter
                {
                    ParameterName = "@POSTED_RESPONSE",
                    Value = resultadoEnvio
                }
            };

            try
            {
                BaseDeDatosServicio.ExecuteQuery<Operacion>("SWIFT_SP_MARK_INVOICE_AS_FAILED_TO_ERP",
                    CommandType.StoredProcedure, parameters);
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