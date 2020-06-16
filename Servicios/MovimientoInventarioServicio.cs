using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using MobilityScm.Modelo.Entidades;
using MobilityScm.Modelo.Interfaces.Servicios;
using MobilityScm.Vertical.Entidades;
using System.Data.Common;
using MobilityScm.Modelo.Estados;
using MobilityScm.Modelo.Tipos;
using Telerik.OpenAccess.Data.Common;

namespace MobilityScm.Modelo.Servicios
{
    public class MovimientoInventarioMasterPackServicio : IMovimientoInventarioMasterPackServicio
    {
        #region Conexión BD
        private IBaseDeDatosServicio _baseDeDatosServicio;

        public IBaseDeDatosServicio BaseDeDatosServicio
        {
            get { return _baseDeDatosServicio; }
            set { _baseDeDatosServicio = value; }
        }
        #endregion

        public Operacion MarcarComoEnviadaExplosionMasterPackAErp(int masterPackHeader, string resultadoEnvio, string referenciaErp, int esImplosion)
        {
            DbParameter[] parameters =
             {
                new OAParameter
                {
                    ParameterName = "@MASTER_PACK_HEADER_ID",
                    Value = masterPackHeader
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
                    ParameterName = "@IS_IMPLOSION",
                    Value = esImplosion
                }
            };


            try
            {
                BaseDeDatosServicio.ExecuteQuery<Operacion>(BaseDeDatosServicio.Esquema + "OP_WMS_SP_MARK_MASTER_PACK_AS_SEND_TO_ERP", CommandType.StoredProcedure, parameters);
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

        public Operacion MarcarComoErradaExplosionMasterPack(int masterPackHeader, string resultadoEnvio)
        {
            DbParameter[] parameters =
            {
                new OAParameter
                {
                    ParameterName = "@MASTER_PACK_HEADER_ID",
                    Value = masterPackHeader
                },
                new OAParameter
                {
                    ParameterName = "@POSTED_RESPONSE",
                    Value = resultadoEnvio
                }
             };

            try
            {

                BaseDeDatosServicio.ExecuteQuery<Operacion>(BaseDeDatosServicio.Esquema + "OP_WMS_SP_MARK_MASTER_PACK_AS_FAILED_TO_ERP", CommandType.StoredProcedure, parameters);
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

        public List<MasterPackHeader> ObtenerPrimerosCincoMasterPackExplotados(string dueño)
        {
            DbParameter[] parameters =
            {
                new OAParameter
                {
                    ParameterName = "@OWNER",
                    Value = dueño
                }
            };
            return BaseDeDatosServicio.ExecuteQuery<MasterPackHeader>(BaseDeDatosServicio.Esquema + "OP_WMS_SP_GET_TOP5_MASTER_PACK_EXPLOTION", CommandType.StoredProcedure, parameters).ToList();
        }

        public List<MasterPackHeader> ObtenerPrimerosCincoMasterPackExplotadosFallidos(string dueño)
        {
            DbParameter[] parameters =
            {
                new OAParameter
                {
                    ParameterName = "@OWNER",
                    Value = dueño
                }
            };
            return BaseDeDatosServicio.ExecuteQuery<MasterPackHeader>(BaseDeDatosServicio.Esquema + "OP_WMS_SP_GET_TOP5_FAILED_MASTER_PACK_EXPLOTION", CommandType.StoredProcedure, parameters).ToList();
        }

        public MasterPackTransaction ObtenerTransaccionDeInventarioExplosionMasterPack(int masterPackHeader, int esImplosion)
        {
            var transaccion = esImplosion == (int) SiNo.No
                ? new MasterPackTransaction
                {
                    EncabezadoTransaccion = ObtenerEncabezadoExplosionMasterPack(masterPackHeader)
                    ,
                    ListadoEntradaInventario = ObtenerTransaccionEntradaMasterPack(masterPackHeader)
                    ,
                    ListadoSalidaInventario = ObtenerTransaccionSalidaMasterPack(masterPackHeader)
                }
                : new MasterPackTransaction
                {
                    EncabezadoTransaccion = ObtenerEncabezadoExplosionMasterPack(masterPackHeader)
                    ,
                    ListadoEntradaInventario = ObtenerTransaccionSalidaMasterPack(masterPackHeader)
                    ,
                    ListadoSalidaInventario = ObtenerTransaccionEntradaMasterPack(masterPackHeader)
                };
            return transaccion;
        }

        public List<InventarioDetalle> ObtenerTransaccionEntradaMasterPack(int masterPackHeader)
        {

            DbParameter[] parameters =
        {
                  new OAParameter
                  {
                      ParameterName = "@MASTER_PACK_HEADER_ID",
                      Value = masterPackHeader
                  }
              };
            var detalle =
                BaseDeDatosServicio.ExecuteQuery<InventarioDetalle>(BaseDeDatosServicio.Esquema + "OP_WMS_SP_GET_IN_TRANSACTION_DETAIL_ERP_BY_MASTER_PACK",
                    CommandType.StoredProcedure, parameters);
            return detalle.ToList();
        }

        public List<InventarioDetalle> ObtenerTransaccionSalidaMasterPack(int masterPackHeader)
        {
            DbParameter[] parameters =
        {
                  new OAParameter
                  {
                      ParameterName = "@MASTER_PACK_HEADER_ID",
                      Value = masterPackHeader
                  }
              };
            var detalle =
                BaseDeDatosServicio.ExecuteQuery<InventarioDetalle>(BaseDeDatosServicio.Esquema + "OP_WMS_SP_GET_OUT_TRANSACTION_DETAIL_ERP_BY_MASTER_PACK",
                    CommandType.StoredProcedure, parameters);
            return detalle.ToList();
        }

        public TransaccionInventario ObtenerEncabezadoExplosionMasterPack(int masterPackHeader)
        {
            DbParameter[] parameters =
          {
                  new OAParameter
                  {
                      ParameterName = "@MASTER_PACK_HEADER_ID",
                      Value = masterPackHeader
                  }
              };
            var encabezado =
                BaseDeDatosServicio.ExecuteQuery<TransaccionInventario>(BaseDeDatosServicio.Esquema + "OP_WMS_SP_GET_INVENTORY_TRANSACTION_HEADER_ERP_BY_MASTER_PACK",
                    CommandType.StoredProcedure, parameters).FirstOrDefault();
            return encabezado;
        }

        private TransaccionInventario ObtenerEncabezadoImplosionMasterPackPicking(int pickingId)
        {
            DbParameter[] parameters =
            {
                new OAParameter
                {
                    ParameterName = "@PICKING_DEMAND_HEADER_ID",
                    Value = pickingId
                }
            };
            var encabezado =
                BaseDeDatosServicio.ExecuteQuery<TransaccionInventario>(BaseDeDatosServicio.Esquema + "OP_WMS_SP_GET_INVENTORY_TRANSACTION_HEADER_ERP_BY_PICKING",
                    CommandType.StoredProcedure, parameters).FirstOrDefault();
            return encabezado;
        }

        private List<InventarioDetalle> ObtenerMasterPackDelPicking(int pickingId)
        {
            DbParameter[] parameters =
            {
                new OAParameter
                {
                    ParameterName = "@PICKING_DEMAND_HEADER_ID",
                    Value = pickingId
                }
            };
            var detalle =
                BaseDeDatosServicio.ExecuteQuery<InventarioDetalle>(BaseDeDatosServicio.Esquema + "OP_WMS_SP_GET_PICKING_MASTERPACK",
                    CommandType.StoredProcedure, parameters);
            return detalle.ToList();
        }

        private List<InventarioDetalle> ObtenerComponentesMasterPackDelPicking(int pickingId)
        {
            DbParameter[] parameters =
            {
                new OAParameter
                {
                    ParameterName = "@PICKING_DEMAND_HEADER_ID",
                    Value = pickingId
                }
            };
            var detalle =
                BaseDeDatosServicio.ExecuteQuery<InventarioDetalle>(BaseDeDatosServicio.Esquema + "OP_WMS_SP_GET_PICKING_MASTERPACK_DETAIL",
                    CommandType.StoredProcedure, parameters);
            return detalle.ToList();
        }

        public MasterPackTransaction ObtenerTransaccionDeInventarioImplosionMasterPackPicking(int pickingId)
        {
            var transaccion = new MasterPackTransaction
            {
                EncabezadoTransaccion = ObtenerEncabezadoImplosionMasterPackPicking(pickingId)
                ,
                ListadoEntradaInventario = ObtenerMasterPackDelPicking(pickingId)
                ,
                ListadoSalidaInventario = ObtenerComponentesMasterPackDelPicking(pickingId)
            };
            return transaccion;
        }
    }
}
