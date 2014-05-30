namespace LawnBattle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class gamescorestomodel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Game", "Team1Score", c => c.Int(nullable: false));
            AddColumn("dbo.Game", "Team2Score", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Game", "Team2Score");
            DropColumn("dbo.Game", "Team1Score");
        }
    }
}
