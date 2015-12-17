using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjetSessionWebServ2.Models
{
    public class Evenement
    {
        public int Id { get; set; }
        [Display(Name = "Name", ResourceType = typeof(GlobalRessources.EvenementRes))]
        public string Nom {get;set;}
        public enum TypeEvent
        {
           TypeTournoi,
           TypeKiosque,
           TypeSpectacle,
           TypeConference,
           TypeAutre
        };
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public TypeEvent TypeEvenement { get; set; }

        public List<PlageHoraire> PlageHoraires { get; set; }

        public List<ApplicationUser> Users { get; set; }

        [Display(Name = "Room", ResourceType = typeof(GlobalRessources.EvenementRes))]
        public Salle Salle { get; set; }

        [Display(Name = "Active", ResourceType = typeof(GlobalRessources.EvenementRes))]
        public bool Actif { get; set; }

        public virtual Congres Congres { get; set; }

        }
}