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
    public class RecepcionServicio : IRecepcionServicio
    {
        public IBaseDeDatosServicio BaseDeDatosServicio { get; set; }

        public SwiftRecepcionEncabezado ObtenerRecepcion(string receptionHeader, string dueño)
        {
            DbParameter[] parameters =
           {
                new OAParameter
                {
                    ParameterName = "@RECEPTION_HEADER",
                    Value = receptionHeader
                }
            };
            var recepcion =
                BaseDeDatosServicio.ExecuteQuery<SwiftRecepcionEncabezado>("SWIFT_SP_GET_ERP_POH",
                    CommandType.StoredProcedure, parameters).FirstOrDefault();
            BaseDeDatosServicio.Commit();

            if (recepcion != null)
            {
                recepcion.SwiftRecepcionDetalle = this.SwiftRecepcionDetalle(recepcion.DocEntry);
                recepcion.SwiftRecepcionSeries = this.SwiftRecepcionSeries(recepcion.DocEntry);
            }
            return recepcion;
        }


        public Operacion MarcarComoEnviadaRecepcionAErp(string receptionHeader, string resultadoEnvio, string referenciaErp)
        {
            DbParameter[] parameters =
             {
            new OAParameter
            {
            ParameterName = "@RECEPTION_HEADER",
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
                BaseDeDatosServicio.ExecuteQuery<Operacion>("SWIFT_SP_MARK_RECEPTION_AS_SEND_TO_ERP", CommandType.StoredProcedure, parameters);
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
            ParameterName = "@RECEPTION_HEADER",
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

                BaseDeDatosServicio.ExecuteQuery<Operacion>("SWIFT_SP_MARK_RECEPTION_AS_FAILED_TO_ERP", CommandType.StoredProcedure, parameters);
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
            return new List<SwiftRecepcionEncabezado>();
        }

        public List<SwiftRecepcionEncabezado> ObtenerPrimerosCincoDocumentosDeRecepcionFallidos(string dueño)
        {
            return new List<SwiftRecepcionEncabezado>();
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
                BaseDeDatosServicio.ExecuteQuery<SwiftRecepcionDetalle>("SWIFT_SP_GET_ERP_POD",
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
            var pickingSeries = BaseDeDatosServicio.ExecuteQuery<SwiftRecepcionSerie>("SWIFT_SP_GET_ERP_POS",
                CommandType.StoredProcedure, parameters);
            return pickingSeries.ToList();
        }
    }
}