using MobilityScm.Modelo.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MobilityScm.Modelo;
using MobilityScm.Vertical.Entidades;

namespace MobilityScm.Modelo.Interfaces.Servicios
{
    /// <summary>
    /// Interfaz de servicio para almacenadora
    /// </summary>
    public interface IAlmacenadora
    {

        #region Picking
        /// <summary>
        /// Marcar un picking como enviado a ERP
        /// </summary>
        /// <param name="pickingHeader"></param>
        /// <param name="resultadoEnvio"></param>
        /// <param name="referenciaErp"></param>
        /// <param name="etapaPosteo"></param>
        /// <param name="owner"></param>
        /// <param name="factura"></param>
        /// <returns></returns>
        string MarcarComoEnviadoPickingASbo(string pickingHeader, string resultadoEnvio, string referenciaErp, int etapaPosteo, string owner = "", bool factura = false);

        /// <summary>
        /// Obtener picking
        /// </summary>
        /// <param name="pickingHeader"></param>
        /// <param name="owner"></param>
        /// <param name="factura"></param>
        /// <returns></returns>
        SwiftPickingEncabezado ObtenerPicking(string pickingHeader, string owner = "", bool factura = false);

        /// <summary>
        /// Obtener picking de reabastecimiento
        /// </summary>
        /// <param name="pickingHeader"></param>
        /// <returns></returns>
        SwiftPickingEncabezado ObtenerPickingReabastecimientoLp(string pickingHeader);

        /// <summary>
        /// Obtener picking de traslado
        /// </summary>
        /// <param name="pickingHeader"></param>
        /// <returns></returns>
        SwiftPickingEncabezado ObtenerPickingTranlado(string pickingHeader);

        /// <summary>
        /// Marcar picking como errado
        /// </summary>
        /// <param name="pickingHeader"></param>
        /// <param name="resultadoEnvio"></param>
        /// <param name="etapaPosteo"></param>
        /// <param name="owner"></param>
        /// <param name="factura"></param>
        /// <returns></returns>
        string MarcarComoErradoPickingASbo(string pickingHeader, string resultadoEnvio, int etapaPosteo, string owner = "", bool factura = false);

        /// <summary>
        /// Cambia estado compra/venta de un picking
        /// </summary>
        /// <param name="pickingHeader"></param>
        /// <param name="referencia"></param>
        /// <param name="estadoVentaInterna"></param>
        /// <param name="owner"></param>
        /// <returns></returns>
        string CambiarEstadoCompraVentaPicking(string pickingHeader, string referencia, string estadoVentaInterna, string owner);

        /// <summary>
        /// Obtener primeros 5 documentos de picking
        /// </summary>
        /// <returns></returns>
        List<SwiftPickingEncabezado> ObtenerPrimerosCincoDocumentosDePicking(string owner = "", bool factura = false);

        /// <summary>
        /// Obtener primeros 5 documentos de picking
        /// </summary>
        /// <returns></returns>
        List<SwiftPickingEncabezado> ObtenerPrimerosCincoDocumentosDePickingFallidos(string owner = "", bool factura = false);

        /// <summary>
        /// Obtener los encabezados para las facturas de compraventa.
        /// </summary>
        /// <returns></returns>
        List<SwiftPickingEncabezado> ObtenerFacturaDeCompraVentaParaPicking(string owner, int pickingHeader, string internalSaleOwner);

        /// <summary>
        /// Obtiene los detalles de venta interna.
        /// </summary>
        /// <returns></returns>
        List<SwiftExpressPickingDetalle> ObtenerDetalleVentaInterna(int pickingHeader, string owner, string ownerPara, bool facturaDeVenta);
        #endregion

        #region Recepcion
        /// <summary>
        /// Marcar como enviada recepción a SBO
        /// </summary>
        /// <param name="receptionHeader"></param>
        /// <param name="resultadoEnvio"></param>
        /// <param name="referenciaErp"></param>
        /// <returns></returns>
        string MarcarComoEnviadaRecepcionASbo(string receptionHeader, string resultadoEnvio, string referenciaErp);

        /// <summary>
        /// Obtener recepcion
        /// </summary>
        /// <param name="receptionHeader"></param>
        /// /// <param name="dueño"></param>
        /// <returns></returns>
        SwiftRecepcionEncabezado ObtenerRecepcion(string receptionHeader, string dueño);

        /// <summary>
        /// Marcar recepción como errada.
        /// </summary>
        /// <param name="receptionHeader"></param>
        /// <param name="resultadoEnvio"></param>
        /// <returns></returns>
        string MarcarComoErradaRecepcionASbo(string receptionHeader, string resultadoEnvio);



        /// <summary>
        /// Obtener primeras cinco recepciones 
        /// </summary>
        /// <returns></returns>
        List<SwiftRecepcionEncabezado> ObtenerPrimerosCincoDocumentosDeRecepcion(string dueño);

        /// <summary>
        /// Obtener primeras cinco recepciones fallidas
        /// </summary>
        /// <returns></returns>
        List<SwiftRecepcionEncabezado> ObtenerPrimerosCincoDocumentosDeRecepcionFallidos(string dueño);


        #endregion

        #region MasterPack

        /// <summary>
        /// Marcar como enviada recepción a SBO
        /// </summary>
        /// <param name="masterPackHeader"></param>
        /// <param name="resultadoEnvio"></param>
        /// <param name="referenciaErp"></param>
        /// <param name="esImplosion"></param>
        /// <returns></returns>
        string MarcarComoEnviadaExplosionMasterPackAErp(int masterPackHeader, string resultadoEnvio, string referenciaErp, int esImplosion);

        /// <summary>
        /// Obtener recepcion
        /// </summary>
        /// <param name="masterPackHeader"></param>
        /// <param name="esImplosion"></param>
        /// <returns></returns>
        MasterPackTransaction ObtenerTransaccionDeInventarioExplosionMasterPack(int masterPackHeader, int esImplosion);

        /// <summary>
        /// Marcar recepción como errada.
        /// </summary>
        /// <param name="masterPackHeader"></param>
        /// <param name="resultadoEnvio"></param>
        /// <returns></returns>
        string MarcarComoErradaExplosionMasterPack(int masterPackHeader, string resultadoEnvio);



        /// <summary>
        /// Obtener primeras cinco recepciones 
        /// </summary>
        /// <returns></returns>
        List<MasterPackHeader> ObtenerPrimerosCincoMasterPackExplotados(string dueño);

        /// <summary>
        /// Obtener primeras cinco recepciones fallidas
        /// </summary>
        /// <returns></returns>
        List<MasterPackHeader> ObtenerPrimerosCincoMasterPackExplotadosFallidos(string dueño);

        /// <summary>
        /// Obtener implosion en picking
        /// </summary>
        /// <param name="pickingId"></param>
        /// <returns></returns>
        MasterPackTransaction ObtenerTransaccionDeInventarioImplosionMasterPackPicking(int pickingId);

        #endregion

        #region Solicitud de Traslado
        string MarcarComoEnviadoSolicitudDeTrasladoASbo(string transferRequestId, string resultadoEnvio, string referenciaErp, string owner);
        string MarcarComoErradoSolicitudDeTrasladoASbo(string transferRequestId, string resultadoEnvio, string owner);
        SwiftSolicitudDeTrasladoEncabezado ObtenerSolicitudDeTraslado(string transferRequestId, string owner);
        List<SwiftRecepcionEncabezado> ObtenerPrimerosCincoDocumentosDeSolicitudDeTraslado(string owner);
        #endregion

        #region Nota de Credito
        string MarcarComoEnviadaNotaDeCreditoAErp(string idDeDocumentoDeRecepcion, string resultadoEnvio, string referenciaErp, string owner, string tabla);
        string MarcarComoErradaNotaDeCreditoAErp(string idDeDocumentoDeRecepcion, string resultadoEnvio, string owner);
        SwiftNotaDeCreditoEncabezado ObtenerNotaDeCredito(string idDeDocumentoDeRecepcion, string owner);
        List<SwiftNotaDeCreditoEncabezado> ObtenerPrimerasCincoNotasDeCredito(string owner);
        #endregion

        #region Nota de Entrega
        List<ActualizacionDeNotaDeEntrega> ObtenerPrimerasCincoActualizacionesDeNotaDeEntrega(string owner);
        string MarcarActualizacionDeNotaDeEntregaExitosaEnErp(int pickingHeaderId, string respuesta);
        string MarcarActualizacionDeNotaDeEntregaFallidaEnErp(int pickingHeaderId, string respuesta);
        #endregion

        #region Entrada General de Mercaderia

        List<TransaccionInventario> ObtenerPrimerasCincoEntradasGeneralesDeMercaderia(string owner);
        string MarcarComoEnviadaEntradaDeMercaderia(int idEntrada, string respuesta, string referencia);
        string MarcarComoFallidaEntradaDeMercaderia(int idEntrada, string respuesta);

        #endregion

        #region Salida General de Mercaderia

        List<TransaccionInventario> ObtenerPrimerasCincoSalidasGeneralesDeMercaderia(string owner);
        string MarcarComoEnviadaSalidaDeMercaderia(int idSalida, string respuesta, string referencia);
        string MarcarComoFallidaSalidaDeMercaderia(int idSalida, string respuesta);

        #endregion
    }
}
