using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MobilityScm.Modelo.Entidades;
using MobilityScm.Vertical.Entidades;

namespace MobilityScm.Modelo.Interfaces.Servicios
{
    public interface IEntradaGeneralDeMercaderiaServicio
    {
        IList<TransaccionInventario> ObtenerPrimerasCincoEntradasGeneralesDeMercaderia(string owner);

        Operacion MarcarComoEnvioExitoso(int idEntrada, string respuesta, string referencia);

        Operacion MarcarComoEnvioFallido(int idEntrada, string respuesta);
    }
}
