using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entidade
{
    public class Equipe
    {
        public int EquipeId { get; set; }
        public string Nome { get; set; }

        public string Hash { get; set; }
        
        public int JogoId { get; set; }

        public Jogo Jogo { get; set; }

    }
}
