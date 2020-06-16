using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using MobilityScm.Modelo.Entidades;
using MobilityScm.Modelo.Estados;
using MobilityScm.Modelo.Interfaces.Servicios;
using MobilityScm.Vertical.Entidades;
using Telerik.OpenAccess.Data.Common;

namespace MobilityScm.Modelo.Servicios
{
    public class NotaDeEntregaServicio : INotaDeEntregaServicio
    {
        public IBaseDeDatosServicio _baseDeDatosServicio { get; set; }

        public IBaseDeDatosServicio BaseDeDatosServicio
        {
            get { return _baseDeDatosServicio; }
            set { _baseDeDatosServicio = value; }
        }

        public List<ActualizacionDeNotaDeEntrega> ObtenerPrimerasCincoActualizacionesDeNotaDeEntrega(string owner)
        {
            DbParameter[] parameters =
               {
                    new OAParameter
                    {
                        ParameterName = "@OWNER",
                        Value = owner
                    }
                };
            return BaseDeDatosServicio.ExecuteQuery<ActualizacionDeNotaDeEntrega>(BaseDeDatosServicio.Esquema + "OP_WMS_SP_GET_TOP_5_UPDATES_DELIVERY_NOTE", CommandType.StoredProcedure, parameters).ToList();
        }

        public Operacion MarcarActualizacionDeNotaDeEntregaExitosaEnErp(int pickingHeaderId, string respuesta)
        {
            DbParameter[] parameters =
               {
                    new OAParameter
                    {
                        ParameterName = "@POSTED_RESPONSE",
                        Value = respuesta
                    },new OAParameter
                    {
                        ParameterName = "@PICKING_DEMAND_HEADER_ID",
                        Value = pickingHeaderId
                    }
                };
            return BaseDeDatosServicio.ExecuteQuery<Operacion>(BaseDeDatosServicio.Esquema + "OP_WMS_SP_MARK_AS_SENT_DELIVERY_NOTE_UPDATE", CommandType.StoredProcedure, parameters).ToList().FirstOrDefault();
        }

        public Operacion MarcarActualizacionDeNotaDeEntregaFallidaEnErp(int pickingHeaderId, string respuesta)
        {
            DbParameter[] parameters =
               {
                    new OAParameter
                    {
                        ParameterName = "@POSTED_RESPONSE",
                        Value = respuesta
                    },new OAParameter
                    {
                        ParameterName = "@PICKING_DEMAND_HEADER_ID",
                        Value = pickingHeaderId
                    }
                };
            return BaseDeDatosServicio.ExecuteQuery<Operacion>(BaseDeDatosServicio.Esquema + "OP_WMS_SP_MARK_AS_FAILED_DELIVERY_NOTE_UPDATE", CommandType.StoredProcedure, parameters).ToList().FirstOrDefault();
        }
    }
}
