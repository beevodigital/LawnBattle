﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace LawnBattle.Models
{
    public class Tournament
    {
        public int ID { get; set; }
        public int TournamentType { get; set; }
        [Required]
        public string Name { get; set; }

        public string JSONstate { get; set; }
        
        public virtual Event Event { get; set; }
        public virtual ICollection<Team> Teams { get; set; }
        public virtual ICollection<Game> Games { get; set; }
    }
}