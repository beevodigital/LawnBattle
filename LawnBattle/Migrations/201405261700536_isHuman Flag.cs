namespace LawnBattle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class isHumanFlag : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Player", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Player", "IsHuman", c => c.Boolean(nullable: false));
            DropColumn("dbo.Player", "Gender");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Player", "Gender", c => c.Int(nullable: false));
            DropColumn("dbo.Player", "IsHuman");
            DropColumn("dbo.Player", "IsActive");
        }
    }
}
