namespace Domain.Entidade
{
    public class LancamentoKaizen
    {
        public int LancamentoKaizenId { get; set; }
        public int KaizenId { get; set; }
        public int JogoId { get; set; }
        public int EquipeId { get; set; }
        public int DiaConlcusao { get; set; }
        public int DiaInicio { get; set; }
    }
}