using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entidade
{
    public class Kaizen
    {
        public int KaizenId { get; set; }
       
        public string Letra { get; set; }
        public string Descricao { get; set; }
        public int Esforco { get; set; }
        public int Repeticao { get; set; }
        public string Requisito { get; set; }
        public bool Ativo { get; set; }
    }
}
