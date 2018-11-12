using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entidade
{
    public class LancamentoKaizen
    {
        public int LancamentoKaizenId { get; set; }
        public int KaizenId { get; set; }
        [NotMapped]
        public string Letra { get; set; }
        public int JogoId { get; set; }
        public int EquipeId { get; set; }
        public int DiaConlcusao { get; set; }
        public int DiaInicio { get; set; }

        public Jogo Jogo { get; set; }
        public Equipe Equipe { get; set; }
        public Kaizen Kaizen { get; set; }
    }
}