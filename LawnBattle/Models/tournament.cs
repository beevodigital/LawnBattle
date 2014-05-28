using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LawnBattle.Models
{
    public class Tournament
    {
        public int ID { get; set; }
        public int TournamentType { get; set; }
        public string Name { get; set; }
        
        public virtual Event Event { get; set; }
        public virtual ICollection<Team> Teams { get; set; }
    }
}