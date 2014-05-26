using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LawnBattle.Models
{
    public class Game
    {
        public int ID { get; set; }
        public int GameSlot { get; set; }

        public virtual Tournament Tournament { get; set; }
        public virtual Team Team1 { get; set; }
        public virtual Team Team2 { get; set; }
    }
}