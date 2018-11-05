namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Alteracao_fase_cor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Fase", "RGB", c => c.String(maxLength: 100, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Fase", "RGB");
        }
    }
}
