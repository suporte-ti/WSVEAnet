using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSVEAnet.Models
{
    public class Rootobject
    {
        public bool success { get; set; }
        public string message { get; set; }
        public Cobranca cobranca { get; set; }
    }

    public class Cobranca
    {
        public string acao { get; set; }
        public string n_parcela { get; set; }
        public string documento { get; set; }
        public string tipo_documento { get; set; }
        public string nome { get; set; }
        public string email { get; set; }
        public string telefone { get; set; }
        public string celular { get; set; }
        public string data_envio { get; set; }
        public string data_vencimento { get; set; }
        public string situacao_pagamento { get; set; }
        public string mes_referencia { get; set; }
        public string endereco { get; set; }
        public string bairro { get; set; }
        public string cidade { get; set; }
        public string uf { get; set; }
        public string cep { get; set; }
        public string banco { get; set; }
        public string agencia { get; set; }
        public string digito_agencia { get; set; }
        public string conta { get; set; }
        public string digito_conta { get; set; }
        public int carteira { get; set; }
        public string valor { get; set; }
        public string multa { get; set; }
        public string valor_total { get; set; }
        public string nosso_numero { get; set; }
        public string updated_at { get; set; }
        public string created_at { get; set; }
        public int id { get; set; }
    }
}