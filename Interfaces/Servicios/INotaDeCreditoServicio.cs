using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MobilityScm.Modelo.Entidades;
using MobilityScm.Vertical.Entidades;

namespace MobilityScm.Modelo.Interfaces.Servicios
{
    public interface INotaDeCreditoServicio
    {
        Operacion MarcarComoEnviadaNotaDeCreditoAErp(string idDeDocumentoDeRecepcion, string resultadoEnvio, string referenciaErp, string owner, string tabla);
        Operacion MarcarComoErradaNotaDeCreditoAErp(string idDeDocumentoDeRecepcion, string resultadoEnvio, string owner);
        SwiftNotaDeCreditoEncabezado ObtenerNotaDeCredito(string idDeDocumentoDeRecepcion, string owner);
        List<SwiftNotaDeCreditoEncabezado> ObtenerPrimerasCincoNotasDeCredito(string owner);
    }
}
