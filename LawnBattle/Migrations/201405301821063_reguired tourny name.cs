namespace LawnBattle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reguiredtournyname : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Tournament", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tournament", "Name", c => c.String());
        }
    }
}
