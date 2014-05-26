namespace LawnBattle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedGender : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Player", "Gender");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Player", "Gender", c => c.Int(nullable: false));
        }
    }
}
