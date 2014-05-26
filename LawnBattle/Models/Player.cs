using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LawnBattle.Models
{
    public class Player
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public bool IsHuman { get; set; }
        
        public virtual Event Event { get; set; }
    }
}