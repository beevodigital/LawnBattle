using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LawnBattle.Models
{
    public class Event
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string EventKey { get; set; }

        public virtual ICollection<Player> Players { get; set; }
        public virtual ICollection<Tournament> Tournaments { get; set; }
    }
}