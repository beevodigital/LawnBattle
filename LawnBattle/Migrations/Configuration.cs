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
            //create a fake event
            var ThisEvent = new Event { Name = "Seed Event" };
            context.Events.Add(ThisEvent);

            context.SaveChanges();

            //lets add 6 players to this event
            //we always have to have an even number of players
            var Players = new List<Player>
            {
                new Player{Name = "Brian",Event = ThisEvent},
                new Player{Name = "Naomi",Event = ThisEvent},
                new Player{Name = "Matt",Event = ThisEvent},
                new Player{Name = "Ben",Event = ThisEvent},
                new Player{Name = "Ryan",Event = ThisEvent},
                new Player{Name = "Marc",Event = ThisEvent}
            };
            Players.ForEach(s => context.Players.Add(s));
            context.SaveChanges();

            //next we need to create a tournament
            //eventually, we will ask if this is a bracket or round robin
            //bracket we need 8 or 16 teams. Round robin we can use any number of teams

            var ThisTournament = new Tournament { Event = ThisEvent, TournamentType = 1 };
            context.Tournaments.Add(ThisTournament);

            //now we need to create teams. 
            //eventually, we will ask if teams are set, or need to be created. 
            //this is very manual right  now
            //create library to finish later

            //get all the players for this tournament
            //TODO: Add properties for active in active players
            var GetPlayers = context.Players.ToList();
            
            //pass to team builder:
            LawnBattle.Helpers.TeamHelper ThisTeamHelper = new Helpers.TeamHelper();
            ThisTeamHelper.CreateRandomTeams(GetPlayers);

            //allowed number of teams: 4, 8, 16
            //perfect number of players: 8, 16, 32 (why even support 32? It is basically never going to happen)

            //thinking as I type.... we are always going to play with 16 players, loading up the Bye players

        }
    }
}
