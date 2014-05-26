namespace LawnBattle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nottoousre : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Team", "IsHuman", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Team", "IsHuman");
        }
    }
}
