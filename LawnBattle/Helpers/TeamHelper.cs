using System;
using System.Data;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LawnBattle.Models;

namespace LawnBattle.Helpers
{
    public class TeamHelper
    {
        private LawnBattleContext db = new LawnBattleContext();

        public List<LawnBattle.Models.Team> CreateRandomTeams(List<Player> HumanPlayers)
        {
            List<LawnBattle.Models.Team> HumanTeams = new List<LawnBattle.Models.Team>();
            List<LawnBattle.Models.Team> FakeTeams = new List<LawnBattle.Models.Team>();
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
                ThisTeam.IsHuman = true;
                ThisTeam.Player1 = HumanPlayers.ElementAt(0);
                HumanPlayers.RemoveAt(0);
                ThisTeam.Player2 = HumanPlayers.ElementAt(0);
                HumanPlayers.RemoveAt(0);
                HumanTeams.Add(ThisTeam);
            }


            //*************************** returnin teams. more good code below for creating fake teams and games. Will get pulled out later.
            return HumanTeams;



            //at this point, we have teams! now we need to figure out our fake teams:
            Player FakePlayer = new Player{IsActive = true, IsHuman = false, Name = "Bye"};
            int NumberOfFakeTeams = (8 - HumanTeams.Count);
            if(NumberOfFakeTeams > 0)
            {
                for(int i = 0; i < NumberOfFakeTeams; i++)
                {
                    Team ThisTeam = new Team { IsHuman = false };
                    FakeTeams.Add(ThisTeam);
                }
            }

            //we have human teams and fake teams now

            //create the first 4 games...
            List<Game> TheseGames = new List<Game>();
            for(int i = 0; i < 4; i++)
            {
                //creat the teams. 
                Game ThisGame = new Game();
                //are there humans for team 1?
                if(HumanTeams.Count > 0)
                {
                    ThisGame.Team1 = HumanTeams.ElementAt(0);
                    HumanTeams.RemoveAt(0);
                }
                else if(FakeTeams.Count > 0)
                {
                    ThisGame.Team1 = FakeTeams.ElementAt(0);
                    FakeTeams.RemoveAt(0);
                }

                //for team 2, we want to fill with fakes if possible
                if (FakeTeams.Count > 0)
                {
                    ThisGame.Team2 = FakeTeams.ElementAt(0);
                    FakeTeams.RemoveAt(0);
                } 
                else if (HumanTeams.Count > 0)
                {
                    ThisGame.Team2 = HumanTeams.ElementAt(0);
                    HumanTeams.RemoveAt(0);
                }
                
                //now, where do we stick this newly created game?
                //TODO: this is a bit hack
                if (i == 0)
                    TheseGames.Add(ThisGame);
                else if (i == 1)
                    TheseGames.Add(ThisGame);
                else if (i == 2)
                    TheseGames.Insert(1, ThisGame);
                else if (i == 3)
                    TheseGames.Insert(2, ThisGame);

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
        
        public List<LawnBattle.Models.Team> CreateFakeTeams(int NumberOfTemas)
        {
            List<LawnBattle.Models.Team> FakeTeams = new List<Team>();

            if (NumberOfTemas > 0)
            {
                for (int i = 0; i < NumberOfTemas; i++)
                {
                    Team ThisTeam = new Team { IsHuman = false };
                    FakeTeams.Add(ThisTeam);
                }
            }

            return FakeTeams;
        }

        public void AdvanceToGame(Game game)
        {
            //need to grab the game from the db to get lazy objects
            var LoadGame = db.Games.Where(x => x.ID.Equals(game.ID)).Include(x => x.Tournament).Include(x => x.Team1).Include(x => x.Team2).FirstOrDefault();

            //since it is complete, we need to move the winner over to the next game
            //ONLY if the current game slot is not 2 (championship round)
            if (game.GameSlot != 2)
            {
                var WinningTeam = new Team();
                if (game.Team1Score > game.Team2Score)
                    WinningTeam = LoadGame.Team1;
                else
                    WinningTeam = LoadGame.Team2;

                int CurrentGameSlot = game.GameSlot + 1;
                int NumberOfTeams = LoadGame.Tournament.Teams.Count;

                int NewGameSlot = ((NumberOfTeams / 2) + (int)Math.Ceiling((double)CurrentGameSlot / 2)) - 1;

                //grab the new Game by slot and update date
                var NewGame = db.Games.Where(x => x.GameSlot.Equals(NewGameSlot)).Where(x => x.Tournament.ID.Equals(LoadGame.Tournament.ID)).FirstOrDefault();
                if (game.GameSlot % 2 == 0)
                    NewGame.Team1 = WinningTeam;
                else
                    NewGame.Team2 = WinningTeam;

                if (NewGame.Team1 != null && NewGame.Team2 != null)
                    NewGame.GameStatus = (int)LawnBattle.Helpers.enums.GameStatus.New;

                db.Entry(NewGame).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public List<Game> EliminationCreateGames(List<Team> HumanTeams, List<Team> FakeTeams, int BracketSize)
        {
            //we need to create our first round games
            //BracketSize / 2
            //Populate those with real teams
            List<Game> TheseGames = new List<Game>();
            int ThisGameCounter = 0;

            for (int i = 0; i < (BracketSize - 1); i++ )
            {
                if(i < (BracketSize / 2))
                {
                    //creating real games
                    //creat the teams. 
                    Game ThisGame = new Game();
                    ThisGame.GameStatus = (int)enums.GameStatus.New;
                    //are there humans for team 1?
                    if (HumanTeams.Count > 0)
                    {
                        ThisGame.Team1 = HumanTeams.ElementAt(0);
                        HumanTeams.RemoveAt(0);
                    }
                    else if (FakeTeams.Count > 0)
                    {
                        ThisGame.Team1 = FakeTeams.ElementAt(0);
                        FakeTeams.RemoveAt(0);
                    }

                    //for team 2, we want to fill with fakes if possible
                    if (FakeTeams.Count > 0)
                    {
                        ThisGame.Team2 = FakeTeams.ElementAt(0);
                        FakeTeams.RemoveAt(0);
                    }
                    else if (HumanTeams.Count > 0)
                    {
                        ThisGame.Team2 = HumanTeams.ElementAt(0);
                        HumanTeams.RemoveAt(0);
                    }

                    TheseGames.Insert(ThisGameCounter, ThisGame);
                    if (i % 2 == 0)
                        ThisGameCounter++;
                    string stopHere = "";
                }
                else
                {
                    //creating empty games
                    Game ThisEmptyGame = new Game();
                    ThisEmptyGame.GameStatus = (int)enums.GameStatus.Waiting;
                    //TODO: stopped creating the empty games for now
                    //TheseGames.Add(ThisEmptyGame);
                }
                //Number the game slots
                int GameCounter = 0;
                foreach(var ThisGame in TheseGames)
                {
                    ThisGame.GameSlot = GameCounter;
                    GameCounter++;
                }
            }

                //after that create empties
            return TheseGames;
        }
    }

    public class TeamForJSBracket
    {
        public string Name { get; set; }
        public int ID { get; set; }
    }
}