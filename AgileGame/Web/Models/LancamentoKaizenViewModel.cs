using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class LancamentoKaizenViewModel
    {

        public int LancamentoKaizenId { get; set; }
        public int KaizenId { get; set; }
        public string KaizenLetra { get; set; }
        public int JogoId { get; set; }
        public int EquipeId { get; set; }
        public int DiaConlcusao { get; set; }
        public int DiaInicio { get; set; }
    }
}