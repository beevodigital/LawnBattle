namespace LawnBattle.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using LawnBattle.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<LawnBattle.Models.LawnBattleContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(LawnBattle.Models.LawnBattleContext context)
        {
            

        }
    }
}
