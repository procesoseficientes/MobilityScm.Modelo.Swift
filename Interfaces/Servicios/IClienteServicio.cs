using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MobilityScm.Modelo.Entidades;
using MobilityScm.Vertical.Entidades;

namespace MobilityScm.Modelo.Interfaces.Servicios
{
    public interface IClienteServicio
    {
        Operacion MarcarComoEnviadoClienteASbo(string idCustomer, string postedResponse, string idCustomerBo);
        Operacion MarcarComoErradoClienteASbo(string idCustomer, string postedResponse);

        List<Cliente> ObtenerPrimerosCincoClientes(string owner);
        
        Cliente ObtenerCliente(string idCustomer);
        Operacion MarcarComoEnviadoModificacion(string idCustomer, string postedResponse);
        Operacion MarcarComoErradaModificacion(string idCustomer, string postedResponse);
        List<CambiosCliente> ObtenerPrimerasCincoModificacones(string owner);
        CambiosCliente ObtenerModificacion(string idCustomer);
    }
}
