using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using LawnBattle.Helpers.DataAnnotations;

namespace LawnBattle.Models
{
    public class Event
    {
        public int ID { get; set; }
        [Required]
        [CheckEventName]
        public string Name { get; set; }
        public string EventKey { get; set; }

        public virtual ICollection<Player> Players { get; set; }
        public virtual ICollection<Tournament> Tournaments { get; set; }
    }
}