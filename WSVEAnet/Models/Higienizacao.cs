using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WSVEAnet.Models
{
    public class Higienizacao
    {
        public string id { get; set; }
        public string email_origem { get; set; }
        public string email_higienizado { get; set; }
        public string aplicada { get; set; }
        public string cliente_afetado { get; set; }
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