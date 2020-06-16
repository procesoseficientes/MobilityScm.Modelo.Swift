using System.Collections;
using System.Collections.Generic;
using MobilityScm.Modelo.Entidades;
using MobilityScm.Vertical.Entidades;

namespace MobilityScm.Modelo.Interfaces.Servicios
{
    public interface IOrdenDeVentaServicio
    {
        SondaOrdenDeVentaEncabezado ObtenerSondaOrdenDeVenta(string idOrdenDeVenta);

        List<SondaOrdenDeVentaEncabezado> ObtenerPrimerasCincoOrdenesDeVenta(string owner);

        List<SondaOrdenDeVentaEncabezado> ObtenerPrimerasCincoOrdenesDeVenta();

        Operacion MarcarComoEnviadaOrdenDeVentaASbo(string idOrdenDeVenta, string resultadoEnvio,string referenciaErp, string owner, string customerOwner);

        Operacion MarcarComoEnviadaOrdenDeVentaASbo(string idOrdenDeVenta, string resultadoEnvio, string referenciaErp);

        Operacion MarcarComoErradoOrdenDeVentaASbo(string idOrdenDeVenta, string resultadoEnvio, string owner, string customerOwner);

        Operacion MarcarComoErradoOrdenDeVentaASbo(string idOrdenDeVenta, string resultadoEnvio);

    }
}