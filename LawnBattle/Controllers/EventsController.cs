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
    public class EventsController : Controller
    {
        private LawnBattleContext db = new LawnBattleContext();

        // GET: /Event/
        public ActionResult Index()
        {
            return View(db.Events.ToList());
        }

        public ActionResult ReRoute()
        {
            if(Request.Form["EventKey"] != null && Request.Form["EventKey"] != "")
            {
                string EvenyKey = Request.Form["EventKey"].ToString();
                var GetEvent = db.Events.Where(x => x.EventKey.Equals(EvenyKey)).FirstOrDefault();

                if (GetEvent != null)
                {
                    return Redirect("/events/" + Request.Form["EventKey"]);
                }
                else
                    return Redirect("/?Found=Nope");
            }
            return View(db.Events.ToList());
        }

        // GET: /Event/Details/5
        public ActionResult eventBySlug(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Where(x => x.EventKey.Equals(id)).FirstOrDefault();
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View("Details",@event);
        }

        // GET: /Event/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // GET: /Event/Create
        public ActionResult Create()
        {
            if(Request.QueryString["Found"] != null)
                ModelState.AddModelError("", "Event Code Not Found");
            return View();
        }

        // POST: /Event/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,Name,EventKey")] Event @event)
        {
            if (ModelState.IsValid)
            {
                @event.EventKey = @event.Name;
                db.Events.Add(@event);
                db.SaveChanges();
                return Redirect("/events/" + @event.EventKey);
                //return RedirectToAction("Index");
            }

            return View(@event);
        }

        // GET: /Event/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: /Event/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,Name,EventKey")] Event @event)
        {
            if (ModelState.IsValid)
            {
                db.Entry(@event).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(@event);
        }

        // GET: /Event/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: /Event/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Event ThisEvent = db.Events.Find(id);

            var GetTournaments = db.Tournaments.Where(x => x.Event.ID.Equals(ThisEvent.ID)).ToList();
            foreach (var ThisTourny in GetTournaments)
            {
                var GetGames = db.Games.Where(x => x.Tournament.ID.Equals(ThisTourny.ID)).ToList();
                foreach (var ThisGame in GetGames)
                {
                    db.Games.Remove(ThisGame);
                    db.SaveChanges();
                }

                var GetTeams = db.Teams.Where(x => x.Tournament.ID.Equals(ThisTourny.ID)).ToList();
                foreach (var ThisTeam in GetTeams)
                {
                    db.Teams.Remove(ThisTeam);
                    db.SaveChanges();
                }

                db.Tournaments.Remove(ThisTourny);
                db.SaveChanges();
            }

            var GetPlayers = db.Players.Where(x => x.Event.ID.Equals(ThisEvent.ID)).ToList();
            foreach (var ThisPlayer in GetPlayers)
            {
                db.Players.Remove(ThisPlayer);
                db.SaveChanges();
            }

            db.Events.Remove(ThisEvent);
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
