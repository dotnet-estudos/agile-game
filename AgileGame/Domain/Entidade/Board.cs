using System.Collections;
using System.Collections.Generic;

namespace Domain.Entidade
{
    public class Board
    {
        public int BoardId { get; set; }
        
        public string Nome { get; set; }

        public bool Ativo { get; set; }

        public IEnumerable<Fase> Fases { get; set; }
    }
}