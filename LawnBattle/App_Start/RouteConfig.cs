using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace LawnBattle
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "LeaveEvnet",
                url: "events/seeya",
                defaults: new { controller = "events", action = "seeya", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "EventsManage",
                url: "events/manage/{action}/{id}",
                defaults: new { controller = "events", action = "index", id = UrlParameter.Optional }
            );

            /*
            routes.MapRoute(
                name: "TournamentGamesBasic",
                url: "events/{eventSlug}/tournaments/game/{action}/{id}",
                defaults: new { controller = "games", action = "index", eventSlug = UrlParameter.Optional, id = UrlParameter.Optional }
            );
            */

            routes.MapRoute(
                name: "EventsTournamentGames",
                url: "events/{eventSlug}/tournaments/details/{tournamentid}/game/{action}/{id}/{winningTeam}",
                defaults: new { controller = "games", action = "index", eventSlug = UrlParameter.Optional, id = UrlParameter.Optional, winningTeam = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "EventsTournaments",
                url: "events/{eventSlug}/tournaments/{action}/{id}",
                defaults: new { controller = "tournaments", action = "index", eventSlug = UrlParameter.Optional, id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "EventsPlayers",
                url: "events/{eventSlug}/players/{action}/{id}",
                defaults: new { controller = "players", action = "index", eventSlug = UrlParameter.Optional, id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "EventSlug",
                url: "events/{id}",
                defaults: new { controller = "events", action = "eventBySlug", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "events", action = "create", id = UrlParameter.Optional }
            );
        }
    }
}
