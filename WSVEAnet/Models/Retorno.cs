using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSVEAnet.Models
{


    public class Retorno
    {
        public bool success { get; set; }
        public int total { get; set; }
        public Higienizaco[] higienizacoes { get; set; }
    }

    public class Higienizaco
    {
        public int id { get; set; }
        public string email_origem { get; set; }
        public string email_higienizado { get; set; }
        public bool aplicada { get; set; }
        public Cliente_Afetado cliente_afetado { get; set; }
    }

    public class Cliente_Afetado
    {
        public int id { get; set; }
        public string nome { get; set; }
        public string email { get; set; }
        public string documento { get; set; }
        public string tipo_documento { get; set; }
        public string telefone { get; set; }
        public string celular { get; set; }
        public string endereco { get; set; }
        public string numero_endereco { get; set; }
        public string bairro { get; set; }
        public string cidade { get; set; }
        public string uf { get; set; }
        public string cep { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
    }

}