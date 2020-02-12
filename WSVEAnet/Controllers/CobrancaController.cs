using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
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

        [AcceptVerbs("POST")]
        [Route("Cobranca")]
        public string Cobranca()
        {
            string str_retorno;
            str_retorno = buscaParcelas();

            return str_retorno;
        }

        [AcceptVerbs("POST")]
        [Route("Pagamento")]
        public string Pagamento()
        {
            string str_retorno;
            str_retorno = atualizaParcelas();

            return str_retorno;
        }

        private string atualizaParcelas()
        {
            Models.Pagamento pagamento = new Pagamento();

            DataTable dtParcelas = new DataTable();
            DataTable dtAtualizaDadosParcelas = new DataTable();

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
                        dtAtualizaDadosParcelas = daoParcelas.GetDadosParcela(2, Convert.ToInt32(rowParcelas[0].ToString()));
                    }
                    else
                    {
                        dtAtualizaDadosParcelas = daoParcelas.GetDadosParcela(3, Convert.ToInt32(rowParcelas[0].ToString()));
                    }

                    foreach (DataRow rowDadosParcelas in dtAtualizaDadosParcelas.Rows)
                    {
                        pagamento.documento = rowDadosParcelas[0].ToString();
                        pagamento.tipo_documento = "CPF";
                        pagamento.nome = rowDadosParcelas[1].ToString();
                        pagamento.email = rowDadosParcelas[2].ToString();
                        pagamento.telefone = rowDadosParcelas[3].ToString();
                        pagamento.celular = rowDadosParcelas[4].ToString();
                        pagamento.online = Convert.ToInt32(rowDadosParcelas[5].ToString());

                        DateTime DtDiaMesAnoEnvio = Convert.ToDateTime(rowDadosParcelas[6].ToString());
                        if (Convert.ToInt32(DtDiaMesAnoEnvio.Month.ToString()) < 10)
                        {
                            pagamento.DtMesEnvio = "0" + DtDiaMesAnoEnvio.Month.ToString();
                        }
                        else
                        {
                            pagamento.DtMesEnvio = DtDiaMesAnoEnvio.Month.ToString();
                        }

                        if (Convert.ToInt32(DtDiaMesAnoEnvio.Day.ToString()) < 10)
                        {
                            pagamento.DtDiaEnvio = "0" + DtDiaMesAnoEnvio.Day.ToString();
                        }
                        else
                        {
                            pagamento.DtDiaEnvio = DtDiaMesAnoEnvio.Day.ToString();
                        }
                        pagamento.data_envio = DtDiaMesAnoEnvio.Year + "-" + pagamento.DtMesEnvio.ToString() + "-" + pagamento.DtDiaEnvio.ToString();


                        DateTime DtDiaMesAnoVencimento = Convert.ToDateTime(rowDadosParcelas[7].ToString());
                        if (Convert.ToInt32(DtDiaMesAnoVencimento.Month.ToString()) < 10)
                        {
                            pagamento.DtMesVencimento = "0" + DtDiaMesAnoVencimento.Month.ToString();
                        }
                        else
                        {
                            pagamento.DtMesVencimento = DtDiaMesAnoVencimento.Month.ToString();
                        }

                        if (Convert.ToInt32(DtDiaMesAnoVencimento.Day.ToString()) < 10)
                        {
                            pagamento.DtDiaVencimento = "0" + DtDiaMesAnoVencimento.Day.ToString();
                        }
                        else
                        {
                            pagamento.DtDiaVencimento = DtDiaMesAnoVencimento.Day.ToString();
                        }
                        pagamento.data_vencimento = DtDiaMesAnoVencimento.Year + "-" + pagamento.DtMesVencimento.ToString() + "-" + pagamento.DtDiaVencimento.ToString();

                        pagamento.situacao_pagamento = rowDadosParcelas[8].ToString();
                        pagamento.mes_referencia = rowDadosParcelas[9].ToString().Substring(0, 4) + "/" + rowDadosParcelas[9].ToString().Substring(4, 2);
                        pagamento.pagamento_url = rowDadosParcelas[10].ToString();
                        pagamento.endereco = rowDadosParcelas[11].ToString();
                        pagamento.bairro = rowDadosParcelas[12].ToString();
                        pagamento.cidade = rowDadosParcelas[13].ToString();
                        pagamento.uf = rowDadosParcelas[14].ToString();
                        pagamento.cep = rowDadosParcelas[15].ToString();
                        pagamento.n_parcela = Convert.ToInt64(rowDadosParcelas[16].ToString());
                        pagamento.banco = rowDadosParcelas[17].ToString();
                        pagamento.agencia = rowDadosParcelas[18].ToString();
                        pagamento.digito_agencia = rowDadosParcelas[19].ToString();
                        pagamento.conta = rowDadosParcelas[20].ToString();
                        pagamento.digito_conta = rowDadosParcelas[21].ToString();
                        pagamento.carteira = Convert.ToInt32(rowDadosParcelas[22].ToString());
                        pagamento.valor = Convert.ToDouble(rowDadosParcelas[23].ToString());
                        pagamento.multa = Convert.ToDouble(rowDadosParcelas[24].ToString());
                        pagamento.valor_total = Convert.ToDouble(rowDadosParcelas[25].ToString());
                        pagamento.nosso_numero = rowDadosParcelas[26].ToString();
                        pagamento.acao = rowDadosParcelas[27].ToString();

                        pagamento.codigoEnvio = Convert.ToInt32(rowParcelas[4].ToString());

                        var client = new RestClient(" https://boleto.carsystem.com/api/v1/cobranca/" + pagamento.codigoEnvio + "/pagamento");
                        client.Timeout = -1;
                        var request = new RestRequest(Method.POST);

                        request.AddHeader("Content-Type", "application/json");
                        request.AddHeader("Authorization", "Basic Y2Fyc3lzdGVtOkBhcGlib2xldG8yMDIw");

                        request.AddParameter("application/json", "{\"documento\":\"" + pagamento.documento + "\",\"tipo_documento\":\"" + pagamento.tipo_documento + "\",\"nome\":\"" + pagamento.nome + "\",\"email\":\"" + pagamento.email + "\",\"telefone\":\"" + pagamento.telefone + "\",\"celular\":\"" + pagamento.celular + "\",\"online\":" + pagamento.online + ",\"data_envio\":\"" + pagamento.data_envio + "\",\"data_vencimento\":\"" + pagamento.data_vencimento + "\",\"situacao_pagamento\":\"" + pagamento.situacao_pagamento + "\",\"mes_referencia\":\"" + pagamento.mes_referencia + "\",\"pagamento_url\":0,\"endereco\":\"" + pagamento.endereco + "\",\"bairro\":\"" + pagamento.bairro + "\",\"cidade\":\"" + pagamento.cidade + "\",\"uf\":\"" + pagamento.uf + "\",\"cep\":\"" + pagamento.cep + "\",\"n_parcela\":\"" + pagamento.n_parcela + "\",\"banco\":\"" + pagamento.banco + "\",\"agencia\":\"" + pagamento.agencia + "\",\"digito_agencia\":\"" + pagamento.digito_agencia + "\",\"conta\":\"" + pagamento.conta + "\",\"digito_conta\":\"" + pagamento.digito_conta + "\",\"carteira\":" + pagamento.carteira + ",\"valor\":" + pagamento.valor.ToString().Replace(",", ".") + ",\"multa\":" + pagamento.multa.ToString().Replace(",", ".") + ",\"valor_total\":" + pagamento.valor_total.ToString().Replace(",", ".") + ",\"nosso_numero\":\"" + pagamento.nosso_numero + "\",\"acao\":\"" + pagamento.acao + "\"}", ParameterType.RequestBody);

                        IRestResponse response = client.Execute(request);
                        Console.WriteLine(response.Content);

                        JavaScriptSerializer js = new JavaScriptSerializer();
                        string strJson = js.Serialize(response.Content);


                        string[] spl = strJson.Split(new char[] { });

                        pagamento.resultado = spl[0].ToString().Substring(14, 4);

                        if (pagamento.resultado == "true")
                        {
                            pagamento.str_retorno = response.Content.ToString();
                            daoParcelas.SetParcelaEnvioEmail(Convert.ToInt32(pagamento.codigoEnvio), Convert.ToInt64(pagamento.n_parcela), pagamento.str_retorno.ToString());
                        }
                        
                    }
                }
            }
            catch (Exception ex)
            {
                throw new global::System.Data.StrongTypingException("'Procure o Administrador'", ex);
            }

            return "Pagamento de parcela(s) efetuado com sucesso!!!";  //pagamento.str_retorno;
        }

        public string buscaParcelas()
        {
            Models.Cobranca cobranca = new Cobranca();

            DataTable dtParcelas = new DataTable();
            DataTable dtDadosParcelas = new DataTable();

            DaoVEAnet daoParcelas = new DaoVEAnet();

            try
            {
                // carrega parcelas
                dtParcelas = daoParcelas.GetDadosParcela(1,0);

                foreach (DataRow rowParcelas in dtParcelas.Rows)
                {
                    // carrega dados parcelas
                    if (rowParcelas[3].ToString().Substring(0,3) == "P99")
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
                            cobranca.banco = rowDadosParcelas[17].ToString();
                            cobranca.agencia = rowDadosParcelas[18].ToString();
                            cobranca.digito_agencia = rowDadosParcelas[19].ToString();
                            cobranca.conta = rowDadosParcelas[20].ToString();
                            cobranca.digito_conta = rowDadosParcelas[21].ToString();
                            cobranca.carteira = Convert.ToInt32(rowDadosParcelas[22].ToString());
                            cobranca.valor = Convert.ToDouble(rowDadosParcelas[23].ToString());
                            cobranca.multa = Convert.ToDouble(rowDadosParcelas[24].ToString());
                            cobranca.valor_total = Convert.ToDouble(rowDadosParcelas[25].ToString());
                            cobranca.nosso_numero = rowDadosParcelas[26].ToString();
                            cobranca.acao = rowDadosParcelas[27].ToString();

                            var client = new RestClient("https://boleto.carsystem.com/api/v1/cobranca");
                            client.Timeout = -1;
                            var request = new RestRequest(Method.POST);

                            request.AddHeader("Content-Type", "application/json");
                            request.AddHeader("Authorization", "Basic Y2Fyc3lzdGVtOkBhcGlib2xldG8yMDIw");

                            request.AddParameter("application/json", "{\"documento\":\"" + cobranca.documento + "\",\"tipo_documento\":\"" + cobranca.tipo_documento + "\",\"nome\":\"" + cobranca.nome + "\",\"email\":\"" + cobranca.email + "\",\"telefone\":\"" + cobranca.telefone + "\",\"celular\":\"" + cobranca.celular + "\",\"online\":" + cobranca.online + ",\"data_envio\":\"" + cobranca.data_envio + "\",\"data_vencimento\":\"" + cobranca.data_vencimento + "\",\"situacao_pagamento\":\"" + cobranca.situacao_pagamento + "\",\"mes_referencia\":\"" + cobranca.mes_referencia + "\",\"pagamento_url\":0,\"endereco\":\"" + cobranca.endereco + "\",\"bairro\":\"" + cobranca.bairro + "\",\"cidade\":\"" + cobranca.cidade + "\",\"uf\":\"" + cobranca.uf + "\",\"cep\":\"" + cobranca.cep + "\",\"n_parcela\":\"" + cobranca.n_parcela + "\",\"banco\":\"" + cobranca.banco + "\",\"agencia\":\"" + cobranca.agencia + "\",\"digito_agencia\":\"" + cobranca.digito_agencia + "\",\"conta\":\"" + cobranca.conta + "\",\"digito_conta\":\"" + cobranca.digito_conta + "\",\"carteira\":" + cobranca.carteira + ",\"valor\":" + cobranca.valor.ToString().Replace(",", ".") + ",\"multa\":" + cobranca.multa.ToString().Replace(",", ".") + ",\"valor_total\":" + cobranca.valor_total.ToString().Replace(",", ".") + ",\"nosso_numero\":\"" + cobranca.nosso_numero + "\",\"acao\":\"" + cobranca.acao + "\"}", ParameterType.RequestBody);

                            IRestResponse response = client.Execute(request);
                            Console.WriteLine(response.Content);

                            JavaScriptSerializer js = new JavaScriptSerializer();
                            string strJson = js.Serialize(response.Content);


                            string[] spl = strJson.Split(new char[] { });

                            int linha = spl.Length;

                            cobranca.resultado = spl[0].ToString().Substring(14, 4);

                            if (cobranca.resultado == "true")
                            {
                                cobranca.codigo = spl[linha - 1].ToString().Substring(18, 4).Replace("}", "").Replace("\"","");

                                cobranca.codigoEnvio = Convert.ToInt32(cobranca.codigo.ToString());

                                //cobranca.codigoEnvio = Convert.ToInt32(spl[linha - 1].ToString().Substring(18, 4));
                                //cobranca.codigoEnvio = Convert.ToInt32(strJson.Substring(888, 4));
                            }
                            else
                            {
                                cobranca.codigoEnvio = 0;
                            }

                            cobranca.str_retorno = response.Content.ToString();
                            daoParcelas.SetParcelaEnvioEmail(Convert.ToInt32(cobranca.codigoEnvio), Convert.ToInt64(cobranca.n_parcela), cobranca.str_retorno.ToString());
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw new global::System.Data.StrongTypingException("'Procure o Administrador'", ex);
            }

            //string atualizaPagamentos;
            //atualizaPagamentos = atualizaParcelas();

            return "Envio de parcela(s) efetuado com sucesso!!!";  //cobranca.str_retorno;
        }

        //// GET api/<controller>
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/<controller>/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<controller>
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/<controller>/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/<controller>/5
        //public void Delete(int id)
        //{
        //}
    }
}