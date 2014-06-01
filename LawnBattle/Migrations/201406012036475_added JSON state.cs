namespace LawnBattle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedJSONstate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tournament", "JSONstate", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tournament", "JSONstate");
        }
    }
}
