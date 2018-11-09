namespace Domain.Entidade
{
    public class Fase
    {
        public int FaseId { get; set; }

        public int BoardId { get; set; }

        public string Nome { get; set; }

        public string NomeAbreviado { get; set; }

        public string RGB { get; set; }

        public string FaIcon { get; set; }

        public int Ordem { get; set; }

        public bool Ativo { get; set; }

        public Board Board { get; set; }
    }
}