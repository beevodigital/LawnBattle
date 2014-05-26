namespace LawnBattle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Moremodels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tournament",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TournamentType = c.Int(nullable: false),
                        Event_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Event", t => t.Event_ID)
                .Index(t => t.Event_ID);
            
            CreateTable(
                "dbo.Team",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Player1_ID = c.Int(),
                        Player2_ID = c.Int(),
                        Tournament_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Player", t => t.Player1_ID)
                .ForeignKey("dbo.Player", t => t.Player2_ID)
                .ForeignKey("dbo.Tournament", t => t.Tournament_ID)
                .Index(t => t.Player1_ID)
                .Index(t => t.Player2_ID)
                .Index(t => t.Tournament_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Team", "Tournament_ID", "dbo.Tournament");
            DropForeignKey("dbo.Team", "Player2_ID", "dbo.Player");
            DropForeignKey("dbo.Team", "Player1_ID", "dbo.Player");
            DropForeignKey("dbo.Tournament", "Event_ID", "dbo.Event");
            DropIndex("dbo.Team", new[] { "Tournament_ID" });
            DropIndex("dbo.Team", new[] { "Player2_ID" });
            DropIndex("dbo.Team", new[] { "Player1_ID" });
            DropIndex("dbo.Tournament", new[] { "Event_ID" });
            DropTable("dbo.Team");
            DropTable("dbo.Tournament");
        }
    }
}
