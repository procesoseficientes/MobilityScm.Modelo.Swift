using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Runtime.CompilerServices;
using MobilityScm.Modelo.Entidades;
using MobilityScm.Modelo.Interfaces.Servicios;
using MobilityScm.Modelo.Tipos;
using MobilityScm.Vertical.Entidades;
using Telerik.OpenAccess.Data.Common;

namespace MobilityScm.Modelo.Servicios
{
    public class PickingServicio : IPickingServicio
    {
        public IBaseDeDatosServicio BaseDeDatosServicio { get; set; }
        public SwiftPickingEncabezado ObtenerPicking(string pickingHeader, string owner = "", bool factura = false)
        {
            DbParameter[] parameters =
           {
                new OAParameter
                {
                    ParameterName = "@PICKING_HEADER",
                    Value = pickingHeader
                }
            };
            try
            {
                var picking =
                BaseDeDatosServicio.ExecuteQuery<SwiftPickingEncabezado>("SWIFT_SP_GET_ERP_SOH", CommandType.StoredProcedure, parameters).FirstOrDefault();
                BaseDeDatosServicio.Commit();

                if (picking != null)
                {
                    picking.SwiftExpressPickingDetalle = this.SwiftExpressPickingDetalle(picking);
                    picking.SwiftPickingSeries = this.SwiftPickingSeries(picking);
                }



                return picking;
            }
            catch (Exception)
            {
                return null;
            }
            
        }

        private List<SwiftPickingSerie> SwiftPickingSeries(SwiftPickingEncabezado picking)
        {
            if (picking.DocEntry == "") return new List<SwiftPickingSerie>();
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

        private List<SwiftExpressPickingDetalle> SwiftExpressPickingDetalle(SwiftPickingEncabezado picking)
        {
            if (picking.DocEntry == "") return new List<SwiftExpressPickingDetalle>();
            DbParameter[] parameters =
            {
                    new OAParameter
                    {
                        ParameterName = "@PICKING_HEADER",
                        Value = picking.DocEntry
                    }
                };

            var nombreSp = "";
            if (picking.Classification == Enums.GetStringValue(ClasificacionPickingTipo.PickingPorVenta))
            {
                nombreSp = "SWIFT_SP_GET_ERP_SOD";
            }

            if (picking.Classification == Enums.GetStringValue(ClasificacionPickingTipo.PickingPorTraslado))
            {
                nombreSp = "SWIFT_SP_GET_ERP_ITWD";
            }

            try
            {
                var pickingDetalle =
                BaseDeDatosServicio.ExecuteQuery<SwiftExpressPickingDetalle>(nombreSp,
                    CommandType.StoredProcedure, parameters);


                return pickingDetalle.ToList();
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public SwiftPickingEncabezado ObtenerPickingReabastecimientoLp(string pickingHeader)
        {
            DbParameter[] parameters =
                      {
                new OAParameter
                {
                    ParameterName = "@PICKING_HEADER",
                    Value = pickingHeader
                }
            };

            var picking =
                BaseDeDatosServicio.ExecuteQuery<SwiftPickingEncabezado>("SWIFT_SP_GET_RESTOCK_PICKING_HEADER_FOR_ERP",
                    CommandType.StoredProcedure, parameters).FirstOrDefault();
            BaseDeDatosServicio.Commit();

            if (picking != null)
            {
                picking.SwiftExpressPickingDetalle = this.SwiftExpressPickingDetalle(picking);
                picking.SwiftPickingSeries = this.SwiftPickingSeries(picking);
            }

            return picking;
        }


        public Operacion MarcarComoEnviadoPickingAErp(string pickingHeader, string resultadoEnvio, string referenciaErp, int etapaPosteo = 0, string owner = "", bool factura = false)
        {
            DbParameter[] parameters =
             {
            new OAParameter
            {
            ParameterName = "@PICKING_HEADER",
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
             }
             };


            try
            {
                BaseDeDatosServicio.ExecuteQuery<Operacion>("SWIFT_SP_MARK_PICKING_AS_SEND_TO_ERP", CommandType.StoredProcedure, parameters);
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


        public Operacion MarcarComoErradoPickingAErp(string pickingHeader, string resultadoEnvio, int etapaPosteo = 0, string owner = "", bool factura = false)
        {

            DbParameter[] parameters =
             {
            new OAParameter
            {
            ParameterName = "@PICKING_HEADER",
            Value = pickingHeader
             },
              new OAParameter
            {
            ParameterName = "@POSTED_RESPONSE",
            Value = resultadoEnvio
             }
             };

            try
            {

                BaseDeDatosServicio.ExecuteQuery<Operacion>("SWIFT_SP_MARK_PICKING_AS_FAILED_TO_ERP", CommandType.StoredProcedure, parameters);
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

        public List<SwiftPickingEncabezado> ObtenerPrimerosCincoDocumentosDePicking(string owner = "", bool factura= false)
        {
            return BaseDeDatosServicio.ExecuteQuery<SwiftPickingEncabezado>("SWIFT_SP_GET_TOP5_PICKING_DOCUMENT", CommandType.StoredProcedure).ToList();
        }

        public List<SwiftPickingEncabezado> ObtenerPrimerosCincoDocumentosDePickingFallidos(string owner = "", bool factura = false)
        {
            throw new NotImplementedException();
        }

        public List<SwiftPickingEncabezado> ObtenerFacturaDeCompraVentaParaPicking(string owner, int pickingHeader, string internalSaleOwner)
        {
            throw new NotImplementedException();
        }

        public Operacion CambiarEstadoCompraVentaPicking(string pickingHeader, string referencia, string estadoVentaInterna, string owner)
        {
            throw new NotImplementedException();
        }

        public List<SwiftExpressPickingDetalle> ObtenerDetalleVentaInterna(int pickingHeader, string owner, string ownerPara, bool facturaDeVenta)
        {
            throw new NotImplementedException();
        }
    }
}