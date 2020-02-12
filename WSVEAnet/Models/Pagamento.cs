using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSVEAnet.Models
{
    public class Pagamento
    {
        public string documento { get; set; }
        public string tipo_documento { get; set; }
        public string nome { get; set; }
        public string email { get; set; }
        public string telefone { get; set; }
        public string celular { get; set; }
        public int online { get; set; }
        public string DtMesEnvio { get; set; }
        public string DtDiaEnvio { get; set; }
        public string DtMesVencimento { get; set; }
        public string DtDiaVencimento { get; set; }
        public string data_envio { get; set; }
        public string data_vencimento { get; set; }
        public string situacao_pagamento { get; set; }
        public string mes_referencia { get; set; }
        public string pagamento_url { get; set; }
        public string endereco { get; set; }
        public string bairro { get; set; }
        public string cidade { get; set; }
        public string uf { get; set; }
        public string cep { get; set; }
        public Int64 n_parcela { get; set; }
        public string banco { get; set; }
        public string agencia { get; set; }
        public string digito_agencia { get; set; }
        public string conta { get; set; }
        public string digito_conta { get; set; }
        public int carteira { get; set; }
        public double valor { get; set; }
        public double multa { get; set; }
        public double valor_total { get; set; }
        public string nosso_numero { get; set; }
        public string acao { get; set; }
        public string str_retorno { get; set; }
        public int codigoEnvio { get; set; }
        public string codigoEnvioTemp { get; set; }
        public string resultado { get; set; }

    }
}