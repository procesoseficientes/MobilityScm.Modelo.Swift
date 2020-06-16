using System;
using System.Text;
using MobilityScm.Modelo.Entidades;
using MobilityScm.Modelo.Estados;
using MobilityScm.Modelo.Interfaces.Repositorio;
using MobilityScm.Modelo.Tipos;
using MobilityScm.Utilerias;
using MobilityScm.Vertical.Entidades;

namespace MobilityScm.Modelo.Repositorio
{
    public class RetornoRepositorio : IRetornoRepositorio
    {
        private readonly IBaseDeDatosServicio _baseDeDatosServicio;

        public RetornoRepositorio()
        {
            _baseDeDatosServicio = Mvx.Ioc.Resolve<IBaseDeDatosServicio>();
        }

        public void RegistrarInsidencia(ReturnHeader retorno, Operacion op )
        {
            _baseDeDatosServicio.BeginTransaction();

            try
            {
                var sb = new StringBuilder();
                sb.Append(" Update SWIFT_RECEPTION_HEADER");
                sb.Append(" SET ");
                sb.AppendFormat(" IS_POSTED_ERP ={0}", Enums.GetStringValue(SiNo.No));
                sb.AppendFormat(", POSTED_RESPONSE='{0}'", op.Mensaje);
                sb.Append(" , ATTEMPTED_WITH_ERROR = isnull(ATTEMPTED_WITH_ERROR,0) +1");
                sb.AppendFormat(", POSTED_ERP={0}", " GetDate() ");
                sb.Append(" WHERE ");
                sb.AppendFormat(" RECEPTION_HEADER ={0}", retorno.DocEntry);

                _baseDeDatosServicio.ExecuteNonQuery(sb.ToString());

                _baseDeDatosServicio.Commit();
            }
            catch (Exception)
            {
                _baseDeDatosServicio.Rollback();
                throw;
            }
        }


        public void MarcarRetornoComoEnviadoAErp(ReturnHeader retorno)
        {
            _baseDeDatosServicio.BeginTransaction();
            try
            {
                var sb = new StringBuilder();
                sb.Append(" Update SWIFT_RECEPTION_HEADER");
                sb.Append(" SET ");
                sb.AppendFormat(" IS_POSTED_ERP ={0}", Enums.GetStringValue(SiNo.Si));
                sb.AppendFormat(", POSTED_RESPONSE='{0}'", retorno.Comments);
                sb.AppendFormat(", POSTED_ERP={0}", " GetDate() ");
                sb.Append(" WHERE ");
                sb.AppendFormat(" RECEPTION_HEADER ={0}", retorno.DocEntry);

                _baseDeDatosServicio.ExecuteNonQuery(sb.ToString());
                _baseDeDatosServicio.Commit();
            }
            catch (Exception)
            {
                _baseDeDatosServicio.Rollback();
                throw;
            }
        }
    }
}