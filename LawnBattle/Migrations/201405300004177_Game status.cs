namespace LawnBattle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Gamestatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Game", "GameStatus", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Game", "GameStatus");
        }
    }
}
