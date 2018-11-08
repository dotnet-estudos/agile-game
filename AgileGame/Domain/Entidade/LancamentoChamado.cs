using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entidade
{
    public class LancamentoChamado
    {
        public int LancamentoChamadoId { get; set; }
        public int EquipeId { get; set; }
        public int  JogoId { get; set; }
        public string ChamadoNumero { get; set; }
        public int DiaAbertura { get; set; }
        public int  DiaFechamento { get; set; }
        public DateTime DataCadastro { get; set; }
        
        public Jogo Jogo { get; set; }
        public Equipe Equipe { get; set; }


    }
}
