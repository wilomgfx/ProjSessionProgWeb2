using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetSessionWebServ2.Models
{
    public class Tournoi : Evenement
    {
        public Nullable<int> TypeTournoiId { get; set; }

        public TypeTournoi TypeTournoi { get; set; }

        public virtual List<Equipe> Equipes { get; set; }

        public virtual List<Partie> Parties { get; set; } 
    }
}