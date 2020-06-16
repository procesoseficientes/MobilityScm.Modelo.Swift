using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using MobilityScm.Modelo.Entidades;
using MobilityScm.Modelo.Interfaces.Servicios;
using MobilityScm.Modelo.Tipos;
using MobilityScm.Vertical.Entidades;
using Telerik.OpenAccess.Data.Common;

namespace MobilityScm.Modelo.Servicios
{
    public class ClienteServicio : IClienteServicio

    {
        public IBaseDeDatosServicio BaseDeDatosServicio { get; set; }

        public Operacion MarcarComoEnviadoClienteASbo(string idCustomer, string postedResponse, string idCustomerBo)
        {
            DbParameter[] parameters =
            {
                 new OAParameter
                 {
                     ParameterName = "@CODE_CUSTOMER",
                     Value = idCustomer
                 },
                 new OAParameter
                 {
                     ParameterName = "@POSTED_RESPONSE",
                     Value = postedResponse
                 },
                   new OAParameter
                 {
                     ParameterName = "@CODE_CUSTOMER_BO",
                     Value = idCustomerBo
                 }
             };


            try
            {
                BaseDeDatosServicio.ExecuteQuery<Operacion>("SWIFT_SP_STATUS_SEND_CUSTOMER_TO_SAP", CommandType.StoredProcedure, parameters);
                return new Operacion
                {
                    Codigo = 0,
                    Mensaje = "Proceso Exitoso",
                    Resultado = ResultadoOperacionTipo.Exito
                };
            }
            catch (Exception e)
            {

                return new Operacion
                {
                    Codigo = -1,
                    Mensaje = e.Message,
                    Resultado = ResultadoOperacionTipo.Error
                };
            }
        }

        public Operacion MarcarComoErradoClienteASbo(string idCustomer, string postedResponse)
        {
            DbParameter[] parameters =
             {
            new OAParameter
            {
            ParameterName = "@CODE_CUSTOMER",
            Value = idCustomer
             },

            new OAParameter
            {
            ParameterName = "@POSTED_RESPONSE",
            Value = postedResponse
             }
             };


            try
            {
                BaseDeDatosServicio.ExecuteQuery<Operacion>("SWIFT_SP_CUSTOMER_ERROR_TO_SEND_SAP", CommandType.StoredProcedure, parameters);
                return new Operacion
                {
                    Codigo = 0,
                    Mensaje = "Proceso Exitoso",
                    Resultado = ResultadoOperacionTipo.Exito
                };
            }
            catch (Exception e)
            {

                return new Operacion
                {
                    Codigo = -1,
                    Mensaje = e.Message,
                    Resultado = ResultadoOperacionTipo.Error
                };
            }
        }

        public List<Cliente> ObtenerPrimerosCincoClientes(string owner)
        {
            DbParameter[] parameters =
            {
                new OAParameter
                {
                    ParameterName = "@OWNER",
                    Value = owner
                }
            };
            
            return BaseDeDatosServicio.ExecuteQuery<Cliente>("SWIFT_SP_GET_TOP5_CUSTOMERS_NEW", CommandType.StoredProcedure, parameters).ToList();

        }

        public List<Cliente> ObtenerPrimerosCincoClientes()
        {
           return BaseDeDatosServicio.ExecuteQuery<Cliente>("SWIFT_SP_GET_TOP5_CUSTOMERS_NEW", CommandType.StoredProcedure, null).ToList();

        }

        public Cliente ObtenerCliente(string idCustomer)
        {
            DbParameter[] parameters =
            {
            new OAParameter
            {
            ParameterName = "@CODE_CUSTOMER",
            Value = idCustomer
             }
             };

            var cliente =
                BaseDeDatosServicio.ExecuteQuery<Cliente>("SWIFT_SP_GET_CUSTOMERS_NEW", CommandType.StoredProcedure,
                    parameters).FirstOrDefault();
            BaseDeDatosServicio.Commit();
            return cliente;
        }

        public Operacion MarcarComoEnviadoModificacion(string idCustomer, string postedResponse)
        {
            DbParameter[] parameters =
            {
                 new OAParameter
                 {
                     ParameterName = "@CUSTOMER",
                     Value = idCustomer
                 },
                 new OAParameter
                 {
                     ParameterName = "@POSTED_RESPONSE",
                     Value = postedResponse
                 }
             };


            try
            {
                BaseDeDatosServicio.ExecuteQuery<Operacion>("SWIFT_SP_SET_STATUS_SEND_CUSTOMER_CHANGE_TO_ERP", CommandType.StoredProcedure, parameters);
                return new Operacion
                {
                    Codigo = 0,
                    Mensaje = "Proceso Exitoso",
                    Resultado = ResultadoOperacionTipo.Exito
                };
            }
            catch (Exception e)
            {

                return new Operacion
                {
                    Codigo = -1,
                    Mensaje = e.Message,
                    Resultado = ResultadoOperacionTipo.Error
                };
            }
        }

        public Operacion MarcarComoErradaModificacion(string idCustomer, string postedResponse)
        {
            DbParameter[] parameters =
           {
            new OAParameter
            {
            ParameterName = "@CUSTOMER",
            Value = idCustomer
             },

            new OAParameter
            {
            ParameterName = "@POSTED_RESPONSE",
            Value = postedResponse
             }
             };


            try
            {
                BaseDeDatosServicio.ExecuteQuery<Operacion>("SWIFT_SP_SET_STATUS_ERROR_CUSTOMER_CHANGE_TO_ERP", CommandType.StoredProcedure, parameters);
                return new Operacion
                {
                    Codigo = 0,
                    Mensaje = "Proceso Exitoso",
                    Resultado = ResultadoOperacionTipo.Exito
                };
            }
            catch (Exception e)
            {

                return new Operacion
                {
                    Codigo = -1,
                    Mensaje = e.Message,
                    Resultado = ResultadoOperacionTipo.Error
                };
            }
        }

        public List<CambiosCliente> ObtenerPrimerasCincoModificacones(string owner)
        {
            DbParameter[] parameters =
            {
                new OAParameter
                {
                    ParameterName = "@OWNER",
                    Value = owner
                }
            };
            return BaseDeDatosServicio.ExecuteQuery<CambiosCliente>("SWIFT_SP_GET_TOP5_CUSTOMER_CHANGE", CommandType.StoredProcedure, parameters).ToList();
        }

        public List<CambiosCliente> ObtenerPrimerasCincoModificacones()
        {
            return BaseDeDatosServicio.ExecuteQuery<CambiosCliente>("SWIFT_SP_GET_TOP5_CUSTOMER_CHANGE", CommandType.StoredProcedure, null).ToList();
        }


        public CambiosCliente ObtenerModificacion(string idCustomer)
        {
            DbParameter[] parameters =
            {
            new OAParameter
            {
            ParameterName = "@CUSTOMER",
            Value = idCustomer
             }
             };

            var cliente =
                BaseDeDatosServicio.ExecuteQuery<CambiosCliente>("SWIFT_SP_GET_CUSTOMER_CHANGE", CommandType.StoredProcedure,
                    parameters).FirstOrDefault();
            BaseDeDatosServicio.Commit();
            return cliente;
        }
    }
}
