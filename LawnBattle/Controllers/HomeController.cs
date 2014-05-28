using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using LawnBattle.Models;

namespace LawnBattle.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            LawnBattleContext ThisContext = new LawnBattleContext();

            //testing
            var GetPlayers = ThisContext.Players.Take(12).ToList();
            LawnBattle.Helpers.TeamHelper ThisTeamHelper = new Helpers.TeamHelper();
            ThisTeamHelper.CreateRandomTeams(GetPlayers);

            
            return View();
        }

        public ActionResult TestEvent()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}