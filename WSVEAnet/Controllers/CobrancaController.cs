using RestSharp;
using System;
using System.Data;
using System.Web.Http;
using System.Xml;
using WSVEAnet.Dao;
using WSVEAnet.Models;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace WSVEAnet.Controllers
{
    public class CobrancaController : ApiController
    {
        [AcceptVerbs("GET")]
        [Route("Relatorio")]
        public string Relatorio()
        {
            string retorno;

            string data_inicio;
            string data_fim;
            int nr_itens;
            int page;
            string tp_acao;

            string mes;

            Models.Relatorio relatorio = new Relatorio();
            DaoVEAnet daoRelatorio = new DaoVEAnet();

            try
            {
                if (DateTime.Now.Month < 10)
                {
                    mes = "0" + DateTime.Now.Month.ToString();
                }
                else
                {
                    mes = DateTime.Now.Month.ToString();
                }

                DateTime dtAtual = DateTime.Today;
                DateTime dtUltimoDiaMes = new DateTime(dtAtual.Year, dtAtual.Month, DateTime.DaysInMonth(dtAtual.Year, dtAtual.Month));
                DateTime now = DateTime.Now;

                data_inicio = now.ToString("yyyy") + "-" + "06" + "-" + "01";
                data_fim = now.ToString("yyyy") + "-" + mes + "-" + dtUltimoDiaMes.Day.ToString();
                nr_itens = 3000;
                page = 1;
                tp_acao = "CADASTRAR";


                var client = new RestClient("https://boleto.carsystem.com/api/v1/relatorio?data_inicio=" + data_inicio.ToString() + "&data_fim=" + data_fim.ToString() + "&itens=+ " + nr_itens + "&page=1&acao=ATUALIZAR");
                client.Timeout = -1;

                var request = new RestRequest(Method.GET);
                //EnvioParam EnvioParametros = new EnvioParam();

                //EnvioParametros.data_inicio = data_inicio.ToString();
                //EnvioParametros.data_fim = data_fim.ToString();
                //EnvioParametros.itens = 100;
                //EnvioParametros.page = 1;
                //EnvioParametros.acao = tp_acao.ToString();

                //var Json = JsonConvert.SerializeObject(EnvioParametros, Newtonsoft.Json.Formatting.Indented, new Newtonsoft.Json.JsonSerializerSettings { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore });

                var iParametros =
                     "{" +
                     "\"data_inicio\":\"" + data_inicio + "\"," +
                      "\"data_fim\":\"" + data_fim + "\"," +
                       "\"itens\":" + nr_itens + "," +
                       "\"page\":" + page + "," +
                       "\"acao\":\"" + tp_acao.ToString() + "\"" +
                     "}";

                request.AddHeader("Authorization", "Basic Y2Fyc3lzdGVtOkBhcGlib2xldG8yMDIw");
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Cookie", "XSRF-TOKEN=eyJpdiI6IjdwK2JNRjhlU0Y0QkRhN3dFaEdWZmc9PSIsInZhbHVlIjoiNEtXQ1pDa1pUSGd1a1wvYzJhazQ3cUF2cnpIR29NVldNY3ZDNUZ4QVV6cUl3ejVVaU04dU53aURma1Bsd2hZbjEiLCJtYWMiOiJhZmZhODFhNGVlNmI2YjI3OTYzNjY1MDI0N2M2OWMzZGQ0ZWUxM2Y4OGQwOTQ3OGIxNzc4NDcxYmU5NGM3M2Q1In0%3D; laravel_session=eyJpdiI6IktmQm55c1dXK3J2NUo1cElPMkVCS0E9PSIsInZhbHVlIjoiSFdxNDZSSXAzUkg5K1NDY3hxKzhNcVp3MW1NYUg2MngxaWpnNjQyR1prTzJGNHFvVjdBUjh5d3FwZmhkNFJvdSIsIm1hYyI6IjE5OGYwNDNkNDJhZjJkMWYwNzdhZjRjMDI5ZmRjYzNmYTFhYmQ3NzRiOTUwNjk2Y2ZmYTE5NTBmMzUyZjUyYmQifQ%3D%3D");

                request.AddParameter("application/json", iParametros
                    //"{\"data_inicio\":\"" + data_inicio + "\",\"data_fim\":\"" + data_fim + "\",\"itens\":3000,\"page\":1,\"acao\": \"ATUALIZAR\"}"
                    , ParameterType.RequestBody);

                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);

                JavaScriptSerializer js = new JavaScriptSerializer();
                var customerRelatorio = JsonConvert.DeserializeObject<RootobjectRelatorio>(response.Content);

                if (customerRelatorio.success == true)
                {
                    foreach (var item in customerRelatorio.relatorio)
                    {
                        int id_envio = item.id;
                        string cd_parcela = item.numero_parcela;
                        string dt_envio = item.data_envio;
                        int nr_tentativas = item.tentativas;
                        string nr_doc = item.documento;
                        string ds_emailEnviado = item.email_enviado;
                        string ds_emailCorrigido = item.email_corrigido;
                        string st_parcela = item.situacao_envio;
                        string ds_erro = item.erro_envio;

                        daoRelatorio.SetRelatorioEmail(id_envio, cd_parcela, dt_envio, nr_tentativas, nr_doc, ds_emailEnviado, ds_emailCorrigido, st_parcela, ds_erro);
                    }

                    retorno = customerRelatorio.success.ToString();
                }
                else
                {
                    retorno = response.Content.ToString();
                }
            }
            catch (Exception ex)
            {
                throw new global::System.Data.StrongTypingException("'Procure o Administrador'", ex);
            }

            return retorno.ToString();
        }


        [AcceptVerbs("GET")]
        [Route("Higienizacao")]
        public string Higienizacao()
        {
            string url;
            string mes;

            Models.Higienizacao higienizacao = new Higienizacao();
            DaoVEAnet daoParcelas = new DaoVEAnet();

            try
            {
                if (DateTime.Now.Month < 10)
                {
                    mes = "0" + DateTime.Now.Month.ToString();
                }
                else
                {
                    mes = DateTime.Now.Month.ToString();
                }

                url = "https://boleto.carsystem.com/api/v1/higienizacao?mes_referencia=" + DateTime.Now.Year + mes.ToString();

                var client = new RestClient(url);
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);

                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", "Basic Y2Fyc3lzdGVtOkBhcGlib2xldG8yMDIw");

                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);

                JavaScriptSerializer js = new JavaScriptSerializer();
                var customerRetorno = JsonConvert.DeserializeObject<Retorno>(response.Content);

                if (customerRetorno.success == true)
                {
                    foreach (var item in customerRetorno.higienizacoes)
                    {

                        if (item.cliente_afetado != null)
                        {
                            string email_origem = item.email_origem.ToString();
                            string email_higienizado = item.email_higienizado.ToString();
                            string documento = item.cliente_afetado.documento.ToString();
                            string aplicada = item.aplicada.ToString();
                            if (documento != null && email_origem != null && email_higienizado != null && aplicada != null)
                            {
                                daoParcelas.SetHigienizacaoEmail(documento, email_origem, email_higienizado, aplicada);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new global::System.Data.StrongTypingException("'Procure o Administrador'", ex);
            }

            return "Ok";  //cobranca.str_retorno;
        }

        [AcceptVerbs("POST")]
        [Route("Cobranca")]
        public string Cobranca()
        {
            String url;

            Models.CobrancaEnvio cobranca = new CobrancaEnvio();

            DataTable dtParcelas = new DataTable();
            DataTable dtDadosParcelas = new DataTable();

            DaoVEAnet daoParcelas = new DaoVEAnet();

            try
            {
                // carrega parcelas
                dtParcelas = daoParcelas.GetDadosParcela(1, 0);

                foreach (DataRow rowParcelas in dtParcelas.Rows)
                {
                    // carrega dados parcelas
                    if (rowParcelas[3].ToString().Substring(0, 3) == "P99")
                    {
                        dtDadosParcelas = daoParcelas.GetDadosParcela(2, Convert.ToInt32(rowParcelas[0].ToString()));
                    }
                    else
                    {
                        dtDadosParcelas = daoParcelas.GetDadosParcela(3, Convert.ToInt32(rowParcelas[0].ToString()));
                    }

                    foreach (DataRow rowDadosParcelas in dtDadosParcelas.Rows)
                    {
                        if (rowParcelas[2].ToString() == "")
                        {
                            cobranca.documento = rowDadosParcelas[0].ToString();

                            if (rowDadosParcelas[0].ToString().Length > 14)
                            {
                                cobranca.tipo_documento = "CNPJ";
                            }
                            else
                            {
                                cobranca.tipo_documento = "CPF";
                            }

                            cobranca.nome = rowDadosParcelas[1].ToString();
                            cobranca.email = rowDadosParcelas[2].ToString();
                            cobranca.telefone = rowDadosParcelas[3].ToString();
                            cobranca.celular = rowDadosParcelas[4].ToString();
                            cobranca.online = Convert.ToInt32(rowDadosParcelas[5].ToString());

                            DateTime DtDiaMesAnoEnvio = Convert.ToDateTime(rowDadosParcelas[6].ToString());
                            if (Convert.ToInt32(DtDiaMesAnoEnvio.Month.ToString()) < 10)
                            {
                                cobranca.DtMesEnvio = "0" + DtDiaMesAnoEnvio.Month.ToString();
                            }
                            else
                            {
                                cobranca.DtMesEnvio = DtDiaMesAnoEnvio.Month.ToString();
                            }

                            if (Convert.ToInt32(DtDiaMesAnoEnvio.Day.ToString()) < 10)
                            {
                                cobranca.DtDiaEnvio = "0" + DtDiaMesAnoEnvio.Day.ToString();
                            }
                            else
                            {
                                cobranca.DtDiaEnvio = DtDiaMesAnoEnvio.Day.ToString();
                            }
                            cobranca.data_envio = DtDiaMesAnoEnvio.Year + "-" + cobranca.DtMesEnvio.ToString() + "-" + cobranca.DtDiaEnvio.ToString();

                            DateTime DtDiaMesAnoVencimento = Convert.ToDateTime(rowDadosParcelas[7].ToString());
                            if (Convert.ToInt32(DtDiaMesAnoVencimento.Month.ToString()) < 10)
                            {
                                cobranca.DtMesVencimento = "0" + DtDiaMesAnoVencimento.Month.ToString();
                            }
                            else
                            {
                                cobranca.DtMesVencimento = DtDiaMesAnoVencimento.Month.ToString();
                            }

                            if (Convert.ToInt32(DtDiaMesAnoVencimento.Day.ToString()) < 10)
                            {
                                cobranca.DtDiaVencimento = "0" + DtDiaMesAnoVencimento.Day.ToString();
                            }
                            else
                            {
                                cobranca.DtDiaVencimento = DtDiaMesAnoVencimento.Day.ToString();
                            }

                            cobranca.data_vencimento = DtDiaMesAnoVencimento.Year + "-" + cobranca.DtMesVencimento.ToString() + "-" + cobranca.DtDiaVencimento.ToString();

                            cobranca.situacao_pagamento = rowDadosParcelas[8].ToString();
                            cobranca.mes_referencia = rowDadosParcelas[9].ToString().Substring(0, 4) + "/" + rowDadosParcelas[9].ToString().Substring(4, 2);
                            cobranca.pagamento_url = rowDadosParcelas[10].ToString();
                            cobranca.endereco = rowDadosParcelas[11].ToString();
                            cobranca.bairro = rowDadosParcelas[12].ToString();
                            cobranca.cidade = rowDadosParcelas[13].ToString();
                            cobranca.uf = rowDadosParcelas[14].ToString();
                            cobranca.cep = rowDadosParcelas[15].ToString();
                            cobranca.n_parcela = Convert.ToInt64(rowDadosParcelas[16].ToString());
                            if (cobranca.n_parcela == 22238780)
                            { }
                            cobranca.banco = rowDadosParcelas[17].ToString();
                            cobranca.agencia = rowDadosParcelas[18].ToString();
                            cobranca.digito_agencia = rowDadosParcelas[19].ToString();
                            cobranca.conta = rowDadosParcelas[20].ToString();
                            cobranca.digito_conta = rowDadosParcelas[21].ToString();
                            cobranca.carteira = Convert.ToInt32(rowDadosParcelas[22].ToString());
                            cobranca.valor = rowDadosParcelas[23].ToString();
                            cobranca.multa = rowDadosParcelas[24].ToString();
                            cobranca.valor_total = rowDadosParcelas[25].ToString();
                            cobranca.nosso_numero = rowDadosParcelas[26].ToString();

                            if (rowDadosParcelas[28].ToString() != "")
                            {
                                cobranca.acao = "ATUALIZAR";
                                cobranca.codigoEnvio = Convert.ToInt32(rowParcelas[4].ToString());
                                cobranca.codigoEnvioAtual = cobranca.codigoEnvio;

                                url = "https://boleto.carsystem.com/api/v1/cobranca/" + cobranca.codigoEnvio + "/Pagamento";
                            }
                            else
                            {
                                cobranca.acao = "CADASTRAR";

                                url = "https://boleto.carsystem.com/api/v1/cobranca";
                            }

                            var client = new RestClient(url);
                            client.Timeout = -1;
                            var request = new RestRequest(Method.POST);

                            request.AddHeader("Content-Type", "application/json");
                            request.AddHeader("Authorization", "Basic Y2Fyc3lzdGVtOkBhcGlib2xldG8yMDIw");

                            request.AddParameter("application/json", "{\"documento\":\"" + cobranca.documento + "\",\"tipo_documento\":\"" + cobranca.tipo_documento + "\",\"nome\":\"" + cobranca.nome + "\",\"email\":\"" + cobranca.email + "\",\"telefone\":\"" + cobranca.telefone + "\",\"celular\":\"" + cobranca.celular + "\",\"online\":" + cobranca.online + ",\"data_envio\":\"" + cobranca.data_envio + "\",\"data_vencimento\":\"" + cobranca.data_vencimento + "\",\"situacao_pagamento\":\"" + cobranca.situacao_pagamento + "\",\"mes_referencia\":\"" + cobranca.mes_referencia + "\",\"pagamento_url\":0,\"endereco\":\"" + cobranca.endereco + "\",\"bairro\":\"" + cobranca.bairro + "\",\"cidade\":\"" + cobranca.cidade + "\",\"uf\":\"" + cobranca.uf + "\",\"cep\":\"" + cobranca.cep + "\",\"n_parcela\":\"" + cobranca.n_parcela + "\",\"banco\":\"" + cobranca.banco + "\",\"agencia\":\"" + cobranca.agencia + "\",\"digito_agencia\":\"" + cobranca.digito_agencia + "\",\"conta\":\"" + cobranca.conta + "\",\"digito_conta\":\"" + cobranca.digito_conta + "\",\"carteira\":" + cobranca.carteira + ",\"valor\":" + cobranca.valor.ToString().Replace(",", ".") + ",\"multa\":" + cobranca.multa.ToString().Replace(",", ".") + ",\"valor_total\":" + cobranca.valor_total.ToString().Replace(",", ".") + ",\"nosso_numero\":\"" + cobranca.nosso_numero + "\",\"acao\":\"" + cobranca.acao + "\"}", ParameterType.RequestBody);

                            IRestResponse response = client.Execute(request);
                            Console.WriteLine(response.Content);

                            JavaScriptSerializer js = new JavaScriptSerializer();
                            var customerRetorno = JsonConvert.DeserializeObject<Rootobject>(response.Content);

                            if (customerRetorno.success == true)
                            {
                                cobranca.codigo = customerRetorno.cobranca.id.ToString();
                                cobranca.codigoEnvio = Convert.ToInt32(cobranca.codigo.ToString());

                                if (rowDadosParcelas[28].ToString() != "")
                                {
                                    cobranca.str_retorno = response.Content.ToString();
                                    daoParcelas.SetParcelaEnvioEmail(cobranca.codigoEnvioAtual, Convert.ToInt64(cobranca.n_parcela), cobranca.str_retorno.ToString());
                                }
                                else
                                {
                                    cobranca.str_retorno = response.Content.ToString();
                                    daoParcelas.SetParcelaEnvioEmail(Convert.ToInt32(cobranca.codigoEnvio), Convert.ToInt64(cobranca.n_parcela), cobranca.str_retorno.ToString());
                                }
                            }
                            else
                            {
                                cobranca.str_retorno = response.Content.ToString();
                                daoParcelas.SetParcelaEnvioEmail(0, Convert.ToInt64(cobranca.n_parcela), cobranca.str_retorno.ToString());
                            }

                            //string strJson = js.Serialize(response.Content);

                            //string[] spl = strJson.Split(new char[] { });

                            //int linha = spl.Length;

                            //cobranca.resultado = spl[0].ToString().Substring(14, 4);

                            //if (cobranca.resultado == "true")
                            //{
                            //    cobranca.codigo = spl[linha - 1].ToString().Substring(18, 7).Replace("}", "").Replace("\"", "");
                            //    cobranca.codigoEnvio = Convert.ToInt32(cobranca.codigo.ToString());

                            //    if (rowDadosParcelas[28].ToString() != "")
                            //    {
                            //        cobranca.str_retorno = response.Content.ToString();
                            //        daoParcelas.SetParcelaEnvioEmail(cobranca.codigoEnvioAtual, Convert.ToInt64(cobranca.n_parcela), cobranca.str_retorno.ToString());
                            //    }
                            //    else
                            //    {
                            //        cobranca.str_retorno = response.Content.ToString();
                            //        daoParcelas.SetParcelaEnvioEmail(Convert.ToInt32(cobranca.codigoEnvio), Convert.ToInt64(cobranca.n_parcela), cobranca.str_retorno.ToString());
                            //    }
                            //}
                            //else
                            //{
                            //    cobranca.str_retorno = response.Content.ToString();
                            //    daoParcelas.SetParcelaEnvioEmail(0, Convert.ToInt64(cobranca.n_parcela), cobranca.str_retorno.ToString());
                            //}
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw new global::System.Data.StrongTypingException("'Procure o Administrador'", ex);
            }

            return "Envio de parcela(s) efetuado com sucesso!!!";  //cobranca.str_retorno;
        }

    }
}
