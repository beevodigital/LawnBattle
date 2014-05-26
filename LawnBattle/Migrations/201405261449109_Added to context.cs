namespace LawnBattle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addedtocontext : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Game",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        GameSlot = c.Int(nullable: false),
                        Team1_ID = c.Int(),
                        Team2_ID = c.Int(),
                        Tournament_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Team", t => t.Team1_ID)
                .ForeignKey("dbo.Team", t => t.Team2_ID)
                .ForeignKey("dbo.Tournament", t => t.Tournament_ID)
                .Index(t => t.Team1_ID)
                .Index(t => t.Team2_ID)
                .Index(t => t.Tournament_ID);
            
            AddColumn("dbo.Player", "Gender", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Game", "Tournament_ID", "dbo.Tournament");
            DropForeignKey("dbo.Game", "Team2_ID", "dbo.Team");
            DropForeignKey("dbo.Game", "Team1_ID", "dbo.Team");
            DropIndex("dbo.Game", new[] { "Tournament_ID" });
            DropIndex("dbo.Game", new[] { "Team2_ID" });
            DropIndex("dbo.Game", new[] { "Team1_ID" });
            DropColumn("dbo.Player", "Gender");
            DropTable("dbo.Game");
        }
    }
}
