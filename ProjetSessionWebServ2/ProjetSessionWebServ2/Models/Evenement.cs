using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetSessionWebServ2.Models
{
    public class Evenement
    {
        public int Id { get; set; }
        public string Nom {get;set;}
        public enum TypeEvent
        {
           TypeTournoi,
           TypeKiosque,
           TypeSpectacle,
           TypeConference,
           TypeAutre
        };
        public string Description { get; set; }

        public TypeEvent TypeEvenement { get; set; }

        public List<PlageHoraire> PlageHoraires { get; set; }

        public List<ApplicationUser> Users { get; set; }

        public Salle Salle { get; set; }


    }
}