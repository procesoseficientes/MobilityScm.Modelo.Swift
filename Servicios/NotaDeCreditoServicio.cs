using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using MobilityScm.Modelo.Entidades;
using MobilityScm.Modelo.Interfaces.Servicios;
using MobilityScm.Modelo.Tipos;
using MobilityScm.Vertical.Entidades;
using Telerik.OpenAccess.Data.Common;

namespace MobilityScm.Modelo.Servicios
{
    public class NotaDeCreditoServicio : INotaDeCreditoServicio
    {
        private IBaseDeDatosServicio _baseDeDatosServicio { get; set; }
        public IBaseDeDatosServicio BaseDeDatosServicio
        {
            get { return _baseDeDatosServicio; }
            set { _baseDeDatosServicio = value; }
        }

        public Operacion MarcarComoEnviadaNotaDeCreditoAErp(string idDeDocumentoDeRecepcion, string resultadoEnvio, string referenciaErp, string owner, string tabla)
        {
            DbParameter[] parameters =
            {
                new OAParameter
                {
                    ParameterName = "@RECEPTION_HEADER_ID",
                    Value = idDeDocumentoDeRecepcion
                },
                new OAParameter
                {
                    ParameterName = "@POSTED_RESPONSE",
                    Value = resultadoEnvio
                },
                new OAParameter
                {
                    ParameterName = "@ERP_REFERENCE",
                    Value = referenciaErp
                },
                new OAParameter
                {
                    ParameterName = "@OWNER",
                    Value = owner
                },
                new OAParameter
                {
                    ParameterName = "@TABLE_NAME",
                    Value = tabla
                }
            };
            try
            {
                BaseDeDatosServicio.ExecuteQuery<Operacion>(BaseDeDatosServicio.Esquema + "OP_WMS_SP_MARK_CREDIT_MEMO_AS_SEND_TO_ERP", CommandType.StoredProcedure, parameters);
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
                BaseDeDatosServicio.Rollback();
                return new Operacion
                {
                    Codigo = -1,
                    Mensaje = e.Message,
                    Resultado = ResultadoOperacionTipo.Error
                };
            }
        }

        public Operacion MarcarComoErradaNotaDeCreditoAErp(string idDeDocumentoDeRecepcion, string resultadoEnvio, string owner)
        {
            DbParameter[] parameters =
            {
                new OAParameter
                {
                    ParameterName = "@RECEPTION_HEADER_ID",
                    Value = idDeDocumentoDeRecepcion
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
                }
            };
            try
            {
                BaseDeDatosServicio.ExecuteQuery<Operacion>(BaseDeDatosServicio.Esquema + "OP_WMS_SP_MARK_CREDIT_MEMO_AS_FAILED_TO_ERP", CommandType.StoredProcedure, parameters);
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
                BaseDeDatosServicio.Rollback();
                return new Operacion
                {
                    Codigo = -1,
                    Mensaje = e.Message,
                    Resultado = ResultadoOperacionTipo.Error
                };
            }
        }

        public SwiftNotaDeCreditoEncabezado ObtenerNotaDeCredito(string idDeDocumentoDeRecepcion, string owner)
        {
            DbParameter[] parameters =
            {
                new OAParameter
                {
                    ParameterName = "@RECEPTION_HEADER_ID",
                    Value = idDeDocumentoDeRecepcion
                }
            };
            var notaDeCredito =
                BaseDeDatosServicio.ExecuteQuery<SwiftNotaDeCreditoEncabezado>(BaseDeDatosServicio.Esquema + "OP_WMS_GET_CREDIT_MEMO_HEADER",
                    CommandType.StoredProcedure, parameters).FirstOrDefault();
            if (notaDeCredito != null)
            {
                notaDeCredito.Detalle = ObtenerNotaDeCreditoDetalle(idDeDocumentoDeRecepcion, owner);
            }

            return notaDeCredito;
        }

        private List<SwiftNotaDeCreditoDetalle> ObtenerNotaDeCreditoDetalle(string idDeDocumentoDeRecepcion, string owner)
        {
            DbParameter[] parameters =
            {
                new OAParameter
                {
                    ParameterName = "@RECEPTION_HEADER_ID",
                    Value = idDeDocumentoDeRecepcion
                },new OAParameter
                {
                    ParameterName = "@OWNER",
                    Value = owner
                }
            };
            return BaseDeDatosServicio.ExecuteQuery<SwiftNotaDeCreditoDetalle>(BaseDeDatosServicio.Esquema + "OP_WMS_GET_CREDIT_MEMO_DETAIL",
                    CommandType.StoredProcedure, parameters).ToList();
        }

        public List<SwiftNotaDeCreditoEncabezado> ObtenerPrimerasCincoNotasDeCredito(string owner)
        {
            DbParameter[] parameters =
             {
                    new OAParameter
                    {
                        ParameterName = "@OWNER",
                        Value = owner
                    }
                };

            return
                BaseDeDatosServicio.ExecuteQuery<SwiftNotaDeCreditoEncabezado>(
                    BaseDeDatosServicio.Esquema + "OP_WMS_GET_TOP5_CREDIT_MEMOS", CommandType.StoredProcedure,
                    parameters).ToList();
        }
    }
}
