namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class hash_turma : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Jogo", "Hash", c => c.String(maxLength: 100, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Jogo", "Hash");
        }
    }
}
