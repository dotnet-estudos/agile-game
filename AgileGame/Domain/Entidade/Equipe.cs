using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entidade
{
    public class Equipe
    {
        public Equipe()
        {
            this.LancamentoBoard = new List<LancamentoBoard>();
            this.LancamentoChamado = new List<LancamentoChamado>();
            this.LancamentoKaizen = new List<LancamentoKaizen>();

        }
        
        public int EquipeId { get; set; }
        public string Nome { get; set; }

        public string Hash { get; set; }

        public int JogoId { get; set; }

        public Jogo Jogo { get; set; }


        public double GetLeadTime()
        {
            if (this.LancamentoChamado.Count > 0)
            {
                var total = LancamentoChamado.Sum(x => (x.DiaFechamento - x.DiaAbertura)) / LancamentoChamado.Count;

                return total;
            }
            return 0;
        }

        public double GetEsforcoKaizen()
        {
            if (this.LancamentoKaizen.Count > 0)
            {
                var total = LancamentoKaizen.Sum(x => x.Kaizen.Esforco);

                return total;
            }

            return 0;
        }

        public double GetRankValue()
        {

            var retorno =  (100-((this.GetLeadTime() *100)/49)) == 100 ? 0 : 100 - ((this.GetLeadTime() * 100) / 49)  + ((this.GetEsforcoKaizen()*100)/230);

            return retorno;
        }

        public double GetRankValuePercent()
        {

            var retorno = this.GetRankValue()/2;

            return retorno;
        }


        public List<LancamentoBoard> LancamentoBoard { get; set; }

        public List<LancamentoChamado> LancamentoChamado { get; set; }

        public List<LancamentoKaizen> LancamentoKaizen { get; set; }

    }
}
