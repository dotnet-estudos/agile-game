using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain.Entidade;

namespace Web.Models
{
    public class BoardViewModel
    {

        public Board Board { get; set; }

        public Equipe Equipe { get; set; }

        public Jogo jogo { get; set; }

        public Kaizen Kaizen { get; set; }

        public List<LancamentoKaizen> LancamentoKaizen { get; set; }

        public List<LancamentoBoard> LancamentoBoard { get; set; }

        public List<LancamentoChamado> LancamentoChamado { get; set; }



    }
}