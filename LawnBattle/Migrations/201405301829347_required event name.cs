namespace LawnBattle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class requiredeventname : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Event", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Event", "EventKey", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Event", "EventKey", c => c.String());
            AlterColumn("dbo.Event", "Name", c => c.String());
        }
    }
}
