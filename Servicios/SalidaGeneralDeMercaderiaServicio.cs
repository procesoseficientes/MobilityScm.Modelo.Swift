using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using MobilityScm.Modelo.Entidades;
using MobilityScm.Modelo.Estados;
using MobilityScm.Modelo.Interfaces.Servicios;
using MobilityScm.Vertical.Entidades;
using Telerik.OpenAccess.Data.Common;

namespace MobilityScm.Modelo.Servicios
{
    public class SalidaGeneralDeMercaderiaServicio : ISalidaGeneralDeMercaderiaServicio
    {
        public IBaseDeDatosServicio _baseDeDatosServicio { get; set; }

        public IBaseDeDatosServicio BaseDeDatosServicio
        {
            get { return _baseDeDatosServicio; }
            set { _baseDeDatosServicio = value; }
        }

        public IList<TransaccionInventario> ObtenerPrimerasCincoSalidasGeneralesDeMercaderia(string owner)
        {
            DbParameter[] parameters =
               {
                    new OAParameter
                    {
                        ParameterName = "@OWNER",
                        Value = owner
                    }
                };
            var salidas = BaseDeDatosServicio.ExecuteQuery<TransaccionInventario>(BaseDeDatosServicio.Esquema + "OP_WMS_SP_GET_TOP5_INVENTORY_GENERAL_EXIT", CommandType.StoredProcedure, parameters).ToList();
            foreach (var salida in salidas)
            {
                salida.Detalle = ObtenerDetalle(salida.DocNum);
            }
            return salidas;
        }

        private List<InventarioDetalle> ObtenerDetalle(string idSalida)
        {
            DbParameter[] parameters =
            {
                new OAParameter
                {
                    ParameterName = "@GENERAL_EXIT_ID",
                    Value = idSalida
                }
            };
            return
                BaseDeDatosServicio.ExecuteQuery<InventarioDetalle>(BaseDeDatosServicio.Esquema + "OP_WMS_SP_GET_GENERAL_EXIT_DETAIL",CommandType.StoredProcedure, parameters)
                    .ToList();
        }

        public Operacion MarcarComoEnvioExitoso(int idSalida, string respuesta, string referencia)
        {
            DbParameter[] parameters =
            {
                new OAParameter
                {
                    ParameterName = "@GENERAL_EXIT_ID",
                    Value = idSalida
                },
                new OAParameter
                {
                    ParameterName = "@POSTED_RESPONSE",
                    Value = respuesta
                },
                new OAParameter
                {
                    ParameterName = "@ERP_REFERENCE",
                    Value = referencia
                }
            };
            return
                BaseDeDatosServicio.ExecuteQuery<Operacion>(BaseDeDatosServicio.Esquema + "OP_WMS_SP_MARK_AS_SENT_GENERAL_EXIT",CommandType.StoredProcedure, parameters)
                    .ToList()
                    .FirstOrDefault();
        }

        public Operacion MarcarComoEnvioFallido(int idSalida, string respuesta)
        {
            DbParameter[] parameters =
            {
                new OAParameter
                {
                    ParameterName = "@GENERAL_EXIT_ID",
                    Value = idSalida
                },
                new OAParameter
                {
                    ParameterName = "@POSTED_RESPONSE",
                    Value = respuesta
                }
            };
            return
                BaseDeDatosServicio.ExecuteQuery<Operacion>(BaseDeDatosServicio.Esquema + "OP_WMS_SP_MARK_AS_FAILED_GENERAL_EXIT", CommandType.StoredProcedure, parameters)
                    .ToList()
                    .FirstOrDefault();
        }
    }
}