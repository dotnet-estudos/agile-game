namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Atualizacao_beta : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.LancamentoKaizen", "KaizenId");
            CreateIndex("dbo.LancamentoKaizen", "EquipeId");
            AddForeignKey("dbo.LancamentoKaizen", "EquipeId", "dbo.Equipe", "EquipeId");
            AddForeignKey("dbo.LancamentoKaizen", "KaizenId", "dbo.Kaizen", "KaizenId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LancamentoKaizen", "KaizenId", "dbo.Kaizen");
            DropForeignKey("dbo.LancamentoKaizen", "EquipeId", "dbo.Equipe");
            DropIndex("dbo.LancamentoKaizen", new[] { "EquipeId" });
            DropIndex("dbo.LancamentoKaizen", new[] { "KaizenId" });
        }
    }
}
