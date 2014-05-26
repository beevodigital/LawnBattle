using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LawnBattle.Models;

namespace LawnBattle.Library.Team
{
    public class helper
    {
        protected void CreateRandomTeams(List<Player> HumanPlayers)
        {
            List<LawnBattle.Models.Team> HumanTeams = new List<LawnBattle.Models.Team>();
            //hardcoding the # of needed players to 16 for now
            //we want to create 8 teams of two. we should always seed with our fake players first
            List<Player> FakePlayers = new List<Player>();
            Random rnd = new Random();
            HumanPlayers = (from item in HumanPlayers orderby rnd.Next() select item).ToList(); 

            //TODO: put a check in for even number of players
            int NumberOfRealTeams = (HumanPlayers.Count / 2);
            for (int i = 0; i < NumberOfRealTeams; i++) 
            {
                LawnBattle.Models.Team ThisTeam = new Models.Team();
                ThisTeam.Name = "Team " + (i + 1).ToString();
                ThisTeam.Player1 = HumanPlayers.ElementAt(0);
                HumanPlayers.RemoveAt(0);
                ThisTeam.Player2 = HumanPlayers.ElementAt(0);
                HumanPlayers.RemoveAt(0);
                HumanTeams.Add(ThisTeam);
            }

            string stopHere = "";

            //I got ahead of myself below. First, just create teams.... we need humans to be with humans...
            /*
            int NumberOfFakePlayers = (16 - HumanPlayers.Count);
            if(NumberOfFakePlayers > 0)
            {
                for (int i = 0; i < NumberOfFakePlayers; i++)
                {
                    FakePlayers.Add(new Player { Name = "Bye", IsActive = true, IsHuman = false });
                }
            }

            //we now have two lists. One of real players and one of fake players

            //we should create teams in the orders of seeding: 1, 16, 2, 15 , 2, 14 etc
            List<int> TeamStagger = new List<int>();
            TeamStagger.Add(0);

            //first, lets randomize our real player list
            Random rnd = new Random();
            HumanPlayers = (from item in HumanPlayers orderby rnd.Next() select item).ToList();   
    
            //we know we are creating 8 teams, so lets get our loop setup....
            for (int i = 0; i < 8; i++)
            {

            }
             */
        }
    }
}
