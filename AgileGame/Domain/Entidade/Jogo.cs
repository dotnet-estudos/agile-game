using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entidade
{
    public class Jogo
    {
        public int JogoId { get; set; }

        public int BoardId { get; set; }

        public string NomeTurma { get; set; }

        public int QuantidadeEquipes { get; set; }

        public DateTime DataCadastro { get; set; }

        public bool Finalizado { get; set; }

        public DateTime? DataHoraFim { get; set; }

        public Board Board { get; set; }
    }
}
