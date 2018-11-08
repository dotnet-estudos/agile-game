namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Board",
                c => new
                    {
                        BoardId = c.Int(nullable: false, identity: true),
                        Nome = c.String(maxLength: 100, unicode: false),
                        Ativo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.BoardId);
            
            CreateTable(
                "dbo.Equipe",
                c => new
                    {
                        EquipeId = c.Int(nullable: false, identity: true),
                        Hash = c.String(maxLength: 100, unicode: false),
                        Nome = c.String(maxLength: 100, unicode: false),
                        JogoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EquipeId)
                .ForeignKey("dbo.Jogo", t => t.JogoId)
                .Index(t => t.JogoId);
            
            CreateTable(
                "dbo.Jogo",
                c => new
                    {
                        JogoId = c.Int(nullable: false, identity: true),
                        BoardId = c.Int(nullable: false),
                        NomeTurma = c.String(maxLength: 100, unicode: false),
                        QuantidadeEquipes = c.Int(nullable: false),
                        DataCadastro = c.DateTime(nullable: false),
                        Finalizado = c.Boolean(nullable: false),
                        DataHoraFim = c.DateTime(),
                    })
                .PrimaryKey(t => t.JogoId)
                .ForeignKey("dbo.Board", t => t.BoardId)
                .Index(t => t.BoardId);
            
            CreateTable(
                "dbo.Fase",
                c => new
                    {
                        FaseId = c.Int(nullable: false, identity: true),
                        BoardId = c.Int(nullable: false),
                        Nome = c.String(maxLength: 100, unicode: false),
                        RGB = c.String(maxLength: 100, unicode: false),
                        Ordem = c.Int(nullable: false),
                        Ativo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.FaseId)
                .ForeignKey("dbo.Board", t => t.BoardId)
                .Index(t => t.BoardId);
            
            CreateTable(
                "dbo.Kaizen",
                c => new
                    {
                        KaizenId = c.Int(nullable: false, identity: true),
                        Letra = c.String(maxLength: 100, unicode: false),
                        Descricao = c.String(maxLength: 100, unicode: false),
                        Esforco = c.Int(nullable: false),
                        Repeticao = c.Int(nullable: false),
                        Requisito = c.String(maxLength: 100, unicode: false),
                        Ativo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.KaizenId);
            
            CreateTable(
                "dbo.LancamentoBoard",
                c => new
                    {
                        LancamentoBoardId = c.Int(nullable: false, identity: true),
                        JogoId = c.Int(nullable: false),
                        EquipeId = c.Int(nullable: false),
                        DiaLancamento = c.Int(nullable: false),
                        FaseId = c.Int(nullable: false),
                        Quantidade = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.LancamentoBoardId)
                .ForeignKey("dbo.Equipe", t => t.EquipeId)
                .ForeignKey("dbo.Fase", t => t.FaseId)
                .ForeignKey("dbo.Jogo", t => t.JogoId)
                .Index(t => t.JogoId)
                .Index(t => t.EquipeId)
                .Index(t => t.FaseId);
            
            CreateTable(
                "dbo.LancamentoChamado",
                c => new
                    {
                        LancamentoChamadoId = c.Int(nullable: false, identity: true),
                        EquipeId = c.Int(nullable: false),
                        JogoId = c.Int(nullable: false),
                        ChamadoNumero = c.String(maxLength: 100, unicode: false),
                        DiaAbertura = c.Int(nullable: false),
                        DiaFechamento = c.Int(nullable: false),
                        DataCadastro = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.LancamentoChamadoId)
                .ForeignKey("dbo.Equipe", t => t.EquipeId)
                .ForeignKey("dbo.Jogo", t => t.JogoId)
                .Index(t => t.EquipeId)
                .Index(t => t.JogoId);
            
            CreateTable(
                "dbo.LancamentoKaizen",
                c => new
                    {
                        LancamentoKaizenId = c.Int(nullable: false, identity: true),
                        KaizenId = c.Int(nullable: false),
                        JogoId = c.Int(nullable: false),
                        EquipeId = c.Int(nullable: false),
                        DiaConlcusao = c.Int(nullable: false),
                        DiaInicio = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.LancamentoKaizenId);
            
            CreateTable(
                "dbo.LogGame",
                c => new
                    {
                        LogGameId = c.Int(nullable: false, identity: true),
                        Tabela = c.String(maxLength: 100, unicode: false),
                        Acao = c.String(maxLength: 100, unicode: false),
                        NomeUsuario = c.String(maxLength: 100, unicode: false),
                        Chaves = c.String(maxLength: 100, unicode: false),
                        NomeColuna = c.String(maxLength: 100, unicode: false),
                        Propriedade = c.String(maxLength: 100, unicode: false),
                        ValorOriginal = c.String(maxLength: 100, unicode: false),
                        ValorAtual = c.String(maxLength: 100, unicode: false),
                        DataOcorrencia = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.LogGameId);
            
            CreateTable(
                "dbo.Usuario",
                c => new
                    {
                        UsuarioId = c.Int(nullable: false, identity: true),
                        Login = c.String(maxLength: 100, unicode: false),
                        Senha = c.String(maxLength: 100, unicode: false),
                        Email = c.String(maxLength: 100, unicode: false),
                        Ativo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.UsuarioId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LancamentoChamado", "JogoId", "dbo.Jogo");
            DropForeignKey("dbo.LancamentoChamado", "EquipeId", "dbo.Equipe");
            DropForeignKey("dbo.LancamentoBoard", "JogoId", "dbo.Jogo");
            DropForeignKey("dbo.LancamentoBoard", "FaseId", "dbo.Fase");
            DropForeignKey("dbo.LancamentoBoard", "EquipeId", "dbo.Equipe");
            DropForeignKey("dbo.Fase", "BoardId", "dbo.Board");
            DropForeignKey("dbo.Equipe", "JogoId", "dbo.Jogo");
            DropForeignKey("dbo.Jogo", "BoardId", "dbo.Board");
            DropIndex("dbo.LancamentoChamado", new[] { "JogoId" });
            DropIndex("dbo.LancamentoChamado", new[] { "EquipeId" });
            DropIndex("dbo.LancamentoBoard", new[] { "FaseId" });
            DropIndex("dbo.LancamentoBoard", new[] { "EquipeId" });
            DropIndex("dbo.LancamentoBoard", new[] { "JogoId" });
            DropIndex("dbo.Fase", new[] { "BoardId" });
            DropIndex("dbo.Jogo", new[] { "BoardId" });
            DropIndex("dbo.Equipe", new[] { "JogoId" });
            DropTable("dbo.Usuario");
            DropTable("dbo.LogGame");
            DropTable("dbo.LancamentoKaizen");
            DropTable("dbo.LancamentoChamado");
            DropTable("dbo.LancamentoBoard");
            DropTable("dbo.Kaizen");
            DropTable("dbo.Fase");
            DropTable("dbo.Jogo");
            DropTable("dbo.Equipe");
            DropTable("dbo.Board");
        }
    }
}
