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
        public int GameStatus { get; set; }

        public int Team1Score { get; set; }
        public int Team2Score { get; set; }

        public virtual Tournament Tournament { get; set; }
        public virtual Team Team1 { get; set; }
        public virtual Team Team2 { get; set; }
    }
}