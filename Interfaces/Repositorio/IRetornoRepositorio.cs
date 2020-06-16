using MobilityScm.Modelo.Entidades;
using MobilityScm.Vertical.Entidades;

namespace MobilityScm.Modelo.Interfaces.Repositorio
{
    public interface IRetornoRepositorio
    {
        void RegistrarInsidencia(ReturnHeader retorno, Operacion op );
        void MarcarRetornoComoEnviadoAErp(ReturnHeader retorno);
    }
}