using MobilityScm.Modelo.Entidades;
using MobilityScm.Vertical.Entidades;
using System.Collections.Generic;

namespace MobilityScm.Modelo.Interfaces.Servicios
{
    public interface IRecepcionServicio
    {

        Operacion MarcarComoEnviadaRecepcionAErp(string receptionHeader, string resultadoEnvio, string referenciaErp);
        Operacion MarcarComoErradaRecepcionAErp(string receptionHeader, string resultadoEnvio);
        SwiftRecepcionEncabezado ObtenerRecepcion(string receptionHeader, string dueño);        
        List<SwiftRecepcionEncabezado> ObtenerPrimerosCincoDocumentosDeRecepcion(string dueño);
        List<SwiftRecepcionEncabezado> ObtenerPrimerosCincoDocumentosDeRecepcionFallidos(string dueño);

        List<SwiftRecepcionDetalle> SwiftRecepcionDetalle(string docEntry);

        List<SwiftRecepcionSerie> SwiftRecepcionSeries(string docEntry);

    }
}