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
        int contador = 0;
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

        public string SetHigienizacaoEmail(string nr_cpfCnpj, string ds_emailOrigem, string ds_emailCorrigido, string fl_aplicado)
        {
            int intGravaEnvio = 0;
            contador = contador + 1;
            try
            {
                using (SqlConnection conn = new SqlConnection(getString.ConnectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("[SGB].[Boleto].[pro_SetHigienizacaoEmail]", conn);

                    cmd.Parameters.AddWithValue("@nr_cpfCnpj", nr_cpfCnpj);
                    cmd.Parameters.AddWithValue("@ds_emailOrigem", ds_emailOrigem);
                    cmd.Parameters.AddWithValue("@ds_emailCorrigido", ds_emailCorrigido);

                    if (fl_aplicado.ToString().ToUpper() == "TRUE".ToUpper())
                    {
                        cmd.Parameters.AddWithValue("@fl_aplicado", 1);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@fl_aplicado", 0);
                    }

                    cmd.CommandTimeout = 160;
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    intGravaEnvio = cmd.ExecuteNonQuery();

                }
            }
            catch (Exception ex)
            {

                return ex.Message.ToString();
                //throw new global::System.Data.StrongTypingException("'Procure o Administrador'", ex);
            }

            return "Ok";
        }

        public string SetRelatorioEmail(int id_envio, string cd_parcela, string dt_envio, int nr_tentativas, string nr_doc, string ds_emailEnviado, string ds_emailCorrigido, string st_parcela, string ds_erro)
        {
            int intGravaEnvio = 0;
            contador = contador + 1;
            try
            {
                using (SqlConnection conn = new SqlConnection(getString.ConnectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("[SGB].[Boleto].[pro_setRelatorioGerencialEmail]", conn);

                    cmd.Parameters.AddWithValue("@id_envio", id_envio);
                    cmd.Parameters.AddWithValue("@cd_parcela", Convert.ToInt32(cd_parcela));
                    cmd.Parameters.AddWithValue("@dt_envio", dt_envio);
                    cmd.Parameters.AddWithValue("@nr_tentativas", nr_tentativas);
                    cmd.Parameters.AddWithValue("@nr_doc", nr_doc);
                    cmd.Parameters.AddWithValue("@ds_emailEnviado", ds_emailEnviado);
                    cmd.Parameters.AddWithValue("@ds_emailCorrigido", ds_emailCorrigido);
                    cmd.Parameters.AddWithValue("@st_parcela", st_parcela);
                    cmd.Parameters.AddWithValue("@ds_erro", ds_erro);

                    cmd.CommandTimeout = 160;
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    intGravaEnvio = cmd.ExecuteNonQuery();

                }
            }
            catch (Exception ex)
            {

                return ex.Message.ToString();
                //throw new global::System.Data.StrongTypingException("'Procure o Administrador'", ex);
            }

            return "Ok";
        }
    }
}