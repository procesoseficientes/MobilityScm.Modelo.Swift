using System.Data;
using System.Data.Common;
using System.Linq;
using MobilityScm.Modelo.Entidades;
using MobilityScm.Modelo.Interfaces.Servicios;
using Telerik.OpenAccess.Data.Common;

namespace MobilityScm.Modelo.Servicios
{
    public class PickingTrasladoServicio: IPickingTrasladoServicio
    {
        public IBaseDeDatosServicio BaseDeDatosServicio { get; set; }
        public SwiftPickingEncabezado ObtenerPicking(string pickingHeader)
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
                BaseDeDatosServicio.ExecuteQuery<SwiftPickingEncabezado>("SWIFT_SP_GET_ERP_ITRH",
                    CommandType.StoredProcedure, parameters).FirstOrDefault();
            BaseDeDatosServicio.Commit();
            return picking;
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
            return picking;
        }




    }
}