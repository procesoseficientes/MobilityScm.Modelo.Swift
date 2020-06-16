using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Runtime.CompilerServices;
using MobilityScm.Modelo.Entidades;
using MobilityScm.Modelo.Estados;
using MobilityScm.Modelo.Interfaces.Servicios;
using MobilityScm.Modelo.Tipos;
using MobilityScm.Vertical.Entidades;
using Telerik.OpenAccess.Data.Common;

namespace MobilityScm.Modelo.Servicios
{
    public class SolicitudDeTrasladoServicio : ISolicitudDeTrasladoServicio
    {
        public IBaseDeDatosServicio _baseDeDatosServicio { get; set; }

        public IBaseDeDatosServicio BaseDeDatosServicio
        {
            get { return _baseDeDatosServicio; }
            set { _baseDeDatosServicio = value; }
        }

        public Operacion MarcarComoEnviadoSolicituDeTrasladoAErp(string transferRequestId, string resultadoEnvio, string referenciaErp, string owner)
        {
            DbParameter[] parameters =
            {
                new OAParameter
                {
                    ParameterName = "@RECEPTION_DOCUMENT_ID",
                    Value = transferRequestId
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
                }
            };
            try
            {
                BaseDeDatosServicio.ExecuteQuery<Operacion>(BaseDeDatosServicio.Esquema + "OP_WMS_SP_MARK_TRANSFER_REQUEST_AS_SEND_TO_ERP", CommandType.StoredProcedure, parameters);
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

        public Operacion MarcarComoErradoSolicitudDeTrasladoAErp(string transferRequestId, string resultadoEnvio, string owner)
        {
            DbParameter[] parameters =
            {
                new OAParameter
                {
                    ParameterName = "@RECEPTION_DOCUMENT_ID",
                    Value = transferRequestId
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
                BaseDeDatosServicio.ExecuteQuery<Operacion>(BaseDeDatosServicio.Esquema + "OP_WMS_SP_MARK_TRANSFER_REQUEST_AS_FAILED_TO_ERP", CommandType.StoredProcedure, parameters);
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

        public SwiftSolicitudDeTrasladoEncabezado ObtenerSolicitudDeTraslado(string transferRequestId, string owner)
        {
            DbParameter[] parameters =
            {
                new OAParameter
                {
                    ParameterName = "@DOCUMENT_ID",
                    Value = transferRequestId
                }
            };
            var traslado =
                BaseDeDatosServicio.ExecuteQuery<SwiftSolicitudDeTrasladoEncabezado>(BaseDeDatosServicio.Esquema + "OP_WMS_GET_TRANSFER_REQUEST_DOCUMENT",
                    CommandType.StoredProcedure, parameters).FirstOrDefault();
            if (traslado != null)
            {
                traslado.SwiftSolicitudDeTrasladoDetalle = ObtenerSolicitudDeTrasladoDetalle(transferRequestId, owner);
            }

            return traslado;
        }

        private List<SwiftSolicitudDeTrasladoDetalle> ObtenerSolicitudDeTrasladoDetalle(string transferRequestId, string owner)
        {
            DbParameter[] parameters =
            {
                new OAParameter
                {
                    ParameterName = "@DOCUMENT_ID",
                    Value = transferRequestId
                },new OAParameter
                {
                    ParameterName = "@OWNER",
                    Value = owner
                }
            };
            return BaseDeDatosServicio.ExecuteQuery<SwiftSolicitudDeTrasladoDetalle>(BaseDeDatosServicio.Esquema + "OP_WMS_GET_TRANSFER_REQUEST_DOCUMENT_DETAIL",
                    CommandType.StoredProcedure, parameters).ToList();
        }

        public List<SwiftRecepcionEncabezado> ObtenerPrimerosCincoDocumentosDeSolicitudDeTraslado(string owner)
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
                BaseDeDatosServicio.ExecuteQuery<SwiftRecepcionEncabezado>(
                    BaseDeDatosServicio.Esquema + "OP_WMS_SP_GET_TOP5_TRANSFER_REQUEST_DOCUMENT", CommandType.StoredProcedure,
                    parameters).ToList();
        }
    }
}
