namespace LawnBattle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedNametoTourny : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tournament", "Name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tournament", "Name");
        }
    }
}
