using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using MobilityScm.Modelo.Entidades;
using MobilityScm.Modelo.Interfaces.Servicios;
using MobilityScm.Vertical.Entidades;
using Telerik.OpenAccess.Data.Common;

namespace MobilityScm.Modelo.Servicios
{
    public class EntradaGeneralDeMercaderiaServicio : IEntradaGeneralDeMercaderiaServicio
    {
        public IBaseDeDatosServicio _baseDeDatosServicio { get; set; }

        public IBaseDeDatosServicio BaseDeDatosServicio
        {
            get { return _baseDeDatosServicio; }
            set { _baseDeDatosServicio = value; }
        }

        public IList<TransaccionInventario> ObtenerPrimerasCincoEntradasGeneralesDeMercaderia(string owner)
        {
            DbParameter[] parameters =
               {
                    new OAParameter
                    {
                        ParameterName = "@OWNER",
                        Value = owner
                    }
                };
            var entradas = BaseDeDatosServicio.ExecuteQuery<TransaccionInventario>(BaseDeDatosServicio.Esquema + "OP_WMS_SP_GET_TOP5_INVENTORY_GENERAL_ENTRY", CommandType.StoredProcedure, parameters).ToList();
            foreach (var entrada in entradas)
            {
                entrada.Detalle = ObtenerDetalle(entrada.DocNum);
            }
            return entradas;
        }

        private List<InventarioDetalle> ObtenerDetalle(string idEntrada)
        {
            DbParameter[] parameters =
            {
                new OAParameter
                {
                    ParameterName = "@GENERAL_ENTRY_ID",
                    Value = idEntrada
                }
            };
            return
                BaseDeDatosServicio.ExecuteQuery<InventarioDetalle>(BaseDeDatosServicio.Esquema + "OP_WMS_SP_GET_GENERAL_ENTRY_DETAIL", CommandType.StoredProcedure, parameters)
                    .ToList();
        }

        public Operacion MarcarComoEnvioExitoso(int idEntrada, string respuesta, string referencia) {
            DbParameter[] parameters =
            {
                new OAParameter
                {
                    ParameterName = "@RECEPTION_DOCUMENT_ID",
                    Value = idEntrada
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
                BaseDeDatosServicio.ExecuteQuery<Operacion>(BaseDeDatosServicio.Esquema + "OP_WMS_SP_MARK_RECEPTION_AS_SEND_TO_ERP", CommandType.StoredProcedure, parameters)
                    .ToList()
                    .FirstOrDefault();
        }

        public Operacion MarcarComoEnvioFallido(int idEntrada, string respuesta)
        {
            DbParameter[] parameters =
            {
                new OAParameter
                {
                    ParameterName = "@RECEPTION_DOCUMENT_ID",
                    Value = idEntrada
                },
                new OAParameter
                {
                    ParameterName = "@POSTED_RESPONSE",
                    Value = respuesta
                }
            };
            return
                BaseDeDatosServicio.ExecuteQuery<Operacion>(BaseDeDatosServicio.Esquema + "OP_WMS_SP_MARK_RECEPTION_AS_FAILED_TO_ERP", CommandType.StoredProcedure, parameters)
                    .ToList()
                    .FirstOrDefault();
        }
    }
}
