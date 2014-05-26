namespace LawnBattle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Event",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        EventKey = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Player",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Email = c.String(),
                        Event_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Event", t => t.Event_ID)
                .Index(t => t.Event_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Player", "Event_ID", "dbo.Event");
            DropIndex("dbo.Player", new[] { "Event_ID" });
            DropTable("dbo.Player");
            DropTable("dbo.Event");
        }
    }
}
