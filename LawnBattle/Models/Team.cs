using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LawnBattle.Models
{
    public class Team
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsHuman { get; set; }

        public virtual Player Player1 { get; set; }
        public virtual Player Player2 { get; set; }
        public virtual Tournament Tournament { get; set; }
    }
}