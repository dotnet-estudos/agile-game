using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entidade
{
   public  class LogGame
    {
        public int LogGameId { get; set; }
        public string Tabela { get; set; }
        public string Acao { get; set; }
        public string NomeUsuario { get; set; }
        public string Chaves { get; set; }
        public string NomeColuna { get; set; }
        public string Propriedade { get; set; }
        public string ValorOriginal { get; set; }
        public string ValorAtual { get; set; }
        public DateTime DataOcorrencia { get; set; }
    }
}
