namespace LawnBattle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dataattributes : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Event", "EventKey", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Event", "EventKey", c => c.String(nullable: false));
        }
    }
}
