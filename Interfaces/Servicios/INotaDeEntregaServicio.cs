using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MobilityScm.Modelo.Entidades;
using MobilityScm.Vertical.Entidades;

namespace MobilityScm.Modelo.Interfaces.Servicios
{
    public interface INotaDeEntregaServicio
    {
        List<ActualizacionDeNotaDeEntrega> ObtenerPrimerasCincoActualizacionesDeNotaDeEntrega(string owner);
        Operacion MarcarActualizacionDeNotaDeEntregaExitosaEnErp(int pickingHeaderId, string respuesta);
        Operacion MarcarActualizacionDeNotaDeEntregaFallidaEnErp(int pickingHeaderId, string respuesta);
    }
}
