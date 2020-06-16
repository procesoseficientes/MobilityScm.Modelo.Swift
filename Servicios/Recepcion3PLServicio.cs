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
    public class Recepcion3PLServicio : IRecepcionServicio
    {

        #region Conexión BD
        private IBaseDeDatosServicio _baseDeDatosServicio;

        public IBaseDeDatosServicio BaseDeDatosServicio
        {
            get { return _baseDeDatosServicio; }
            set { _baseDeDatosServicio = value; }
        }
        #endregion

        public SwiftRecepcionEncabezado ObtenerRecepcion(string receptionHeader,string dueño)
        {
            DbParameter[] parameters =
            {
                new OAParameter
                {
                    ParameterName = "@RECEPTION_DOCUMENT_ID",
                    Value = receptionHeader
                }
                ,new OAParameter
                {
                    ParameterName = "@OWNER",
                    Value = dueño
                }
            };
            SwiftRecepcionEncabezado recepcion = null;
            try
            {
                recepcion =
                BaseDeDatosServicio.ExecuteQuery<SwiftRecepcionEncabezado>(BaseDeDatosServicio.Esquema + "OP_WMS_GET_RECEPTION_DOCUMENT",
                    CommandType.StoredProcedure, parameters).FirstOrDefault();

                BaseDeDatosServicio.Commit();
                if (recepcion != null)
                {
                    recepcion.SwiftRecepcionDetalle = SwiftRecepcionDetalle(recepcion.DocNum);
                    recepcion.SwiftRecepcionSeries = SwiftRecepcionSeries(recepcion.DocNum);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.StackTrace);
            }

            return recepcion;
        }


        public Operacion MarcarComoEnviadaRecepcionAErp(string receptionHeader, string resultadoEnvio, string referenciaErp)
        {
            DbParameter[] parameters =
             {
            new OAParameter
            {
            ParameterName = "@RECEPTION_DOCUMENT_ID",
            Value = receptionHeader
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
             }

             };


            try
            {
                BaseDeDatosServicio.ExecuteQuery<Operacion>(BaseDeDatosServicio.Esquema + "OP_WMS_SP_MARK_RECEPTION_AS_SEND_TO_ERP", CommandType.StoredProcedure, parameters);
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


        public Operacion MarcarComoErradaRecepcionAErp(string receptionHeader, string resultadoEnvio)
        {

            DbParameter[] parameters =
             {
            new OAParameter
            {
            ParameterName = "@RECEPTION_DOCUMENT_ID",
            Value = receptionHeader
             },
              new OAParameter
            {
            ParameterName = "@POSTED_RESPONSE",
            Value = resultadoEnvio
             }
             };

            try
            {

                BaseDeDatosServicio.ExecuteQuery<Operacion>(BaseDeDatosServicio.Esquema + "OP_WMS_SP_MARK_RECEPTION_AS_FAILED_TO_ERP", CommandType.StoredProcedure, parameters);
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

        public List<SwiftRecepcionEncabezado> ObtenerPrimerosCincoDocumentosDeRecepcion(string dueño)
        {
            DbParameter[] parameters =
            {
                new OAParameter
                {
                    ParameterName = "@OWNER",
                    Value = dueño
                }
            };
            return BaseDeDatosServicio.ExecuteQuery<SwiftRecepcionEncabezado>(BaseDeDatosServicio.Esquema + "OP_WMS_SP_GET_TOP5_RECEPTION_DOCUMENT", CommandType.StoredProcedure, parameters).ToList();
        }

        public List<SwiftRecepcionEncabezado> ObtenerPrimerosCincoDocumentosDeRecepcionFallidos(string dueño)
        {
            DbParameter[] parameters =
            {
                new OAParameter
                {
                    ParameterName = "@OWNER",
                    Value = dueño
                }
            };
            return BaseDeDatosServicio.ExecuteQuery<SwiftRecepcionEncabezado>(BaseDeDatosServicio.Esquema + "OP_WMS_SP_GET_TOP5_FAILED_RECEPTION_DOCUMENT", CommandType.StoredProcedure, parameters).ToList();
        }

        public List<SwiftRecepcionDetalle> SwiftRecepcionDetalle(string docEntry)
        {
            if (docEntry != "") return new List<SwiftRecepcionDetalle>();
            DbParameter[] parameters =
            {
                    new OAParameter
                    {
                        ParameterName = "@RECEPTION_HEADER",
                        Value = docEntry
                    }
                };

            var pickingDetalle =
                BaseDeDatosServicio.ExecuteQuery<SwiftRecepcionDetalle>(BaseDeDatosServicio.Esquema + "OP_WMS_SP_GET_RECEPTION_DETAILS",
                    CommandType.StoredProcedure, parameters);
            return pickingDetalle.ToList();
        }

        public List<SwiftRecepcionSerie> SwiftRecepcionSeries(string docEntry)
        {
            if (docEntry != "") return new List<SwiftRecepcionSerie>();
            DbParameter[] parameters = {
                                           new OAParameter
                                           {
                                               ParameterName = "@RECEPTION_HEADER",
                                               Value = docEntry
                                           }
                                       };
        var pickingSeries = BaseDeDatosServicio.ExecuteQuery<SwiftRecepcionSerie>(BaseDeDatosServicio.Esquema + "OP_WMS_SP_GET_SERIES_BY_RECEPTION_HEADER",
            CommandType.StoredProcedure, parameters);
            return pickingSeries.ToList();
        }
    }
}
