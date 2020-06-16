using MobilityScm.Modelo.Entidades;
using MobilityScm.Vertical.Entidades;

namespace MobilityScm.Modelo.Interfaces.Servicios
{
    public interface IPickingTrasladoServicio
    {

        SwiftPickingEncabezado ObtenerPicking(string pickingHeader);
        SwiftPickingEncabezado ObtenerPickingReabastecimientoLp(string pickingHeader);
    }
}