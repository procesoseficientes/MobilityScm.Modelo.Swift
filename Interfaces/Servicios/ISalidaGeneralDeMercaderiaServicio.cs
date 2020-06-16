using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MobilityScm.Modelo.Entidades;
using MobilityScm.Vertical.Entidades;

namespace MobilityScm.Modelo.Interfaces.Servicios
{
    public interface ISalidaGeneralDeMercaderiaServicio
    {
        IList<TransaccionInventario> ObtenerPrimerasCincoSalidasGeneralesDeMercaderia(string owner);

        Operacion MarcarComoEnvioExitoso(int idSalida, string respuesta, string referencia);

        Operacion MarcarComoEnvioFallido(int idSalida, string respuesta);
    }
}
