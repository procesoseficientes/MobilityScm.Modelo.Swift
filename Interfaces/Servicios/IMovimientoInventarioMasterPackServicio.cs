using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MobilityScm.Modelo.Entidades;
using MobilityScm.Vertical.Entidades;

namespace MobilityScm.Modelo.Interfaces.Servicios
{
    public interface IMovimientoInventarioMasterPackServicio
    {
        Operacion MarcarComoEnviadaExplosionMasterPackAErp(int masterPackHeader, string resultadoEnvio, string referenciaErp, int esImplosion);
        Operacion MarcarComoErradaExplosionMasterPack(int masterPackHeader, string resultadoEnvio);
        MasterPackTransaction ObtenerTransaccionDeInventarioExplosionMasterPack(int masterPackHeader, int esImplosion);
        List<MasterPackHeader> ObtenerPrimerosCincoMasterPackExplotados(string dueño);
        List<MasterPackHeader> ObtenerPrimerosCincoMasterPackExplotadosFallidos(string dueño);
        List<InventarioDetalle> ObtenerTransaccionEntradaMasterPack(int masterPackHeader);
        List<InventarioDetalle> ObtenerTransaccionSalidaMasterPack(int masterPackHeader);
        TransaccionInventario ObtenerEncabezadoExplosionMasterPack(int masterPackHeader);
        
        MasterPackTransaction ObtenerTransaccionDeInventarioImplosionMasterPackPicking(int pickingId);
    }
}
