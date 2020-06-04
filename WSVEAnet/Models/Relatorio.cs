using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSVEAnet.Models
{
    public class RootobjectRelatorio
    {
        public bool success { get; set; }
        public Relatorio[] relatorio { get; set; }
    }

    public class Relatorio
    {
        public int id { get; set; }
        public string numero_parcela { get; set; }
        public string data_envio { get; set; }
        public int tentativas { get; set; }
        public string documento { get; set; }
        public string email_enviado { get; set; }
        public string email_corrigido { get; set; }
        public string situacao_envio { get; set; }
        public string erro_envio { get; set; }
    }

}