using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entidade
{
    public class LancamentoBoard
    {
        public int LancamentoBoardId { get; set; }
        public int JogoId { get; set; }
        public int EquipeId { get; set; }
        public int DiaLancamento { get; set; }

        public int FaseId { get; set; }

        public int Quantidade { get; set; }


        public Fase Fase { get; set; }
        public Jogo Jogo { get; set; }
        public Equipe Equipe { get; set; }

       
    }
}
