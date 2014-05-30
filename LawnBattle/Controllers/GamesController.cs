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
    public class GamesController : Controller
    {
        private LawnBattleContext db = new LawnBattleContext();

        // GET: /Games/
        public ActionResult Index()
        {
            return View(db.Games.ToList());
        }

        // GET: /Games/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Game game = db.Games.Find(id);
            if (game == null)
            {
                return HttpNotFound();
            }
            return View(game);
        }

        // GET: /Games/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Games/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,GameSlot,GameStatus")] Game game)
        {
            if (ModelState.IsValid)
            {
                db.Games.Add(game);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(game);
        }

        // GET: /Games/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Game game = db.Games.Find(id);
            if (game == null)
            {
                return HttpNotFound();
            }
            return View(game);
        }

        // POST: /Games/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,GameSlot,GameStatus, Team1Score, Team2Score")] Game game)
        {
            if (ModelState.IsValid)
            {
                //set the game to in progress
                game.GameStatus = (int)LawnBattle.Helpers.enums.GameStatus.InProgress;

                //did they mark it as complete? 
                if(Request.Form["IsComplete"] != null)
                {
                    game.GameStatus = (int)LawnBattle.Helpers.enums.GameStatus.Complete;

                    db.Entry(game).State = EntityState.Modified;
                    db.SaveChanges();

                    //need to grab the game from the db to get lazy objects
                    var LoadGame = db.Games.Where(x => x.ID.Equals(game.ID)).Include(x => x.Tournament).Include(x => x.Team1).Include(x => x.Team2).FirstOrDefault();

                    //since it is complete, we need to move the winner over to the next game
                    //ONLY if the current game slot is not 2 (championship round)
                    if(game.GameSlot < (LoadGame.Tournament.Games.Count - 1))
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

                        if(NewGame.Team1 != null && NewGame.Team2 != null)
                            NewGame.GameStatus = (int)LawnBattle.Helpers.enums.GameStatus.New;

                        db.Entry(NewGame).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    string stophere = "";
                }
                else
                {
                    db.Entry(game).State = EntityState.Modified;
                    db.SaveChanges();
                }

                //TODO: bad to look up again...
                var ReloadGame = db.Games.Where(x => x.ID.Equals(game.ID)).Include(x => x.Tournament).Include(x => x.Team1).Include(x => x.Team2).FirstOrDefault();
                return RedirectToRoute("EventsTournaments", new { eventSlug = ReloadGame.Tournament.Event.EventKey, action = "details", id = ReloadGame.Tournament.ID });
                return RedirectToAction("Index");
            }
            return View(game);
        }

        // GET: /Games/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Game game = db.Games.Find(id);
            if (game == null)
            {
                return HttpNotFound();
            }
            return View(game);
        }

        // POST: /Games/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Game game = db.Games.Find(id);
            db.Games.Remove(game);
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
