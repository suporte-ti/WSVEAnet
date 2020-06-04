using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSVEAnet.Models
{


    public class EnvioParam
    {
        public string data_inicio { get; set; }
        public string data_fim { get; set; }
        public int itens { get; set; }
        public int page { get; set; }
        public string acao { get; set; }
    }


}