using System.Collections.Generic;
using MobilityScm.Modelo.Entidades;
using MobilityScm.Vertical.Entidades;

namespace MobilityScm.Modelo.Servicios
{
    public interface IFacturaServicio
    {

        Operacion MarcarComoEnviadaFacturaASbo(string idFactura, string resultadoEnvio, string referenciaErp);
        Operacion MarcarComoErradoFacturaASbo(string idFactura, string resultadoEnvio);
        List<SondaFacturaEncabezado> ObtenerPrimerasCincoFacturas();
        SondaFacturaEncabezado ObtenerSondaFactura(string idFactura);
    }
}