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
    public class PlayersController : Controller
    {
        private LawnBattleContext db = new LawnBattleContext();

        // GET: /Players/
        public ActionResult Index(string eventSlug)
        {
            ViewBag.eventSlug = eventSlug;

            int TournyCount = 0;
            var GetEvent = db.Events.Where(x => x.EventKey.Equals(eventSlug)).Include("Tournaments").FirstOrDefault();
            if (GetEvent != null)
            {
                if (GetEvent.Tournaments != null)
                    TournyCount = GetEvent.Tournaments.Count;
            }

            ViewBag.TournyCount = TournyCount;

            return View(db.Players.Where(x => x.Event.EventKey.Equals(eventSlug)).ToList());
        }

        // GET: /Players/Details/5
        public ActionResult Details(string eventSlug, int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Player player = db.Players.Find(id);
            if (player == null)
            {
                return HttpNotFound();
            }
            return View(player);
        }

        // GET: /Players/Create
        public ActionResult Create(string eventSlug)
        {
            ViewBag.eventSlug = eventSlug;
            return View();
        }

        // POST: /Players/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string eventSlug, [Bind(Include="ID,Name")] Player player)
        {
            player.Email = "";
            player.IsActive = true;
            player.IsHuman = true;

            var ThesePlayers = Request.Form["Name"].Split(',').ToList();

            ViewBag.eventSlug = eventSlug;
            var GetEvent = db.Events.Where(x => x.EventKey.Equals(eventSlug)).FirstOrDefault();

            if (ModelState.IsValid && ThesePlayers.Count > 0)
            {
                //create a player for of the names entered
                foreach (string ThisName in ThesePlayers)
                {
                    if(ThisName != "")
                    {
                        Player ThisPlayer = new Player { Name = ThisName, IsHuman = true, IsActive = true, Email = "", Event = GetEvent };
                        db.Players.Add(ThisPlayer);
                        db.SaveChanges();
                    }
                }
                //player.Event = GetEvent;
                //db.Players.Add(player);
                //db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(player);
        }

        // GET: /Players/Edit/5
        public ActionResult Edit(string eventSlug, int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Player player = db.Players.Find(id);
            if (player == null)
            {
                return HttpNotFound();
            }
            return View(player);
        }

        // POST: /Players/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string eventSlug, [Bind(Include="ID,Name,Email,IsActive,IsHuman")] Player player)
        {
            if (ModelState.IsValid)
            {
                db.Entry(player).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(player);
        }

        // GET: /Players/Delete/5
        public ActionResult Delete(string eventSlug, int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Player player = db.Players.Find(id);
            if (player == null)
            {
                return HttpNotFound();
            }
            return View(player);
        }

        // POST: /Players/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string eventSlug, int id)
        {
            Player player = db.Players.Find(id);
            db.Players.Remove(player);
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
