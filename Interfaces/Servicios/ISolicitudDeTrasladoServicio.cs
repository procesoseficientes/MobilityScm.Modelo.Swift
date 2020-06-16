using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MobilityScm.Modelo.Entidades;
using MobilityScm.Vertical.Entidades;

namespace MobilityScm.Modelo.Interfaces.Servicios
{
    public interface ISolicitudDeTrasladoServicio
    {
        Operacion MarcarComoEnviadoSolicituDeTrasladoAErp(string transferRequestId, string resultadoEnvio, string referenciaErp, string owner);
        Operacion MarcarComoErradoSolicitudDeTrasladoAErp(string transferRequestId, string resultadoEnvio, string owner);
        SwiftSolicitudDeTrasladoEncabezado ObtenerSolicitudDeTraslado(string transferRequestId, string owner);
        List<SwiftRecepcionEncabezado> ObtenerPrimerosCincoDocumentosDeSolicitudDeTraslado(string owner);
    }
}
