namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlteracaoFase : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Fase", "NomeAbreviado", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.Fase", "FaIcon", c => c.String(maxLength: 100, unicode: false));
            AlterColumn("dbo.Jogo", "DataCadastro", c => c.DateTime());
            CreateIndex("dbo.LancamentoKaizen", "JogoId");
            AddForeignKey("dbo.LancamentoKaizen", "JogoId", "dbo.Jogo", "JogoId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LancamentoKaizen", "JogoId", "dbo.Jogo");
            DropIndex("dbo.LancamentoKaizen", new[] { "JogoId" });
            AlterColumn("dbo.Jogo", "DataCadastro", c => c.DateTime(nullable: false));
            DropColumn("dbo.Fase", "FaIcon");
            DropColumn("dbo.Fase", "NomeAbreviado");
        }
    }
}
