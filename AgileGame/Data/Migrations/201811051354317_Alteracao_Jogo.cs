namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Alteracao_Jogo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Jogo", "NomeTurma", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.Jogo", "QuantidadeEquipes", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Jogo", "QuantidadeEquipes");
            DropColumn("dbo.Jogo", "NomeTurma");
        }
    }
}
