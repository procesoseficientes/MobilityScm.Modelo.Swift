using System.Collections.Generic;
using MobilityScm.Modelo.Entidades;
using MobilityScm.Vertical.Entidades;

namespace MobilityScm.Modelo.Interfaces.Servicios
{
    public interface IPickingServicio
    {
        Operacion MarcarComoEnviadoPickingAErp(string pickingHeader, string resultadoEnvio, string referenciaErp, int etapaPosteo, string owner, bool factura);
        Operacion MarcarComoErradoPickingAErp(string pickingHeader, string resultadoEnvio, int etapaPosteo, string owner, bool factura);
        SwiftPickingEncabezado ObtenerPicking(string pickingHeader, string owner, bool factura);

        List<SwiftPickingEncabezado> ObtenerPrimerosCincoDocumentosDePicking(string owner, bool factura);
        List<SwiftPickingEncabezado> ObtenerPrimerosCincoDocumentosDePickingFallidos(string owner, bool factura);
        List<SwiftPickingEncabezado> ObtenerFacturaDeCompraVentaParaPicking(string owner, int pickingHeader, string internalSaleOwner);
        List<SwiftExpressPickingDetalle> ObtenerDetalleVentaInterna(int pickingHeader, string owner, string ownerPara, bool facturaDeVenta);
        Operacion CambiarEstadoCompraVentaPicking(string pickingHeader, string referencia, string estadoVentaInterna, string owner);
    }
}