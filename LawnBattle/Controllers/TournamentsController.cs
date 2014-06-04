using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LawnBattle.Models;

namespace LawnBattle.Controllers
{
    public class TournamentsController : Controller
    {
        private LawnBattleContext db = new LawnBattleContext();

        // GET: /Tournament/
        public ActionResult Index(string eventSlug)
        {
            ViewBag.eventSlug = eventSlug;

            return View(db.Tournaments.Where(x => x.Event.EventKey.Equals(eventSlug)).ToList());
        }

        // GET: /Tournament/Details/5
        public ActionResult Details(string eventSlug, int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tournament tournament = db.Tournaments.Find(id);
            if (tournament == null)
            {
                return HttpNotFound();
            }
            ViewBag.eventSlug = eventSlug;
            return View(tournament);
        }

        [HttpPost]
        public ActionResult SaveJSON(string eventSlug, int id)
        {
            //look up tourny
            var GetTourny = db.Tournaments.Where(x => x.ID.Equals(id)).Where(x => x.Event.EventKey.Equals(eventSlug)).FirstOrDefault();

            if (GetTourny != null)
            {
                GetTourny.JSONstate = Request.Form["JSONScore"];
                db.Entry(GetTourny).State = EntityState.Modified;
                db.SaveChanges();
            }
            return Content("IN");
        }

        // GET: /Tournament/Create
        public ActionResult Create(string eventSlug)
        {
            //get all of the players in this event

            ViewBag.Players = db.Players.Where(x => x.Event.EventKey.Equals(eventSlug)).ToList();
            ViewBag.eventSlug = eventSlug;
            return View();
        }

        // POST: /Tournament/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string eventSlug, [Bind(Include="ID,TournamentType,Name")] Tournament tournament)
        {
            ViewBag.eventSlug = eventSlug;

            if (Request.Form["TournamentPlayers"] == null)
            {
                ModelState.AddModelError("", "You must select at least 4 teams");
                ViewBag.Players = db.Players.Where(x => x.Event.EventKey.Equals(eventSlug)).ToList();
                return View(tournament);
            }

            var ThesePlayers = Request.Form["TournamentPlayers"].Split(',').ToList();

            //make sure there are an even number of players
            if(ThesePlayers.Count < 4 || ThesePlayers.Count % 2 != 0)
            { 
                ModelState.AddModelError("", "You must have at least 4 players and an event amount");
                ViewBag.Players = db.Players.Where(x => x.Event.EventKey.Equals(eventSlug)).ToList();
                return View(tournament);
            }

            //one more check. If it is valid and has 4 teams, make sure it is set to single elim
            if(ThesePlayers.Count == 4 && tournament.TournamentType == 2)
            {
                tournament.TournamentType = 3;
            }

            //look up the event
            var GetEvent = db.Events.Where(x => x.EventKey.Equals(eventSlug)).FirstOrDefault();

            if (ModelState.IsValid && GetEvent != null)
            {
                tournament.Event = GetEvent;
                db.Tournaments.Add(tournament);
                db.SaveChanges();

                //now that the tournament is created, we need to:
                //1) make teams
                //2) create games

                //make teams (this will only make REAL teams):
                //look up the players that have been selected
                List<Player> GetPlayers = new List<Player>();
                

                foreach(var ThisPlayer in ThesePlayers)
                {
                    int ThisPlayerID = int.Parse(ThisPlayer);
                    var GetPlayer = db.Players.Find(ThisPlayerID);
                    if (GetPlayer != null)
                        GetPlayers.Add(GetPlayer);
                }

                //var GetPlayers = db.Players.Where(x => x.Event.EventKey.Equals(eventSlug)).ToList();
                LawnBattle.Helpers.TeamHelper ThisTeamHelper = new Helpers.TeamHelper();
                var MakeTeams = ThisTeamHelper.CreateRandomTeams(GetPlayers);

                //add the teams to the database
                int teamNumber = 1;
                foreach (var ThisTeam in MakeTeams)
                {
                    Team NewTeam = new Team { IsHuman = true, Name = "Team" + teamNumber.ToString(), Player1 = ThisTeam.Player1, Player2 = ThisTeam.Player2, Tournament = tournament };
                    db.Teams.Add(NewTeam);
                    db.SaveChanges();
                    teamNumber++;
                }
                //regrab them from the db
                MakeTeams = db.Teams.Where(x => x.Tournament.ID.Equals(tournament.ID)).Where(x => x.IsHuman.Equals(true)).ToList();

                //find out what size tournament this is
                int BracketSize = 1;

                while (BracketSize < MakeTeams.Count)
                    BracketSize *= 2;

                //MakeTeams = real teams
                //Bracksize = first round games
                //needed fale teams = bracketsize - MakeTeams.count
                

                //at this point, we have all of our teams!
                //get all the teams in once list
                //List<Team> AllTeams = MakeTeams.Concat(FakeTeams).ToList();
                //start creating games
                //Double Elimination
                if(tournament.TournamentType == 2)
                {
                    var FakeTeams = ThisTeamHelper.CreateFakeTeams(BracketSize - MakeTeams.Count);
                    foreach (var ThisTeam in FakeTeams)
                    {
                        Team NewTeam = new Team { IsHuman = false, Tournament = tournament };
                        db.Teams.Add(NewTeam);
                        db.SaveChanges();
                    }
                    FakeTeams = db.Teams.Where(x => x.Tournament.ID.Equals(tournament.ID)).Where(x => x.IsHuman.Equals(false)).ToList();

                    var CreatedGames = ThisTeamHelper.EliminationCreateGames(MakeTeams, FakeTeams, BracketSize);

                    foreach (var ThisGame in CreatedGames)
                    {
                        ThisGame.Tournament = tournament;
                        db.Games.Add(ThisGame);
                        db.SaveChanges();
                    }


                    //send them to the page
                    return RedirectToRoute("EventsTournaments", new { eventSlug = eventSlug, action = "details", id = tournament.ID });
                }
                else if (tournament.TournamentType == 3)
                {
                    var FakeTeams = ThisTeamHelper.CreateFakeTeams(BracketSize - MakeTeams.Count);
                    foreach (var ThisTeam in FakeTeams)
                    {
                        Team NewTeam = new Team { IsHuman = false, Tournament = tournament };
                        db.Teams.Add(NewTeam);
                        db.SaveChanges();
                    }
                    FakeTeams = db.Teams.Where(x => x.Tournament.ID.Equals(tournament.ID)).Where(x => x.IsHuman.Equals(false)).ToList();

                    var CreatedGames = ThisTeamHelper.EliminationCreateGames(MakeTeams, FakeTeams, BracketSize);

                    foreach (var ThisGame in CreatedGames)
                    {
                        ThisGame.Tournament = tournament;
                        db.Games.Add(ThisGame);
                        db.SaveChanges();
                    }


                    //send them to the page
                    return RedirectToRoute("EventsTournaments", new { eventSlug = eventSlug, action = "details", id = tournament.ID });
                }
                else if (tournament.TournamentType == 4)
                {
                    //round robin
                    var CreatedGames = ThisTeamHelper.RoundRobinCreateGames(MakeTeams);

                    foreach (var ThisGame in CreatedGames)
                    {
                        ThisGame.Tournament = tournament;
                        db.Games.Add(ThisGame);
                        db.SaveChanges();
                    }


                    //send them to the page
                    return RedirectToRoute("EventsTournaments", new { eventSlug = eventSlug, action = "details", id = tournament.ID });
                }
                //this is the first try at a c# based single elimination
                if(tournament.TournamentType == 1)
                {
                   

                    //find games against not human players and advance them
                    var GetFakeGames = db.Games.Where(x => x.Team1.IsHuman.Equals(false) || x.Team2.IsHuman.Equals(false)).ToList();

                    foreach(var ThisGame in GetFakeGames)
                    {
                        //we need to update this game, and then pass it on
                        ThisGame.GameStatus = (int)LawnBattle.Helpers.enums.GameStatus.Complete;
                        //what team won?
                        if(ThisGame.Team1.IsHuman)
                        {
                            ThisGame.Team1Score = 1;
                            ThisGame.Team2Score = 0;
                        }
                        else
                        {
                            ThisGame.Team1Score = 0;
                            ThisGame.Team2Score = 0;
                        }

                        db.Entry(ThisGame).State = EntityState.Modified;
                        db.SaveChanges();

                        ThisTeamHelper.AdvanceToGame(ThisGame);
                        
                    }

                    string stopHere = "";
                }

               

                return RedirectToAction("Index");
            }

            return View(tournament);
        }

        // GET: /Tournament/Edit/5
        public ActionResult Edit(string eventSlug, int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tournament tournament = db.Tournaments.Find(id);
            if (tournament == null)
            {
                return HttpNotFound();
            }
            return View(tournament);
        }

        // POST: /Tournament/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,TournamentType")] Tournament tournament)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tournament).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tournament);
        }

        // GET: /Tournament/Delete/5
        public ActionResult Delete(string eventSlug, int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tournament tournament = db.Tournaments.Find(id);
            if (tournament == null)
            {
                return HttpNotFound();
            }
            return View(tournament);
        }

        // POST: /Tournament/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string eventSlug,int id)
        {
            Tournament tournament = db.Tournaments.Find(id);

            //my own cascase delete.
            //TODO: use real cascade later

            var GetGames = db.Games.Where(x => x.Tournament.ID.Equals(id)).ToList();
            foreach (var ThisGame in GetGames)
            {
                db.Games.Remove(ThisGame);
                db.SaveChanges();
            }

            var GetTeams = db.Teams.Where(x => x.Tournament.ID.Equals(id)).ToList();
            foreach (var ThisTeam in GetTeams)
            {
                db.Teams.Remove(ThisTeam);
                db.SaveChanges();
            }

            db.Tournaments.Remove(tournament);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
