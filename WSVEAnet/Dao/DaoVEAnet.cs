using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace WSVEAnet.Dao
{
    public class DaoVEAnet
    {

        ConnectionStringSettings getString = WebConfigurationManager.ConnectionStrings["getVEAnet"] as ConnectionStringSettings;

        public DataTable GetDadosParcela(int tp, int parcela)
        {
            DataTable dados_parcela = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(getString.ConnectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("[SGB].[Boleto].[pro_GetParcelaEmail]", conn);

                    cmd.CommandTimeout = 160;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@tp", tp);
                    cmd.Parameters.AddWithValue("@cd_parcela", parcela);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dados_parcela);

                    //intResposta = cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                throw new global::System.Data.StrongTypingException("'Procure o Administrador'", ex);
            }
            return dados_parcela;
        }

        public string SetParcelaEnvioEmail(int id_envio, Int64 cd_parcela, string ds_mensagemEnvio)
        {
            int intGravaEnvio;

            try
            {
                using (SqlConnection conn = new SqlConnection(getString.ConnectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("[SGB].[Boleto].[pro_SetParcelaEnvioEmail]", conn);

                    cmd.Parameters.AddWithValue("@id_envio", id_envio);
                    cmd.Parameters.AddWithValue("@cd_parcela", cd_parcela);
                    cmd.Parameters.AddWithValue("@ds_mensagemEnvio", ds_mensagemEnvio);

                    cmd.CommandTimeout = 160;
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    intGravaEnvio = cmd.ExecuteNonQuery();

                }
            }
            catch (SqlException ex)
            {
                return ex.Message.ToString();
                //throw new global::System.Data.StrongTypingException("'Procure o Administrador'", ex);
            }

            return "Ok";
        }


    }
}