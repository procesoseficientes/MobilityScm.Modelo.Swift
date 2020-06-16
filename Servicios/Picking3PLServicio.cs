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
    public class Picking3PLServicio : IPickingServicio
    {

        public IBaseDeDatosServicio _baseDeDatosServicio { get; set; }

        public IBaseDeDatosServicio BaseDeDatosServicio
        {
            get { return _baseDeDatosServicio; }
            set { _baseDeDatosServicio = value; }
        }
        
        public Operacion MarcarComoEnviadoPickingAErp(string pickingHeader, string resultadoEnvio, string referenciaErp, int etapaPosteo, string owner, bool factura)
        {
            DbParameter[] parameters =
            {
                new OAParameter
                {
                    ParameterName = "@PICKING_DEMAND_HEADER_ID",
                    Value = pickingHeader
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
                    ParameterName = "@POSTED_STATUS",
                    Value = etapaPosteo
                },
                new OAParameter
                {
                    ParameterName = "@OWNER",
                    Value = owner
                },
                new OAParameter
                {
                    ParameterName = "@IS_INVOICE",
                    Value = factura ? SiNo.Si : SiNo.No
                }
            };


            try
            {
                BaseDeDatosServicio.ExecuteQuery<Operacion>(BaseDeDatosServicio.Esquema + "OP_WMS_SP_MARK_PICKING_AS_SEND_TO_ERP", CommandType.StoredProcedure, parameters);
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

        public Operacion MarcarComoErradoPickingAErp(string pickingHeader, string resultadoEnvio, int etapaPosteo, string owner, bool factura)
        {
            DbParameter[] parameters =
            {
                new OAParameter
                {
                    ParameterName = "@PICKING_DEMAND_HEADER_ID",
                    Value = pickingHeader
                },
                new OAParameter
                {
                    ParameterName = "@POSTED_RESPONSE",
                    Value = resultadoEnvio
                },
                new OAParameter
                {
                    ParameterName = "@POSTED_STATUS",
                    Value = etapaPosteo
                },
                new OAParameter
                {
                    ParameterName = "@OWNER",
                    Value = owner
                }
            };

            try
            {

                BaseDeDatosServicio.ExecuteQuery<Operacion>(BaseDeDatosServicio.Esquema + "OP_WMS_SP_MARK_PICKING_AS_FAILED_TO_ERP", CommandType.StoredProcedure, parameters);
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

        public Operacion CambiarEstadoCompraVentaPicking(string pickingHeader, string referencia, string estadoVentaInterna, string owner)
        {
            DbParameter[] parameters =
            {
                new OAParameter
                {
                    ParameterName = "@PICKING_DEMAND_HEADER_ID",
                    Value = pickingHeader
                },
                new OAParameter
                {
                    ParameterName = "@ERP_REFERENCE",
                    Value = referencia
                },
                new OAParameter
                {
                    ParameterName = "@INNER_SALE_STATUS",
                    Value = estadoVentaInterna
                },
                new OAParameter
                {
                    ParameterName = "@OWNER",
                    Value = owner
                }
            };

            try
            {

                BaseDeDatosServicio.ExecuteQuery<Operacion>(BaseDeDatosServicio.Esquema + "OP_WMS_SP_SET_PICKING_INTERNAL_SALE_STATUS", CommandType.StoredProcedure, parameters);
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

        public SwiftPickingEncabezado ObtenerPicking(string pickingHeader, string owner, bool factura)
        {
            DbParameter[] parameters =
           {
                new OAParameter
                {
                    ParameterName = "@PICKING_DEMAND_HEADER_ID",
                    Value = pickingHeader
                },new OAParameter
                {
                    ParameterName = "@IS_INVOICE",
                    Value = factura ? SiNo.Si : SiNo.No
                },new OAParameter
                {
                    ParameterName = "@OWNER",
                    Value = owner
                }
            };
            try
            {
                var picking =
                BaseDeDatosServicio.ExecuteQuery<SwiftPickingEncabezado>(BaseDeDatosServicio.Esquema + "OP_WMS_GET_PICKING_DOCUMENT", CommandType.StoredProcedure, parameters).FirstOrDefault();
                

                if (picking != null)
                {
                    picking.SwiftExpressPickingDetalle = ObtenerSwiftExpressPickingDetalle(picking, owner, factura);
                    if(factura == false) picking.SwiftPickingSeries = ObtenerSwiftPickingSeries(picking);
                }
                return picking;
            }
            catch (Exception exe)
            {
                return null;
            }
        }

        private List<SwiftPickingSerie> ObtenerSwiftPickingSeries(SwiftPickingEncabezado picking)
        {
            if (picking.DocEntry == "" || picking.Classification == Enums.GetStringValue(ClasificacionPickingTipo.PickingPorVentaWMS)) return new List<SwiftPickingSerie>();
            DbParameter[] parameters = {
                                           new OAParameter
                                           {
                                               ParameterName = "@PICKING_HEADER",
                                               Value =picking.DocEntry
                                           }
                                       };


            var nombreSp = "";
            if (picking.Classification == Enums.GetStringValue(ClasificacionPickingTipo.PickingPorVenta))
            {
                nombreSp = "SWIFT_SP_GET_ERP_SOS";
            }

            if (picking.Classification == Enums.GetStringValue(ClasificacionPickingTipo.PickingPorTraslado))
            {
                nombreSp = "SWIFT_SP_GET_ERP_ITRS";
            }

            var pickingSeries = BaseDeDatosServicio.ExecuteQuery<SwiftPickingSerie>(nombreSp,
            CommandType.StoredProcedure, parameters);

            return pickingSeries.ToList();
        }

        private List<SwiftExpressPickingDetalle> ObtenerSwiftExpressPickingDetalle(SwiftPickingEncabezado picking, string owner, bool factura) 
        {

            if (picking.DocNum == "") return new List<SwiftExpressPickingDetalle>();
            DbParameter[] parameters =
            {
                new OAParameter
                {
                    ParameterName = "@PICKING_DEMAND_HEADER_ID",
                    Value = picking.DocNum
                },new OAParameter
                {
                    ParameterName = "@OWNER",
                    Value = owner
                }
            };
            
            try
            {
                var pickingDetalle =
                BaseDeDatosServicio.ExecuteQuery<SwiftExpressPickingDetalle>(BaseDeDatosServicio.Esquema + "OP_WMS_SP_GET_NEXT_PICKING_DEMAND_DETAIL",
                    CommandType.StoredProcedure, parameters);

                return pickingDetalle.ToList();
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public List<SwiftExpressPickingDetalle> ObtenerDetalleVentaInterna(int pickingHeader, string owner, string ownerPara, bool facturaDeVenta)
        {
            DbParameter[] parameters =
            {
                new OAParameter
                {
                    ParameterName = "@PICKING_DEMAND_HEADER_ID",
                    Value = pickingHeader
                },new OAParameter
                {
                    ParameterName = "@OWNER",
                    Value = owner
                },new OAParameter
                {
                    ParameterName = "@OWNER_FOR",
                    Value = ownerPara
                },new OAParameter
                {
                    ParameterName = "@IS_SALE_INVOICE",
                    Value = facturaDeVenta ? SiNo.Si : SiNo.No
                }
            };

            try
            {
                var pickingDetalle =
                BaseDeDatosServicio.ExecuteQuery<SwiftExpressPickingDetalle>(BaseDeDatosServicio.Esquema + "OP_WMS_SP_GET_DETAIL_FOR_PICKING_INTERNAL_SALE",
                    CommandType.StoredProcedure, parameters);

                return pickingDetalle.ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<SwiftPickingEncabezado> ObtenerPrimerosCincoDocumentosDePicking(string owner, bool factura)
        {
            DbParameter[] parameters =
               {
                    new OAParameter
                    {
                        ParameterName = "@OWNER",
                        Value = owner
                    },new OAParameter
                    {
                        ParameterName = "@IS_INVOICE",
                        Value = factura ? SiNo.Si : SiNo.No
                    }
                };
            return BaseDeDatosServicio.ExecuteQuery<SwiftPickingEncabezado>(BaseDeDatosServicio.Esquema + "OP_WMS_SP_GET_TOP5_PICKING_DOCUMENT", CommandType.StoredProcedure, parameters).ToList();
        }

        public List<SwiftPickingEncabezado> ObtenerPrimerosCincoDocumentosDePickingFallidos(string owner, bool factura)
        {
            DbParameter[] parameters =
               {
                    new OAParameter
                    {
                        ParameterName = "@OWNER",
                        Value = owner
                    }, new OAParameter
                    {
                        ParameterName = "@IS_INVOICE",
                        Value = factura ? SiNo.Si : SiNo.No
                    }
                };
            return BaseDeDatosServicio.ExecuteQuery<SwiftPickingEncabezado>(BaseDeDatosServicio.Esquema + "OP_WMS_SP_GET_TOP5_FAILED_PICKING_DOCUMENT", CommandType.StoredProcedure, parameters).ToList();
        }

        public List<SwiftPickingEncabezado> ObtenerFacturaDeCompraVentaParaPicking(string owner, int pickingHeader, string internalSaleOwner)
        {
            DbParameter[] parameters =
            {
                new OAParameter
                {
                    ParameterName = "@PICKING_HEADER_ID",
                    Value = pickingHeader
                },new OAParameter
                {
                    ParameterName = "@OWNER",
                    Value = owner
                },new OAParameter
                {
                    ParameterName = "@INTERNAL_SALE_OWNER",
                    Value = internalSaleOwner
                }
            };
            return BaseDeDatosServicio.ExecuteQuery<SwiftPickingEncabezado>(BaseDeDatosServicio.Esquema + "OP_WMS_SP_GET_SALES_INVOICE_HEADER_FOR_INTERNAL_SALE", CommandType.StoredProcedure, parameters).ToList();
        }
    }
}
